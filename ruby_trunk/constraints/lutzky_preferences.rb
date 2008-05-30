require 'constraints'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Constraints
    class LutzkyPreferences < AbstractConstraint
      def initialize
        super

        @enabled = false

        @allowed_groups = {
            "044147" => { :lecture => [10,20], :tutorial => [12,13,22] },
            "236360" => { :tutorial => [11] }
        }
      end

      def evaluate_schedule
        true
      end

      def evaluate_group(grp)
        return true unless @enabled

        @allowed_groups.each do |course, selections|
          if grp.course.number == course
            if selections.include?(grp.type)
              return false unless selections[grp.type].include? grp.number
            end
          end
        end

        true
      end

      def name
        _('Lutzky Preferences')
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        btn_enabled = Gtk::CheckButton.new(_('I am Lutzky'))
        btn_enabled.active = @enabled

        btn_enabled.signal_connect('toggled') do
          @enabled = btn_enabled.active?
        end
        vbox.pack_start btn_enabled, false, false

        vbox
      end
    end
  end
end
