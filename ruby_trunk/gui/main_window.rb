require 'logic/course'
require 'libglade2'
require 'data'

module TTime
  module GUI
    class MainWindow
      GLADE_FILE = "gui/ttime.glade"
      def initialize
        @glade = GladeXML.new(GLADE_FILE) { |handler| method(handler) }

        @selected_courses = []

        @tree_available_courses = Gtk::TreeStore.new String, String,
          Logic::Course
        @list_selected_courses = Gtk::ListStore.new String, String,
          Logic::Course

        init_course_tree_views

        @glade["statusbar"].push(@glade["statusbar"].get_context_id('status'),'Hi there. Another thread is loading the REPY file. This would be more elegant with a modal progress bar.')
        Thread.new do
          load_data
          @glade["statusbar"].pop(@glade["statusbar"].get_context_id('status'))
        end
      end

      def on_quit_activate
        Gtk.main_quit
      end

      def on_about_activate
        @glade["AboutDialog"].run
      end

      def on_add_course
        course = currently_addable_course

        if course
          @selected_courses << course

          iter = @list_selected_courses.append
          iter[0] = course.number
          iter[1] = course.name
          iter[2] = course

          on_available_course_selection
          on_selected_course_selection
        end
      end

      def on_remove_course
        iter = currently_removable_course_iter

        if iter
          @selected_courses.delete iter[2]
          @list_selected_courses.remove iter

          on_available_course_selection
          on_selected_course_selection
        end
      end

      def on_available_course_selection
        @glade["btn_add_course"].sensitive = 
          currently_addable_course ? true : false
      end

      def on_selected_course_selection
        @glade["btn_remove_course"].sensitive =
          currently_removable_course_iter ? true : false
      end

      private
      def currently_addable_course
        available_courses_view = @glade["treeview_available_courses"]

        selected_iter = available_courses_view.selection.selected

        return false unless selected_iter

        return false if @selected_courses.include? selected_iter[2]

        selected_iter[2]
      end

      def currently_removable_course_iter
        selected_courses_view = @glade["treeview_selected_courses"]

        selected_iter = selected_courses_view.selection.selected

        return false unless selected_iter

        selected_iter
      end

      def load_data
        @data = TTime::Data.load do |s,a,b|
          puts "#{s}, #{a} out of #{b}"
        end

        @data.each do |faculty|
          iter = @tree_available_courses.append(nil)
          iter[0] = faculty.name

          faculty.courses.each do |course|
            child = @tree_available_courses.append(iter)
            child[0] = course.number
            child[1] = course.name
            child[2] = course
          end
        end
      end

      def init_course_tree_views
        available_courses_view = @glade["treeview_available_courses"]
        available_courses_view.model = @tree_available_courses

        selected_courses_view = @glade["treeview_selected_courses"]
        selected_courses_view.model = @list_selected_courses
        
        columns = []

        [ "Course No.", "Course Name" ].each_with_index do |label, i|
          columns[i] = Gtk::TreeViewColumn.new label, Gtk::CellRendererText.new,
            :text => i
        end

        columns.each do |c|
          available_courses_view.append_column c
        end

        # This actually has to be done twice, because we need different
        # copies of the columns for each of the views

        [ "Course No.", "Course Name" ].each_with_index do |label, i|
          columns[i] = Gtk::TreeViewColumn.new label, Gtk::CellRendererText.new,
            :text => i
        end

        columns.each do |c|
          selected_courses_view.append_column c
        end
      end
    end
  end
end
