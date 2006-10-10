#!/usr/bin/env ruby

require 'course'
require 'group'
require 'times'


class Scheduler

  def initialize(courses,constraints)
    @courses = courses
    @constraints = constraints
  end

  def each_ok_schedule
    each_schedule_recusively(@courses,[]) do |i|
      yield i
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

