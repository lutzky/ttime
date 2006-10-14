module TTime
  module Logic
    class Day 
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
          puts "its #{x} day\n" if $DEBUG
          @day = @@name_to_day[x]
        end
      end
    end

    class Hour
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
