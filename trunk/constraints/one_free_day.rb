require 'constraints'

module TTime
  module Constraints
    class OneFreeDay < AbstractConstraint
      def initialize
        super

        @enabled = false
      end

      def evaluate_schedule
        return true unless @enabled

        day_grid = []
        event_list.each do |ev|
          day_grid[ev.day] = true
        end

        1.upto(5) do |i|
          return true unless day_grid[i]
        end
        false
      end

      def name
        'One free day'
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        btn_enabled = Gtk::CheckButton.new('Enable')
        btn_enabled.active = @enabled

        btn_enabled.signal_connect('toggled') do
          @enabled = btn_enabled.active?
        end

        vbox.pack_start btn_enabled, false, false
        vbox.pack_start Gtk::Label.new(
        <<-EOF
Written mostly as proof of a concept - UDonkey's documentation says they
can only check this condition after a schedule is built, to which I say "WTF?"
        EOF
        )

        vbox
      end
    end
  end
end
