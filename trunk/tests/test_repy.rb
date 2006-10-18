#!/usr/bin/env ruby

require 'yaml'
require 'pp'
require 'test/unit'
require 'pathname'

require 'logic/repy'
require 'tests/fixtures'

class Date
  def inspect
    "#<Date: #{to_s}>"
  end
end

class TestSchedule < Test::Unit::TestCase
  include TTime::Logic

  fixture :raw_courses

  def assert_equal_courses(c1,c2, name)
    err_string = "Error parsing course #{name}"
    assert_equal c1.academic_points, c2.academic_points, err_string
    assert_equal c1.first_test_date, c2.first_test_date, err_string
    assert_equal c1.second_test_date, c2.second_test_date, err_string
    assert_equal c1.lecturer_in_charge, c2.lecturer_in_charge, err_string
    assert_equal c1.name, c2.name, err_string
    assert_equal c1.number, c2.number, err_string

    # We are asserting that groups get parsed in the same order, which isn't
    # important, but is easier to test and makes sense.

    c1.groups.each_with_index do |g,i|
      assert_equal g.number, c2.groups[i].number, err_string
      assert_equal g.type, c2.groups[i].type, err_string
      assert_equal g.lecturer, c2.groups[i].lecturer, err_string
      g.events.each_with_index do |e,j|
        assert_equal e.day, c2.groups[i].events[j].day, err_string
        assert_equal e.start, c2.groups[i].events[j].start, err_string
        assert_equal e.end, c2.groups[i].events[j].end, err_string
        assert_equal e.place, c2.groups[i].events[j].place, err_string
      end
    end
  end

  def test_course_conversion
    raw_courses.each do |k,v|
      assert_equal_courses v['parsed'], Course.new(v['raw']), k
    end
  end
end

