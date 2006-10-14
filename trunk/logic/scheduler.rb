#!/usr/bin/env ruby

require 'logic/course'
require 'logic/group'
require 'logic/times'

module TTime
  module Logic
    class Schedule
      attr_reader :groups

      def initialize(course_groups_arr)
        @groups = []

        course_groups_arr.each do |course|
          @groups.concat course
        end
      end

      def events
        events = []
        @groups.each do |group|
          group.events.each do |event|
            events << event
          end
        end
        events
      end
    end

    class Scheduler
      attr_reader :ok_schedules

      def initialize(courses,constraints)
        @courses = courses
        @constraints = constraints
        @ok_schedules = []
        generate_ok_schedules
      end

      def generate_ok_schedules
        each_schedule_recusively(@courses,[]) do |groups|
          @ok_schedules << Schedule.new(groups)
        end
      end

      private
      def each_schedule_recusively(courses,group_selections)
        first = courses[0]
        rest = courses - [first]
        first.each_group_selection do |selection|
          catch(:bad_schedule) do
            new_selections = group_selections + [selection]
            @constraints.each do |con|
              unless con.accepts? new_selections
                # end this new schedule thing
                throw :bad_schedule
              end
            end #constraints.each
            if rest.empty?
              yield new_selections 
            else
              each_schedule_recusively(rest,new_selections) do |schedule|
                yield schedule
              end 
            end # if rest.empty?
          end # catch
        end
      end

    end
  end
end
