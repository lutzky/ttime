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
require 'ttime/gui/gtk_queue'
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

    AvailableCoursesColumns = [ 
      [:name, String],
      [:number, String],
      [:course, Logic::Course],
      [:visible, TrueClass],
    ]

    AvailableCoursesColTypes = AvailableCoursesColumns.map { |x| x[1] }

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
        save_settings
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

      def on_full_week_toggled menu_item
        Settings.instance[:show_full_week] = menu_item.active?
        draw_current_schedule
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
        init_info

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
        @glade["AboutDialog"].name = _("TTime")
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

          Gtk.queue do
            progress_dialog.dispose
          end

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

      def on_clear_courses
        return false if @selected_courses.empty?

        unless are_you_sure_dialog(
          _("Are you sure you want to clear your selected courses?"))
          return false
        end

        @selected_courses.clear
        @list_selected_courses.clear

        on_available_course_selection

        update_exam_collisions

        return true
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
        Gtk.queue do
          self.current_schedule =
            @glade["spin_current_schedule"].adjustment.value - 1
          @glade["notebook"].page = 1
          draw_current_schedule
        end
      end

      def current_schedule=(n)
        @current_schedule = n

        Gtk.queue do
          spinner = @glade["spin_current_schedule"]

          spinner.sensitive = true
          spinner.adjustment.lower = 1
          spinner.adjustment.value = @current_schedule + 1
        end
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

      def update_search_matches
        text = @glade["search_box"].text
        log.debug { "Starting search for \"%s\"" % text }

        @tree_available_courses.each do |model, path, iter|
          iter[3] = false
          matches = false

          begin
          if text == ''
            matches = true
          elsif iter[1] == ''
            matches = true
          elsif text =~ /^[0-9]/ # Key is numeric
            matches = (iter[1] =~ /^#{text}/)
          elsif @nicknames.beautify[iter[0]] =~ /#{text}/
            matches = true
          else
            matches = (iter[0] =~ /#{text}/)
          end
          rescue
            matches = true
          end

          if matches
            iter[3] = true
            iter.parent[3] = true unless iter.parent.nil?
          end
        end
        log.debug "Search complete"
      end

      def save_settings(settings_file = nil)
        Settings.instance['selected_courses'] = \
          @selected_courses.collect { |course| course.number }
        Settings.instance[:semester_start_date] = @semester_start_entry.text
        Settings.instance[:semester_end_date] = @semester_end_entry.text
        Settings.instance.save(settings_file)
      end

      def load_settings(settings_file = nil)
        Settings.instance.load_settings(settings_file)

        if Settings.instance[:show_full_week].nil?
          Settings.instance[:show_full_week] = true
        end
        Gtk.queue do
          @glade["full_week"].active = Settings.instance[:show_full_week]

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
        if not Settings.instance[:semester_start_date].nil?
            @semester_start_entry.text = Settings.instance[:semester_start_date]
        end
        if not Settings.instance[:semester_end_date].nil?
            @semester_end_entry.text = Settings.instance[:semester_end_date]
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

          my_test_dates = [course.first_test_date, course.second_test_date]

          my_test_dates.reject! { |x| x.nil? }

          exam_dates_a = other_courses.collect { |c| c.first_test_date }
          exam_dates_b = other_courses.collect { |c| c.second_test_date }

          exam_dates_a.reject! { |x| x.nil? }
          exam_dates_b.reject! { |x| x.nil? }

          exam_dates = Set.new(exam_dates_a + exam_dates_b)

          if exam_dates.intersection(my_test_dates).empty?
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
        Gtk.queue do
          @glade["spin_current_schedule"].adjustment.upper = n
          @glade["lbl_num_schedules"].text = sprintf(_(" of %d"), n)
        end
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

        Gtk.queue do
          @calendar.redraw
	  set_calendar_info nil, schedule
        end
      end

      def export_ical
        require 'rubygems'
        require 'ri_cal'
        require 'tzinfo'
        unless scheduler_ready?
          error_dialog(_("Please run \"Find Schedules\" first"))
          return false
        end

        filter = Gtk::FileFilter.new
        filter.name = _("iCal files")
        filter.add_pattern "*.ics"
        fs = Gtk::FileChooserDialog.new(_("Export iCal"),
                                        @glade["MainWindow"],
                                        Gtk::FileChooser::ACTION_SAVE,
                                        nil,
                                        [Gtk::Stock::CANCEL,
                                          Gtk::Dialog::RESPONSE_CANCEL],
                                          [Gtk::Stock::SAVE,
                                            Gtk::Dialog::RESPONSE_ACCEPT]
                                       )
        fs.add_filter filter
        fs.do_overwrite_confirmation = true

        if fs.run == Gtk::Dialog::RESPONSE_ACCEPT
          if fs.filename =~ /\.ics$/
            filename = fs.filename
          else
            filename = "#{fs.filename}.ics"
          end

          # TODO find a way to get the current dates automatically
          semester_dstart = DateTime.parse("10/14/2009")
          semester_dend   = DateTime.parse("1/21/2010")

          semester_start_weekday = semester_dstart.wday
          schedule = @scheduler.ok_schedules[@current_schedule]
          ical = RiCal.Calendar do |ical|
            ical.default_tzid = "Asia/Jerusalem"
            schedule.events.each do |ev|
              ical.event do |ical_ev|
                ical_ev.summary     = @nicknames.beautify[ev.group.name] || ev.group.name
                ical_ev.description = text_for_event(ev)
                start_time = DateTime.parse(Logic::Hour::military_to_human(ev.start))
                end_time   = DateTime.parse(Logic::Hour::military_to_human(ev.end))
                event_weekday = ev.day - 1
                start_date = semester_dstart + ((event_weekday - semester_start_weekday) % 7)
                ical_ev.dtstart = start_date + start_time.day_fraction
                ical_ev.dtend   = start_date + end_time  .day_fraction
                ical_ev.location = ev.place
                ical_ev.rrule = {
                  :freq => "WEEKLY",
                  :interval => 1,
                  :wkst => Logic::Day::numeric_to_ical(ev.day),
                  :until => semester_dend
                }
              end
            end
            @selected_courses.each do |course|
              if not course.first_test_date.nil?
                create_exam_event(ical, course, course.first_test_date,  true)
              end
              if not course.second_test_date.nil?
                create_exam_event(ical, course, course.second_test_date, false)
              end
            end
          end
          file = File.new(filename,"w")
          print ical.export_to file
          file.close
        end
        fs.destroy
      end

      def create_exam_event(ical, course, exam_date, is_first_exam)
        ical.event do |ev|
            name = @nicknames.beautify[course.name] || course.name
            ev.summary = name + " - " + (is_first_exam ? _("Moed A") : _("Moed B"))
            ev.dtstart = ev.dtend = exam_date
        end
      end

      def export_pdf
        unless scheduler_ready?
          error_dialog(_("Please run \"Find Schedules\" first"))
          return false
        end

        filter = Gtk::FileFilter.new
        filter.name = _("PDF files")
        filter.add_pattern "*.pdf"
        fs = Gtk::FileChooserDialog.new(_("Export PDF"),
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
          if fs.filename =~ /\.pdf$/
            filename = fs.filename
          else
            filename = "#{fs.filename}.pdf"
          end

          schedule = @scheduler.ok_schedules[@current_schedule]
          @calendar.clear_events
          schedule.events.each do |ev|
            add_event_to_calendar ev
          end

          @calendar.output_pdf(filename)
        end
        fs.destroy
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

        @tree_available_courses = Gtk::TreeStore.new *AvailableCoursesColTypes

        @tree_available_search = Gtk::TreeModelFilter.new @tree_available_courses
        @list_selected_courses = Gtk::ListStore.new String, String,
          Logic::Course, String


        init_course_tree_views

        progress_dialog = ProgressDialog.new @glade["MainWindow"]

        Thread.new do
          @data = TTime::Data.new(force, &progress_dialog.get_status_proc)

          Gtk.queue do
            progress_dialog.dispose
            update_available_courses_tree
            load_settings
          end
        end
      end

      def update_available_courses_tree
        Gtk.queue do
          @tree_available_courses.clear

          progress_dialog = ProgressDialog.new @glade["MainWindow"]
          progress_dialog.text = _('Populating available courses')

          @data.each_with_index do |faculty,i|
            progress_dialog.fraction = i.to_f / @data.size.to_f

            iter = @tree_available_courses.append(nil)
            iter[0] = faculty.name
            iter[3] = true

            faculty.courses.each do |course|
              child = @tree_available_courses.append(iter)
              child[0] = course.name
              child[1] = course.number
              child[2] = course
              child[3] = true
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

        @tree_available_search.set_visible_column 3

        @glade["search_box"].signal_connect("activate") do |widget|
          log.debug "Updating search matches"
          update_search_matches
          log.debug "Done searching"
          unless @glade["search_box"].text.empty?
            log.debug "Expanding available courses treeview"
            @glade["treeview_available_courses"].expand_all
          end
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
          scale.adjustment.signal_connect("value-changed") do |adj|
            c.weight = adj.value
          end
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

      def init_info
        info_tab = Gtk::Table.new(2, 2)

        lbl = Gtk::Label.new(_("Semester start:"))
        lbl.justify = Gtk::JUSTIFY_LEFT
        info_tab.attach lbl, 0, 1, 0, 1, Gtk::FILL, Gtk::FILL, 5, 5
        lbl = Gtk::Label.new(_("Semester end:"))
        lbl.justify = Gtk::JUSTIFY_LEFT
        info_tab.attach lbl, 0, 1, 1, 2, Gtk::FILL, Gtk::FILL, 5, 5

        @semester_start_entry = Gtk::Entry.new
        info_tab.attach @semester_start_entry, 1, 2, 0, 1, Gtk::FILL, Gtk::FILL, 5, 5
        @semester_end_entry = Gtk::Entry.new
        info_tab.attach @semester_end_entry, 1, 2, 1, 2, Gtk::FILL, Gtk::FILL, 5, 5

        notebook = @glade["notebook"]
        notebook.append_page info_tab,
          Gtk::Label.new(_("Semester Information"))
        notebook.show_all
      end

      def error_dialog(msg)
        Gtk.queue do
          dialog = Gtk::MessageDialog.new @glade["MainWindow"],
            Gtk::Dialog::MODAL | Gtk::Dialog::DESTROY_WITH_PARENT,
            Gtk::MessageDialog::ERROR, Gtk::MessageDialog::BUTTONS_OK, msg
          dialog.show
          dialog.signal_connect('response') { dialog.destroy }
        end
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
