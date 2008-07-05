require 'ttime/constraints'
require 'ttime/logic/times'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class NoClashes < AbstractConstraint
      def initialize
        super

        @enabled = true
      end

      def evaluate_schedule
        return true unless @enabled

        # Note: Do not use Array.new(8,[]), that makes only one [] copy
        # and 8 pointers to it
        event_grid = Array.new(8) { [] }
        earliest_start = 100000000
        latest_finish = 0
        event_list.each do |ev|
          start_time = TTime::Logic::Hour::military_to_grid ev.start
          end_time = TTime::Logic::Hour::military_to_grid ev.end

          earliest_start = start_time if start_time < earliest_start
          latest_finish = end_time if end_time > latest_finish

          # Ending at xx:30 means ending at xx:20, last box isn't taken
          start_time.upto(end_time - 1) do |i|
            return false if event_grid[ev.day][i]
            event_grid[ev.day][i] = true
          end
        end
        true
      end

      def name
        _('No clashes')
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        btn_enabled = Gtk::CheckButton.new(_('Enable'))
        btn_enabled.active = @enabled

        btn_enabled.signal_connect('toggled') do
          @enabled = btn_enabled.active?
        end

        vbox.pack_start btn_enabled, false, false
        vbox.pack_start Gtk::Label.new(
        _(<<-EOF
WARNING! Disabling the 'no clashes' constraint will make scheduling take
MUCH longer! You probably want to leave this on. (Coming soon: allow
clashes only for specific courses)
        EOF
        ))

        vbox
      end

      def print_event_grid(event_grid)
        puts '-start------------'
        earliest_start.upto(latest_finish) do |h|
          print h
          print ": "
          0.upto(7) do |d|
            if event_grid[d][h]
              print '*'
            else
              print ' '
            end
          end
          print "\n"
        end
        puts '--------------end-'
      end
    end
  end
end
