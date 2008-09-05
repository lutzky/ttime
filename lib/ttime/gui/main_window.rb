require 'libglade2'
require 'singleton'

require 'ttime/data'
require 'ttime/constraints'
require 'ttime/ratings'
require 'ttime/settings'
require 'ttime/logic/course'
require 'ttime/logic/scheduler'
require 'ttime/logic/nicknames'
require 'ttime/gui/progress_dialog'
require 'ttime/gui/exam_schedule'
require 'ttime/tcal/tcal'
require 'ttime/gettext_settings'

module Gtk
  class Menu
    def add_with_callback label, &blk
      mi = Gtk::MenuItem.new label
      mi.signal_connect("activate", &blk)
      self.append mi
    end
  end
end

module TTime
  module GUI
    # Candidates for GUI data paths are given either relative to $0's directory
    # or absolutely. The first match (for any specific file) is chosen.
    DataPathCandidates = [
      '../data/ttime/',
      '/usr/share/ttime/',
      '/usr/local/share/ttime/',
    ]

    EventDataMembers = [
      [ :course_name, _("Course name") ],
      [ :course_number, _("Course number") ],
      [ :group_number, _("Group number") ],
      [ :place, _("Place") ],
      [ :lecturer, _("Lecturer") ],
    ]

    DefaultEventDataMembers = [ :course_name, :group_number, :place ]

    class << self
      def find_data_file filename
        my_path = Pathname.new($0).dirname
        DataPathCandidates.collect { |p| my_path + p + filename }.each do |path|
          return path.to_s if path.exist?
        end
        raise Errno::ENOENT.new(filename)
      end
    end

    class MainWindow
      include Singleton

      def on_auto_update
        load_data(true)
      end

      def on_selected_courses_keypress obj, k
        if k.keyval == Gdk::Keyval::GDK_Delete
          on_remove_course
        end
      end

      def on_load_settings_activate
        filter = Gtk::FileFilter.new
        filter.name = _("YAML files")
        filter.add_pattern "*.yml"
        filter.add_pattern "*.yaml"
        fs = Gtk::FileChooserDialog.new(_("Load Settings"),
                                        @glade["MainWindow"],
                                        Gtk::FileChooser::ACTION_OPEN,
                                        nil,
                                        [Gtk::Stock::CANCEL,
                                          Gtk::Dialog::RESPONSE_CANCEL],
                                          [Gtk::Stock::OPEN,
                                            Gtk::Dialog::RESPONSE_ACCEPT]
                                       )

        fs.add_filter filter
        if fs.run == Gtk::Dialog::RESPONSE_ACCEPT
          load_settings(fs.filename)
        end
        fs.destroy
      end

      def on_save_settings_activate
        filter = Gtk::FileFilter.new
        filter.name = _("YAML files")
        filter.add_pattern "*.yml"
        filter.add_pattern "*.yaml"
        fs = Gtk::FileChooserDialog.new(_("Save Settings"),
                                        @glade["MainWindow"],
                                        Gtk::FileChooser::ACTION_SAVE,
                                        nil,
                                        [Gtk::Stock::CANCEL,
                                          Gtk::Dialog::RESPONSE_CANCEL],
                                          [Gtk::Stock::SAVE,
                                            Gtk::Dialog::RESPONSE_ACCEPT]
                                       )
        fs.add_filter filter
        if fs.run == Gtk::Dialog::RESPONSE_ACCEPT
          if fs.filename =~ /\.ya?ml$/
            filename = fs.filename
          else
            filename = "#{fs.filename}.yml"
          end
          save_settings(filename)
        end
        fs.destroy
      end

      def on_next_activate
        if self.current_schedule
          self.current_schedule += 1
          on_change_current_schedule
        end
      end

      def on_previous_activate
        if self.current_schedule
          self.current_schedule -= 1
          on_change_current_schedule
        end
      end

      def on_jump_forward_activate
        if self.current_schedule
          self.current_schedule += 10
          on_change_current_schedule
        end
      end

      def on_jump_back_activate
        if self.current_schedule
          self.current_schedule -= 10
          on_change_current_schedule
        end
      end

      def initialize
        glade_file = GUI.find_data_file("ttime.glade")
        @glade = GladeXML.new(glade_file,nil,"ttime") do |handler|
          method(handler) 
        end

        @colliding_courses = false

        notebook = @glade["notebook"]

        @constraints = []
        @ratings = []

        # Touch the instance so nicknames get loaded
        @nicknames = Logic::Nicknames.instance

        init_schedule_view
        init_constraints
        init_ratings

        load_data
      end

      def selected_event_data_members
        Settings.instance[:shown_event_data] ||= DefaultEventDataMembers
      end

      def show_event_data_member symbol
        return if selected_event_data_members.include? symbol
        previously_selected = selected_event_data_members.dup
        selected_event_data_members.clear
        EventDataMembers.each do |orig_symbol, orig_name|
          if orig_symbol == symbol or previously_selected.include? orig_symbol
            selected_event_data_members << orig_symbol
          end
        end
      end

      def hide_event_data_member symbol
        selected_event_data_members.reject! { |s| s == symbol }
      end

      def on_quit_activate
        save_settings
        Gtk.main_quit
      end

      def on_about_activate
        @glade["AboutDialog"].version = TTime::Version
        @glade["AboutDialog"].run
      end

      def on_AboutDialog_response
        @glade["AboutDialog"].hide
      end

      def find_schedules
        if @selected_courses.empty?
          error_dialog(_('Please select some courses first.'))
          return
        end

        progress_dialog = ProgressDialog.new @glade["MainWindow"]

        Thread.new do
          @scheduler = Logic::Scheduler.new @selected_courses,
            @constraints,
            @ratings,
            &progress_dialog.get_status_proc(:pulsating => true,
                                             :show_cancel_button => true)

          progress_dialog.dispose

          if @scheduler.ok_schedules.empty?
            error_dialog _("Sorry, but no schedules are possible with the " \
                           "selected courses and constraints.")
          else
            set_num_schedules @scheduler.ok_schedules.size
            self.current_schedule = 0
            on_change_current_schedule
          end
        end
      end

      def on_add_course
        course = currently_addable_course(:expand => true)

        if course
          add_selected_course course

          on_available_course_selection
          on_selected_course_selection
        end
      end

      def drop_course course
        return false unless course

        unless are_you_sure_dialog(_("Are you sure you want to drop %s?") %
                                     course)
          return false
        end

        @selected_courses.delete course

        @list_selected_courses.each do |model, path, iter|
          if iter[2] == course
            model.remove iter
            break
          end
        end

        on_available_course_selection

        on_available_course_selection
        on_selected_course_selection
        update_exam_collisions

        return course
      end

      def on_remove_course
        iter = currently_removable_course_iter

        return iter ? drop_course(iter[2]) : false
      end

      def on_available_course_selection
        course = currently_addable_course

        @glade["btn_add_course"].sensitive = 
          course ? true : false

        set_course_info course
      end

      def on_selected_course_selection
        course_iter = currently_removable_course_iter
        @glade["btn_remove_course"].sensitive =
          course_iter ? true : false

        if course_iter
          set_course_info course_iter[2]
        else
          set_course_info nil
        end
      end

      def on_change_current_schedule
        self.current_schedule =
          @glade["spin_current_schedule"].adjustment.value - 1
        @glade["notebook"].page = 1
        draw_current_schedule
      end

      def current_schedule=(n)
        @current_schedule = n

        spinner = @glade["spin_current_schedule"]

        spinner.sensitive = true
        spinner.adjustment.lower = 1
        spinner.adjustment.value = n + 1
      end

      attr_reader :current_schedule

      def reject_events_from_calendar! &blk
        @calendar.reject_events!(&blk)
        @calendar.redraw
      end

      def text_for_event ev
        name = @nicknames.beautify[ev.group.name] || ev.group.name

        data_member_translation = {
          :course_name => name,
          :course_number => ev.course.number,
          :group_number => "קבוצה %d" % ev.group.number,
          :lecturer => ev.group.lecturer,
          :place => ev.place,
        }

        selected_event_data_members.map do |s|
          data_member_translation[s]
        end.reject { |s| s.nil? or s.empty? }.join("\n")
      end

      def add_event_to_calendar ev
        text = text_for_event(ev)
        day = ev.day
        hour = ev.start_frac
        length = ev.end_frac - ev.start_frac
        color = @selected_courses.index(ev.group.course)
        data = { :event => ev }
        type = ev.group.type

        @calendar.add_event(text, day, hour, length, color, data, type)
      end

      private

      def matches_search?(iter)
        text = @glade["search_box"].text
        ret=true

        if iter.has_child?
          child = iter.first_child
          return true if matches_search?(child)
          while child.next!
            return true if matches_search?(child)
          end
          return false
        end


        begin
          if text == ''
            ret=true
          elsif iter[1] == ''
            ret=true
          elsif text =~ /^[0-9]/ # Key is numeric
            ret = (iter[1] =~ /^#{text}/)
          elsif @nicknames.beautify[iter[0]] =~ /#{text}/
            ret=true
          else
            ret = (iter[0] =~ /#{text}/)
          end
        rescue
          ret = true
        end
        return ret
      end

      def save_settings(settings_file = nil)
        Settings.instance['selected_courses'] = \
          @selected_courses.collect { |course| course.number }
        Settings.instance.save(settings_file)
      end

      def load_settings(settings_file = nil)
        Settings.instance.load_settings(settings_file)

        @list_selected_courses.clear
        @selected_courses.clear

        Settings.instance.selected_courses.each do |course_num|
          begin
            add_selected_course @data.find_course_by_num(course_num)
          rescue NoSuchCourse
            error_dialog "There was a course with number \"#{course_num}\"" \
              " in your preferences, but it doesn't seem to exist now."
          end
        end
      end

      def add_selected_course(course)
        @selected_courses << course

        update_contraint_courses

        iter = @list_selected_courses.append
        iter[0] = course.name
        iter[1] = course.number
        iter[2] = course
        iter[3] = nil

        update_exam_collisions
      end

      # Look for exam collisions in selected courses and color them accordingly
      def update_exam_collisions
        @colliding_courses = false

        @list_selected_courses.each do |model, path, iter|
          course = iter[2]
          next if course.first_test_date.nil?
          other_courses = @selected_courses - [ course ]

          moed_a = course.first_test_date
          moed_b = course.second_test_date

          exam_dates_a = other_courses.collect { |c| c.first_test_date }
          exam_dates_b = other_courses.collect { |c| c.second_test_date }

          exam_dates = Set.new(exam_dates_a + exam_dates_b)

          if exam_dates.intersection([moed_a, moed_b]).empty?
            iter[0] = course.name
            iter[3] = nil
          else
            @colliding_courses = true
            iter[0] = "*%s*" % course.name
            iter[3] = "red"
          end
        end

        on_selected_course_selection
      end

      def set_num_schedules(n)
        @glade["spin_current_schedule"].adjustment.upper = n
        @glade["lbl_num_schedules"].text = sprintf(_(" of %d"), n)
      end

      def init_schedule_view
        notebook = @glade["notebook"]

        v = Gtk::VPaned.new
        s = Gtk::ScrolledWindow.new
        h = Gtk::HBox.new
        inner_vbox = Gtk::VBox.new

        s.shadow_type = Gtk::ShadowType::ETCHED_IN

        s.hscrollbar_policy = Gtk::PolicyType::NEVER
        s.vscrollbar_policy = Gtk::PolicyType::AUTOMATIC

        logo_file = GUI.find_data_file('ttime.svg')
        @calendar = TCal::Calendar.new({ :logo => logo_file })
        @calendar_info = Gtk::TextView.new
        @calendar_info.editable = false

        s.add @calendar_info

        h.pack_start s, true, true
        h.pack_start inner_vbox, false, false

        lbl = Gtk::Label.new
        lbl.markup = "<b>%s:</b>" % _("Show details")
        inner_vbox.pack_start lbl

        EventDataMembers.each do |symbol, text|
          # You'd think the _(text) is redundant, but for some reason it seems
          # to be required.
          check = Gtk::CheckButton.new(_(text))
          check.active = selected_event_data_members.include? symbol
          check.signal_connect('toggled') do
            if check.active?
              show_event_data_member symbol
            else
              hide_event_data_member symbol
            end
            @calendar.update_event_text do |data|
              text_for_event data[:event]
            end
            @calendar.redraw
          end
          inner_vbox.pack_start check
        end

        v.pack1 @calendar, true, false
        v.pack2 h, false, true

        notebook.append_page v, Gtk::Label.new(_("Schedule"))

        @calendar.add_click_handler do |params|
          event = params[:data] ? params[:data][:event] : nil
          schedule = @scheduler ?
            @scheduler.ok_schedules[@current_schedule] : nil
          set_calendar_info event, schedule
        end

        @calendar.add_rightclick_handler do |params|
          menu = Gtk::Menu.new
          menu.add_with_callback _("Show all alternatives") do
            for course in @selected_courses
              show_alternatives_for course
            end
          end
          unless params[:data].nil?
            menu.add_with_callback _("Show alternatives to this event") do
              course = params[:data][:event].course
              group = params[:data][:event].group
              show_alternatives_for course, group.type
            end
            menu.add_with_callback _("Cancel this course") do
              course = params[:data][:event].course
              if drop_course(course)
                reject_events_from_calendar! do |data|
                  data[:event].course == course
                end
              end
            end
          end

          @constraints.select do |constraint|
            if constraint.enabled? and constraint.class.menu_items
              constraint.class.menu_items.each do |item|
                unless item.event_required? and params[:data].nil?
                  menu.add_with_callback item.caption do |*e|
                    constraint.send item.method_name, params
                  end
                end
              end
            end
          end

          menu.show_all
          menu.popup(nil,nil,3,params[:gdk_event].time)
        end

        notebook.show_all
      end

      def show_alternatives_for course, group_type = nil
        @calendar.reject_events! do |data|
          ev = data[:event]
          ev.group.course.number == course.number and \
            (group_type == nil or ev.group.type == group_type)
        end
        course.groups.select do |g|
          group_type == nil or g.type == group_type
        end.each do |g|
          g.events.each do |ev|
            add_event_to_calendar ev
          end
        end
        @calendar.redraw
      end

      # Update @calendar_info to display info about the given event
      def set_calendar_info(event = nil, schedule = nil)
        buffer = @calendar_info.buffer

        buffer.text = ''
        iter = buffer.get_iter_at_offset(0)

        tag = buffer.create_tag(nil, { :font => 'Sans Bold 14' })

        if event
          buffer.insert(iter, "#{event.group.name}\n", tag)

          add_detail_to_buffer(buffer, iter, "קבוצה", event.group.number)
          add_detail_to_buffer(buffer, iter, "מקום", event.place)
          add_detail_to_buffer(buffer, iter, "מרצה", event.group.lecturer)

          buffer.insert(iter, "\n")
        end

        if schedule
          buffer.insert(iter, _("Rating details for this schedule:"), tag)
          buffer.insert(iter, "\n")

          schedule.ratings.each do |rater, score|
            add_detail_to_buffer buffer, iter, _("\"%s\" rating") % rater, \
              "%.2f" % score
          end
          add_detail_to_buffer buffer, iter, _("Overall score"), \
            "%.2f" % schedule.score

          buffer.insert(iter, "\n")
        end
      end

      def add_detail_to_buffer(buffer, iter, title, detail)
	    return if detail.nil? or detail == ""
        tag = buffer.create_tag(nil, {
          :weight => Pango::FontDescription::WEIGHT_BOLD
        })

        buffer.insert(iter, "#{title}: ", tag)
        buffer.insert(iter, "#{detail}\n")
      end

      def scheduler_ready?
        return false unless @scheduler.is_a? TTime::Logic::Scheduler
        return false unless @scheduler.ok_schedules.size > @current_schedule
        true
      end

      def draw_current_schedule
        #test
        return unless scheduler_ready?

        #get current schedual to draw
        schedule = @scheduler.ok_schedules[@current_schedule]

        log.info { "Score for current schedule: %p (%p)" % \
          [ schedule.score, schedule.ratings ] }

        #clear the calendar
        @calendar.clear_events

        schedule.events.each do |ev|
          add_event_to_calendar ev
        end

        @calendar.redraw

        set_calendar_info nil, schedule
      end

      def set_course_info(course)
        buf = @glade["text_course_info"].buffer
        buf.text = ""
        iter = buf.get_iter_at_offset(0)

        if @colliding_courses
          tag = buf.create_tag(nil, {
            :font => "Sans Bold 14",
            :foreground => "red"
          })
          buf.insert iter,
            _("WARNING: The courses marked with * have colliding test dates!"),
            tag
          buf.insert iter, "\n"
        end

        if course
          h1 = buf.create_tag(nil, { :font => "Sans Bold 12" })
          h2 = buf.create_tag(nil, { :font => "Sans Bold" })
          buf.insert iter, "[#{course.number}] #{course.name}\n", h1

          [
            [ course.lecturer_in_charge, _("Lecturer in charge") ],
            [ course.academic_points, _("Academic points") ],
            [ course.first_test_date, _("Moed A") ],
            [ course.second_test_date, _("Moed B") ],
          ].each do |param, title|
            if param
              buf.insert iter, "#{title}: ", h2
              buf.insert iter, "#{param}\n"
            end
          end

          course.groups.each do |grp|
            buf.insert iter, "\n"
            buf.insert iter, _("Group %d\n") % grp.number, h2
            got_any_data = false
            if grp.lecturer
              got_any_data = true
              buf.insert iter, _("Lecturer: "), h2
              buf.insert iter, grp.lecturer
              buf.insert iter, "\n"
            end

            grp.events.each do |ev|
              got_any_data = true
              human_day = TTime::Logic::Day::numeric_to_human(ev.day)
              human_start = TTime::Logic::Hour::military_to_human(ev.start)
              human_end = TTime::Logic::Hour::military_to_human(ev.end)
              buf.insert iter, "#{human_day}, #{human_start}-#{human_end}\n"
            end

            unless got_any_data
              buf.insert iter, _("* No data for this group *\n")
            end
          end
        end
      end

      def currently_addable_course(params = {})
        available_courses_view = @glade["treeview_available_courses"]

        selected_iter = available_courses_view.selection.selected

        return false unless selected_iter

        return false if @selected_courses.include? selected_iter[2]

        if params[:expand] and (not selected_iter[2])
          available_courses_view.expand_row(selected_iter.path, false)
        end

        selected_iter[2]
      end

      def currently_removable_course_iter
        selected_courses_view = @glade["treeview_selected_courses"]

        selected_iter = selected_courses_view.selection.selected

        return false unless selected_iter

        selected_iter
      end

      def load_data(force = false)
        @selected_courses = []

        @tree_available_courses = Gtk::TreeStore.new String, String,
          Logic::Course
        @tree_available_search = Gtk::TreeModelFilter.new @tree_available_courses
        @list_selected_courses = Gtk::ListStore.new String, String,
          Logic::Course, String


        init_course_tree_views

        progress_dialog = ProgressDialog.new @glade["MainWindow"]

        Thread.new do
          @data = TTime::Data.new(force, &progress_dialog.get_status_proc)

          progress_dialog.dispose

          update_available_courses_tree

          load_settings
        end
      end

      def update_available_courses_tree
        @tree_available_courses.clear

        progress_dialog = ProgressDialog.new @glade["MainWindow"]
        progress_dialog.text = _('Populating available courses')

        Thread.new do
          @data.each_with_index do |faculty,i|
            progress_dialog.fraction = i.to_f / @data.size.to_f

            iter = @tree_available_courses.append(nil)
            iter[0] = faculty.name

            faculty.courses.each do |course|
              child = @tree_available_courses.append(iter)
              child[0] = course.name
              child[1] = course.number
              child[2] = course
            end
          end

          progress_dialog.dispose
        end
      end

      def init_course_tree_views
        available_courses_view = @glade["treeview_available_courses"]
        available_courses_view.model = @tree_available_search

        available_courses_view.set_search_equal_func do |m,c,key,iter|
          begin
            if key =~ /^[0-9]/
              not (iter[1] =~ /^#{key}/)
            else
              not (iter[0] =~ /#{key}/)
            end
          rescue
            true
          end
        end



        @tree_available_search.set_visible_func do |model, iter|
          matches_search? iter
        end

        @glade["search_box"].signal_connect("activate") do |widget|
          @glade["treeview_available_courses"].expand_all
          @tree_available_search.refilter
          @glade["treeview_available_courses"].expand_all
        end


        selected_courses_view = @glade["treeview_selected_courses"]
        selected_courses_view.model = @list_selected_courses

        [ _("Course Name"), _("Course Number") ].each_with_index do |label, i|
          col = Gtk::TreeViewColumn.new label, Gtk::CellRendererText.new,
            :text => i
          col.resizable = true

          available_courses_view.append_column col

          # We actually have to create an entirely new column again, because a
          # TreeViewColumn object can't be shared between two treeviews.

          col = Gtk::TreeViewColumn.new label, Gtk::CellRendererText.new,
            :text => i, :foreground => 3
          col.resizable = true

          selected_courses_view.append_column col
        end
      end

      def update_contraint_courses
        @constraints.each do |c|
          c.update_courses(@selected_courses)
        end
      end

      def init_constraints
        Constraints.initialize
        @constraints = Constraints.get_constraints

        constraints_notebook = Gtk::Notebook.new

        @constraints.each do |c|
          constraints_notebook.append_page c.preferences_panel,
            Gtk::Label.new(c.name)
        end

        constraints_notebook.tab_pos = 0
        constraints_notebook.border_width = 5

        notebook = @glade["notebook"]
        notebook.append_page constraints_notebook, 
          Gtk::Label.new(_("Constraints"))
        notebook.show_all
      end

      def init_ratings
        Ratings.initialize
        @ratings = Ratings.get_ratings

        ratings_notebook = Gtk::Notebook.new

        @ratings.each do |c|
          ratings_notebook.append_page c.preferences_panel,
            Gtk::Label.new(c.name)
        end

        priorities = Gtk::Table.new @ratings.length, 2
        ratings_notebook.append_page priorities, Gtk::Label.new(_("Priorities"))

        @ratings.each_with_index do |c,i|
          scale = Gtk::HScale.new 1, 10, 1
          scale.adjustment.value = c.weight
          lbl = Gtk::Label.new(c.name)
          lbl.justify = Gtk::JUSTIFY_LEFT
          priorities.attach lbl, 0, 1, i, i+1, Gtk::FILL, Gtk::FILL, 5, 5
          priorities.attach scale, 1, 2, i, i+1, \
            Gtk::EXPAND | Gtk::FILL, Gtk::FILL, 5, 5
        end

        ratings_notebook.tab_pos = 0
        ratings_notebook.border_width = 5

        notebook = @glade["notebook"]
        notebook.append_page ratings_notebook, 
          Gtk::Label.new(_("Schedule ratings"))
        notebook.show_all
      end

      def error_dialog(msg)
        dialog = Gtk::MessageDialog.new @glade["MainWindow"],
          Gtk::Dialog::MODAL | Gtk::Dialog::DESTROY_WITH_PARENT,
          Gtk::MessageDialog::ERROR, Gtk::MessageDialog::BUTTONS_OK, msg
        dialog.show
        dialog.signal_connect('response') { dialog.destroy }
      end

      def are_you_sure_dialog(msg)
        dialog = Gtk::MessageDialog.new @glade["MainWindow"],
          Gtk::Dialog::MODAL | Gtk::Dialog::DESTROY_WITH_PARENT,
          Gtk::MessageDialog::QUESTION, Gtk::MessageDialog::BUTTONS_YES_NO, msg
        response = (dialog.run == Gtk::Dialog::RESPONSE_YES)
        dialog.destroy
        return response
      end

      def on_ExamSchedule_clicked
        begin
          exam_schedule = ExamSchedule.new(@selected_courses, @glade["MainWindow"])
          exam_schedule.run
          exam_schedule.destroy
        rescue ExamSchedule::NoTests
          error_dialog _("No courses with tests are selected.")
        end
      end
    end
  end
end
