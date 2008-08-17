require 'ttime/constraints'
require 'ttime/logic/times'
require 'ttime/gettext_settings'

module TTime
  module Constraints
    class NoRunning < AbstractConstraint
      settings_name :no_running

      def initialize
        super
        self.settings[:enabled] = false if self.settings[:enabled].nil?
      end

      def evaluate_schedule
        return true unless self.enabled

        event_list.each do |ev|
          each_following_event(ev) do |follower|
            return false unless event_building(ev) == event_building(follower)
          end
        end

        return true
      end

      def each_following_event(ev, &proc)
        event_list.select do |candidate|
          candidate.day == ev.day and candidate.start == ev.end
        end.each(&proc)
      end

      def event_building(ev)
        ev.place.split(/\s/)[0]
      end

      def name
        _('No running')
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        btn_enabled = Gtk::CheckButton.new(_('Enable'))
        btn_enabled.active = self.enabled

        btn_enabled.signal_connect('toggled') do
          self.enabled = btn_enabled.active?
        end

        vbox.pack_start btn_enabled, false, false
        vbox.pack_start Gtk::Label.new(
        _(<<-EOF
This constraint makes sure you don't have to run between buildings during a
10-minute (in case the lecturer finishes on time) break.
        EOF
        ))

        vbox
      end
    end
  end
end
