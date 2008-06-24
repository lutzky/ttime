require 'libglade2'
require 'data'
#require 'gtkmozembed'
require 'tempfile'
require 'gettext'
require 'singleton'

require 'constraints'
require 'settings'
require 'logic/course'
require 'logic/scheduler'
require 'gui/progress_dialog'
require 'tcal/tcal'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

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
    class MainWindow
      include Singleton
      include GetText

      def on_auto_update
        load_data(true)
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

      def show_preferences
      end

      GLADE_FILE = "gui/ttime.glade"
      def initialize
        @glade = GladeXML.new(GLADE_FILE,nil,"ttime","locale") do |handler|
          method(handler) 
        end

        notebook = @glade["notebook"]

        @constraints = []

        init_schedule_view
        init_constraints

        # Quick hack around a bug - it seems that MozEmbed gets a little
        # shy when in a notebook, and only displays on the second time
        # we view it.
        notebook.page = 1
        notebook.page = 0

        load_data
      end

      def on_quit_activate
        save_settings
        Gtk.main_quit
      end

      def on_about_activate
        @glade["AboutDialog"].run
      end

      def find_schedules
        if @selected_courses.empty?
          error_dialog(_('Please select some courses first.'))
          return
        end

        progress_dialog = ProgressDialog.new

        Thread.new do
          @scheduler = Logic::Scheduler.new @selected_courses,
            @constraints,
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

      def on_remove_course
        iter = currently_removable_course_iter

        if iter
          @selected_courses.delete iter[2]
          @list_selected_courses.remove iter
          update_contraint_courses

          on_available_course_selection
          on_selected_course_selection
        end
      end

      def on_available_course_selection
        course = currently_addable_course

        @glade["btn_add_course"].sensitive = 
          course ? true : false

        if course
          set_course_info course.text
        end
      end

      def on_selected_course_selection
        course_iter = currently_removable_course_iter
        @glade["btn_remove_course"].sensitive =
          course_iter ? true : false

        if course_iter
          set_course_info course_iter[2].text
        end
      end

      def on_change_current_schedule
        self.current_schedule =
          @glade["spin_current_schedule"].adjustment.value - 1
        draw_current_schedule
        @glade["notebook"].page = 1
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
        @calendar.reject_events! &blk
        @calendar.redraw
      end

      def add_event_to_calendar ev
          @calendar.add_event(ev.desc,ev.day,ev.start_frac,ev.end_frac-ev.start_frac,ev.group_id,{ :event => ev },ev.group.type)
      end

      private

      def matches_search?(iter)
        text = @glade["search_box"].text
        #puts text
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
            #puts "|#{iter[1]}|"
            ret = (iter[1] =~ /^#{text}/)
          else
            ret = (iter[0] =~ /#{text}/)
          end
        rescue
          ret = true
        end
        return ret
      end

      def save_settings
        Settings.instance['selected_courses'] = \
          @selected_courses.collect { |course| course.number }
        Settings.instance.save
      end

      def load_settings
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
      end

      def set_num_schedules(n)
        @glade["spin_current_schedule"].adjustment.upper = n
        @glade["lbl_num_schedules"].text = sprintf(_(" of %d"), n)
      end

      def init_schedule_view
        notebook = @glade["notebook"]

        v = Gtk::VPaned.new
        s = Gtk::ScrolledWindow.new

        s.shadow_type = Gtk::ShadowType::ETCHED_IN

        s.hscrollbar_policy = Gtk::PolicyType::NEVER
        s.vscrollbar_policy = Gtk::PolicyType::AUTOMATIC

        @calendar = TCal::Calendar.new({:logo => 'gui/ttime.svg' })
        @calendar_info = Gtk::TextView.new
        @calendar_info.editable = false

        s.add @calendar_info

        v.pack1 @calendar, true, false
        v.pack2 s, false, true

        notebook.append_page v, Gtk::Label.new(_("Schedule"))

        @calendar.add_click_handler do |params|
          if params[:data]
            set_calendar_info params[:data][:event]
          end
        end

        @calendar.add_rightclick_handler do |params|
          menu = Gtk::Menu.new
          menu.add_with_callback _("Show all alternatives") do |*e|
            for course in @selected_courses
              show_alternatives_for course
            end
          end
          unless params[:data].nil?
            menu.add_with_callback _("Show alternatives to this event") do |*e|
              course = params[:data][:event].course
              group = params[:data][:event].group
              show_alternatives_for course, group.type
            end
          end
          @constraints.select { |c| c.enabled? }.each do |constraint|
            constraint.menu_items.each do |item|
              unless item.event_required? and params[:data].nil?
                menu.add_with_callback item.caption do |*e|
                  item.block.call params
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
      def set_calendar_info(event)
        buffer = @calendar_info.buffer

        buffer.text = ''
        iter = buffer.get_iter_at_offset(0)

        tag = buffer.create_tag(nil, { :font => 'Sans Bold 14' })

        buffer.insert(iter, "#{event.group.name}\n", tag)

        add_detail_to_buffer(buffer, iter, "קבוצה", event.group.number)
        add_detail_to_buffer(buffer, iter, "מקום", event.place)
        add_detail_to_buffer(buffer, iter, "מרצה", event.group.lecturer)
      end

      def add_detail_to_buffer(buffer, iter, title, detail)
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

        #clear the calendar
        @calendar.clear_events

        schedule.events.each do |ev|
          add_event_to_calendar ev
        end

        @calendar.redraw

      end

      def set_course_info(info)
        @glade["text_course_info"].buffer.text = info
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
          Logic::Course


        init_course_tree_views

        progress_dialog = ProgressDialog.new

        Thread.new do
          @data = TTime::Data.new(force, &progress_dialog.get_status_proc)

          progress_dialog.dispose

          update_available_courses_tree

          load_settings
        end
      end

      def update_available_courses_tree
        @tree_available_courses.clear

        progress_dialog = ProgressDialog.new
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

#          @glade["treeview_available_courses"].expand_all
        end
      end




      def init_course_tree_views
        available_courses_view = @glade["treeview_available_courses"]
        available_courses_view.model = @tree_available_search

        available_courses_view.set_search_equal_func do |m,c,key,iter|
          begin
            if ('0'..'9').include? key[0..0] # Key is numeric
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
            :text => i
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

      def error_dialog(msg)
        dialog = Gtk::MessageDialog.new nil,
          Gtk::Dialog::MODAL | Gtk::Dialog::DESTROY_WITH_PARENT,
          Gtk::MessageDialog::ERROR, Gtk::MessageDialog::BUTTONS_OK, msg
        dialog.show
        dialog.signal_connect('response') { dialog.destroy }
      end
    end
  end
end
