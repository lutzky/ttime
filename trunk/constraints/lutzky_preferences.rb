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
        true
      end

      def evaluate_group(grp)
        return true unless @enabled

        # There is no other besides Dado
        return false if grp.course.number == "114073" &&
          grp.type == :lecture &&
          grp.number != 10

        # Makowski please - I like my hints
        return false if grp.course.number == "234293" &&
          grp.type == :lecture &&
          grp.number != 10

        # Kolodny please - Oded's recommendation
        return false if grp.course.number == "044127" &&
          grp.type == :lecture &&
          grp.number != 10

        # Yichia please
        return false if grp.course.number == "104215" &&
          grp.type == :tutorial &&
          grp.number != 11

        # Anarchists please
        #          return false if grp.course.number == "234293" &&
        #            grp.type == :tutorial &&
        #            grp.number != 12
        # Gah, conflicts with Kolodny
        #
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
