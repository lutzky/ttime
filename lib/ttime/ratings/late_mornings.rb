require 'ttime/ratings'
require 'ttime/logic/times'
require 'ttime/gettext_settings'

module TTime
  module Ratings
    class LateMornings < AbstractRating
      settings_name :sleep_late
      default_settings :enabled => false

      def rate_schedule
        return 0 unless self.enabled

        first_start = Array.new(8) 
        event_list.each do |ev|
          start_time = ev.start

          first_start[ev.day] = start_time if first_start[ev.day] == nil or start_time < first_start[ev.day] 
        end
        rating=0
        non_nil_day_count=0
        first_start.each do |hour|
          non_nil_day_count+=1
          if not hour.nil?
            hour -= 600  #FIXME: 6:00 is insufferable, should it be constant?
            if hour<0
              hour=0
            end
            
            if hour> 600 # FIXME after noon it's all the same. should this also be made an option
              hour = 600
            end
            puts "hour computed #{hour}"
            hour = (hour / 100)*100 + ((hour % 100) * 100 / 60)
            puts "hour valued #{hour}"
            rating += hour / 60 
            puts "hour rated #{hour / 60}"
          end
        end

        if non_nil_day_count !=0
            rating /= non_nil_day_count
        end

        puts " #{rating} "
        rating
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
