#!/usr/bin/env ruby

require 'gettext'

require 'logic/course'
require 'logic/group'
require 'logic/times'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

class Array
  def count
    c = 0
    self.each do |a|
      c += 1 if yield a
    end
    c
  end

  def zero_is_one_product
    c = 1
    self.each do |a|
      c *= a unless a == 0
    end
    c
  end
end

module TTime
  module Logic
    class Schedule
      include GetText

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
      include GetText

      attr_reader :ok_schedules

      REPORT_FREQUENCY = 100

      def initialize(courses,constraints,&status_report_proc)
        @courses = courses
        @constraints = constraints
        @ok_schedules = []

        @status_report_proc = status_report_proc || proc {}

        num_types_arr = []

        @expected_schedule_count = 1
        courses.each do |course|
          Course::GROUP_TYPES.each_with_index do |type, i|
            num_types_arr[i] = course.groups.count { |g| g.type == type }
          end
          @expected_schedule_count *= num_types_arr.zero_is_one_product
        end

        generate_ok_schedules
      end

      def generate_ok_schedules
        i = 0
        catch(:cancel) do
          each_schedule_recusively(@courses,[]) do |groups|
            i += 1
            if i % REPORT_FREQUENCY == 0
              @status_report_proc.call sprintf(_("Generating schedules (%d so far)"),i)
            end
            @ok_schedules << Schedule.new(groups)
          end
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
