require 'constraints'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class LutzkyPreferences < AbstractConstraint
      def initialize
        super

        @enabled = true
      end

      def evaluate_schedule
        return true unless @enabled

        event_list.each do |ev|
          # There is no other besides Dado
          return false if ev.course.number == "114073" &&
            ev.group.type == :lecture &&
            ev.group.number != 10

          # Makowski please - I like my hints
          return false if ev.course.number == "234293" &&
            ev.group.type == :lecture &&
            ev.group.number != 10

          # Kolodny please - Oded's recommendation
          return false if ev.course.number == "044127" &&
            ev.group.type == :lecture &&
            ev.group.number != 10

          # Yichia please
          return false if ev.course.number == "104215" &&
            ev.group.type == :tutorial &&
            ev.group.number != 11

          # Anarchists please
          #          return false if ev.course.number == "234293" &&
          #            ev.group.type == :tutorial &&
          #            ev.group.number != 12
          # Gah, conflicts with Kolodny

        end

        true
      end

      def name
        _('Lutzky Preferences')
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
These are my own querky little preferences. This should be enhanced into
a full-blown group-selection constraint.
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
