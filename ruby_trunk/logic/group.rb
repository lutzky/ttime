#!/usr/bin/env ruby

require 'logic/times'
require 'logic/shared'

module TTime
  module Logic
    class Event
      attr_accessor :day, :start, :end, :place, :group

      def initialize(group)
        @group = group
      end

      def inspect
        "#<Event group=#@group>"
      end

      def frac(x)
          x/100 + ( (x%100).to_f / 60 )
      end

      def start_frac
          frac(@start)
      end

      def end_frac
          frac(@end)
      end

      def desc
        "<b>#{@group.name}</b>\nקבוצה #{@group.number}\n#@place"
      end

      def group_id
          @group.course_id
      end

      def course
        group.course
      end
    end
  
    class Group
      attr_accessor :number, :lecturer, :type, :events, :course, :description
      def initialize
        @events = []
      end

      def inspect
        "#<Group number=#@number>"
      end
  
      def course_id
          @course.course_id
      end

      def type_is? (x)
        type == x.to_sym
      end
  
      def time_as_text
        self.events.collect do |e|
          human_day = Day::numeric_to_human(e.day)
          human_start = Hour::military_to_human(e.start)
          human_end = Hour::military_to_human(e.end)
              "יום #{human_day}, #{human_start}-#{human_end}"
        end.join("\n")
      end

      def name
        @description || @course.name
      end
    end
  end
end
