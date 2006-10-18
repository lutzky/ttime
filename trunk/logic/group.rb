#!/usr/bin/env ruby

require 'logic/times'

module TTime
  module Logic
    class Event
      attr_accessor :day, :start, :end, :place, :group

      def initialize(line,group)
        @group = group
        begin
          m=/(.+)'(\d+.\d+) ?-(\d+.\d+) *(.*)/.match(line)
          @day = Day.new(m[1]).to_i
          @start = Hour.new(m[3].reverse).to_military
          puts "at #{m[3]}" if $DEBUG
          @place = place_convert(m[4])
          @end = Hour.new(m[2].reverse).to_military
        rescue
          if $DEBUG
            $stderr.puts '-----------------------------------------------------'
            $stderr.puts 'Parse error! Could not figure out the following line:'
            $stderr.puts line
            $stderr.puts 'Match object:'
            $stderr.puts m.inspect
            $stderr.puts '-----------------------------------------------------'
          end
        end
      end

      def to_javascript
        start_box = (@start - 30) / 100
        # Ending at xx:30 means ending at xx:20, last box isn't taken
        end_box = (@end - 30) / 100 - 1

        course_index = 666
        type = Course::GROUP_TYPES.index(@group.type)
        desc = "#{@group.course.name}<br />קבוצה #{@group.number}<br />#@place"

        "addEvent(#{@group.course.number},#{type},#@day,#{start_box},#{end_box},\"#{desc}\");"
      end

      private

      def place_convert(s)
        # TODO This relies on a very fragile assumption that places in the REPY
        # file consist of two words - a building and a room, and the room is
        # sometimes numeric
        
        s = s.strip

        room, building = s.split(' ')

        return s if room.to_i == 0 # Room isn't a number - do not touch

        [ building, room.reverse ].join(' ')
      end
    end
  
    class Group
      attr_accessor :number, :lecturer, :type, :events, :course
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
        unless x =~ /^ *- *$/
          @events << Event.new(x,self) # FIXME: parse
        end
      end
  
      def set_from_heb(x,y)
        case x.strip
        when 'מרצה'
          self.lecturer = y.strip
        end
      end
    end
  end
end
