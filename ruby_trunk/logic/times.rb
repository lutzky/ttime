module TTime
  module Logic
    class Day 
      class << self
        def numeric_to_human(i)
          ['א','ב','ג','ד','ה','ו','ש'][i - 1] + "'"
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
