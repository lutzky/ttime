module TTime
  module Logic
    class Day 
      class << self
        def numeric_to_human(i)
          ['א','ב','ג','ד','ה','ו','ש'][i - 1] + "'"
        end
      end

      def to_i
        @day
      end

      @@name_to_day = {
        'sun' => 1,
        'mon' => 2,
        'tue' => 3,
        'wed' => 4,
        'thr' => 5,
        'fri' => 6, 
        'א' => 1,
        'ב' => 2,
        'ג' => 3,
        'ד' => 4,
        'ה' => 5,
        'ו' => 6 
      }

      def initialize(x)
        if x.is_a? Integer
          @day = x;
        else
          @day = @@name_to_day[x]
        end
      end
    end

    class Hour
      class << self
        def military_to_grid(hour, granularity = 15)
          (60 / granularity) * (hour / 100) + (hour % 100) / granularity
        end

        def military_to_human(hour)
          sprintf("%02d:%02d", hour / 100, hour % 100)
        end
      end

      def to_military
        @hour * 100 + @minutes
      end

      def initialize(_hour)
        split = /(\d\d?)(:|\.)(\d\d)/.match(_hour)
        @hour = split[1].to_i
        @minutes = split[3].to_i
      end
    end
  end
end
