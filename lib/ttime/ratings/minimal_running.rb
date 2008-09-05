require 'ttime/ratings'
require 'ttime/logic/times'
require 'ttime/gettext_settings'

module TTime
  module Ratings
    class MinimalRunning < AbstractRating
      settings_name :minimal_running
      default_settings :enabled => false

      def rate_schedule
        return 0 unless self.enabled

        rating = 0
        event_list.each do |ev|
          each_following_event(ev) do |follower|
            rating -= self.distance(ev, follower)
          end
        end

        rating
      end

      def distance(ev, follower)
        if event_building(ev) != event_building(follower)
          return 2 # TODO better metric for this
        elsif ev.place.rstrip == "" or follower.place.rstrip == ""
          return 1 # No data, could be different places, don't know how bad
        elsif ev.place == follower.place
          return 0 # Exact same place
        end
      end

      def name
        _('Minimal running')
      end

      def each_following_event(ev, &proc)
        event_list.select do |candidate|
          candidate.day == ev.day and candidate.start == ev.end
        end.each(&proc)
      end

      def event_building(ev)
        ev.place.split(/\s/)[0]
      end

      def preferences_panel
        vbox = Gtk::VBox.new

        btn_enabled = Gtk::CheckButton.new(_('Enable'))
        btn_enabled.active = self.enabled

        btn_enabled.signal_connect('toggled') do
          self.enabled = btn_enabled.active?
        end

        vbox.pack_start btn_enabled, false, false

        vbox
      end

    end
  end
end
