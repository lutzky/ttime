#!/usr/bin/env ruby

require 'logic/times'

module TTime
  module Logic
    class Event
      attr_accessor :day, :start, :end, :place

      def initialize(line)
        begin
          m=/(..)'(\d\d\.\d\d)-(\d\d\.\d\d) (.*)/.match(line)
          @day = Day.new(m[1])
          @start = Hour.new(m[3].reverse)
          puts "at #{m[3]}" if $DEBUG
          @place = m[4] # FIXME reversed rooms
          @end = Hour.new(m[2].reverse)
        rescue
        end
      end

      def to_javascript
        "#{@day.to_i}, #{@start.to_i}-#{@end.to_i}"
      end
    end
  
    class Group
      attr_accessor :number, :lecturer, :type, :events
      HebToType = {
        "הרצאה" => :lecture,
        "מעבדה" => :lab,
        "קבוצה" => :set,
        "תרגיל" => :tutorial
      }
  
      def type_is? (x)
        type == x.to_sym
      end
  
      def heb_type=(x)
        @type = HebToType[x]
      end
  
      def add_hours(x)
        @events = @events.to_a
        @events << Event.new(x) # FIXME: parse
      end
  
      def set_from_heb(x,y)
      end
    end
  end
end
