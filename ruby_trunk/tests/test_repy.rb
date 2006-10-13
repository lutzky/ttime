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

  def assert_equal_courses(c1,c2)
    assert_equal c1.academic_points, c2.academic_points
    assert_equal c1.first_test_date, c2.first_test_date
    assert_equal c1.second_test_date, c2.second_test_date
    assert_equal c1.lecturer_in_charge, c2.lecturer_in_charge
    assert_equal c1.name, c2.name
    assert_equal c1.number, c2.number

    # We are asserting that groups get parsed in the same order, which isn't
    # important, but is easier to test and makes sense.

    c1.groups.each_with_index do |g,i|
      assert_equal g.number, c2.groups[i].number
      assert_equal g.type, c2.groups[i].type
      assert_equal g.lecturer, c2.groups[i].lecturer
      g.events.each_with_index do |e,j|
        assert_equal e.day, c2.groups[i].events[j].day
        assert_equal e.start, c2.groups[i].events[j].start
        assert_equal e.end, c2.groups[i].events[j].end
        assert_equal e.place, c2.groups[i].events[j].place
      end
    end
  end

  def test_course_conversion
    calculus_2m = raw_courses(:calculus_2m)

    assert_equal_courses calculus_2m['parsed'], Course.new(calculus_2m['raw'])
  end
end

