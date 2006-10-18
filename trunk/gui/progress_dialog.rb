require 'gtk2'

module TTime
  module GUI
    class ProgressDialog < Gtk::Dialog
      def initialize
        super

        self.title = "Progress"

        @progressbar = Gtk::ProgressBar.new
        @label = Gtk::Label.new ''

        self.modal = true

        signal_connect('response') do
          @canceled = true
        end

        vbox.pack_start @label
        vbox.pack_end @progressbar

        show_all
      end

      def get_status_proc(args = {})
        if args[:show_cancel_button]
          btn_cancel = Gtk::Button.new(Gtk::Stock::STOP)
          btn_cancel.signal_connect('clicked') { @canceled = true }
          action_area.pack_end btn_cancel, false, false
          show_all
        end

        if args[:pulsating]
          Proc.new do |text|
            throw :cancel if @canceled
            self.text = text
            self.pulse
          end
        else
          Proc.new do |text,fraction|
            throw :cancel if @canceled
            self.text = text
            self.fraction = fraction
          end
        end
      end

      def fraction
        @progressbar.fraction
      end

      def pulse
        @progressbar.pulse
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
        begin
          hide
        rescue
        end
        begin
          destroy
        rescue
        end
      end
    end
  end
end
