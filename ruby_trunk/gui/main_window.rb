require 'libglade2'
require 'data'

module TTime
  module GUI
    class MainWindow
      GLADE_FILE = "gui/ttime.glade"
      def initialize
        @glade = GladeXML.new(GLADE_FILE) { |handler| method(handler) }

        @data = {}

        @main_window = @glade["MainWindow"]

        load_data
      end

      def load_data
        @data = TTime::Data.load

        y @data
      end

      def on_quit_activate
        Gtk.main_quit
      end

      def on_about_activate
        @glade["AboutDialog"].run
      end
    end
  end
end
