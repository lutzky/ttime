require 'gtk2'

module TTime
  module GUI
    class ProgressDialog < Gtk::Dialog
      def initialize
        super

        self.title = "Progress"

        @progressbar = Gtk::ProgressBar.new
        @label = Gtk::Label.new ''

        vbox.pack_start @label
        vbox.pack_end @progressbar
        show_all
      end

      def get_status_proc
        Proc.new do |text,fraction|
          self.text = text
          self.fraction = fraction
        end
      end

      def fraction
        @progressbar.fraction
      end

      def fraction= fraction
        @progressbar.fraction = fraction
      end

      def text
        @label.text
      end

      def text= text
        @label.text = text
      end

      def dispose
        hide
        destroy
      end
    end
  end
end
