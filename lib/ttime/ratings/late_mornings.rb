require 'ttime/ratings'
require 'ttime/logic/times'
require 'ttime/gettext_settings'

module TTime
  module Ratings
    class LateMornings < AbstractRating
      include Logic

      settings_name :sleep_late
      default_settings :enabled => false

      # Earliest time (in military units) for wakeup
      TooEarly = 600

      # Latest time (in military units) for useful sleeping
      LateEnough = 1200

      def rate_schedule
        return 0 unless self.enabled

        first_start = Array.new(8)

        event_list.each do |ev|
          start_time = ev.start
          first_start[ev.day] ||= start_time
          first_start[ev.day] = [ first_start[ev.day], start_time ].min
        end

        first_start.reject! { |x| x.nil? }

        return 0 if first_start.empty?

        rating = 0

        first_start.each do |hour|
          if not hour.nil?
            hour = TooEarly if hour < TooEarly
            hour = LateEnough if hour > LateEnough

            time_to_wake = Hour::military_to_fraction(hour - TooEarly)

            rating += time_to_wake
          end
        end

        rating / first_start.size.to_f
      end

      def name
        _('Extra sleep')
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
