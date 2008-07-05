#!/usr/bin/env ruby

require 'ttime/logic/course'
require 'ttime/logic/group'
require 'ttime/logic/times'

begin
  require 'gettext'
  GetText::bindtextdomain("ttime", "locale", nil, "utf-8")
rescue LoadError
  module GetText; def _ s; s; end; end
end

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

      REPORT_TIME = 0.5

      def initialize(courses,constraints,&status_report_proc)
        @courses = courses
        @constraints = constraints
        @ok_schedules = []

        @status_report_proc = status_report_proc || proc {}

        @courses.each_with_index do |c,i|
            c.course_id = i
        end

        num_types_arr = []

        generate_ok_schedules
      end

      def generate_ok_schedules
        @last_time = Time.now
        @status_report_proc.call sprintf(_("Generating schedules " \
                                           "(%d so far)"), 0)
        catch(:cancel) do
          each_schedule_recusively(@courses,[]) do |groups|
            @ok_schedules << Schedule.new(groups)
          end
        end
      end

      private
      def each_schedule_recusively(courses,group_selections)
        first = courses[0]
        rest = courses - [first]
        first.each_group_selection(@constraints) do |selection|
          call_status_report
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
          end # catch :bad_schedule
        end
      end

      def call_status_report
        if Time.now - @last_time > REPORT_TIME
          @last_time = Time.now
          @status_report_proc.call sprintf(_("Generating schedules " \
                                             "(%d so far)"),
                                             @ok_schedules.size)
        end
      end
    end
  end
end
