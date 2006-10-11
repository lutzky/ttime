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
        available_courses_view = @glade["treeview_available_courses"]

        selected_iter = available_courses_view.selection.selected

        return unless selected_iter && selected_iter[2]

        course = selected_iter[2]

        unless @selected_courses.include? course
          @selected_courses << course

          p @selected_courses

          iter = @list_selected_courses.append
          iter[0] = course.number
          iter[1] = course.name
          iter[2] = course
        end
      end

      private
      def load_data
        @data = TTime::Data.load

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

        p @data.size
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
