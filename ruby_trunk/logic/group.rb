#!/usr/bin/env ruby

require 'logic/times'

module TTime
  module Logic
    class Event
      attr_reader :day,:start,:end, :place
      attr_writer :day,:start,:end, :place
      def initialize(line)
        begin
          m=/(..)'(\d\d\.\d\d)-(\d\d\.\d\d) (.*)/.match(line)
          @day = Day.new(m[1])
          @start = Hour.new(m[3].reverse)
          puts "at #{m[3]}" if $DEBUG
          @end = Hour.new(m[2].reverse)
          @place = m[4] # FIXME reversed rooms
      rescue
      end
    end
  end

  class Group
    attr_reader :number, :lecturer, :type, :events
    attr_writer :number, :lecturer, :type, :events
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
