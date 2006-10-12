#!/usr/bin/env ruby

require 'yaml'
require 'pp'
require 'test/unit'
require 'pathname'

require 'logic/scheduler'
require 'tests/fixtures'

class TrueCon
  def accepts?(x)
    true
  end
end


class FalseCon
  def accepts?(x)
    false
  end
end


class EvenCon
  def accepts?(x)
    @i = false if @i.nil?
    @i =  !(@i)
    return @i
  end
end

class Array
  def count
    c = 0
    self.each do |a|
      c += 1 if yield a
    end
    c
  end
end

class TestSchedule < Test::Unit::TestCase
  include TTime::Logic

  fixture :courses

  def assert_group_equality(arr1, arr2)
    arr1.each do |m|
      assert arr2.include?(m)
    end
    arr2.each do |m|
      assert arr1.include?(m)
    end
  end

  def test_one_member_of_each_member
    orig_array = [[1,2,3],[4,5,6],[7,8,9]]
    expected_array = [
      [1,4,7],[1,4,8],[1,4,9],
      [1,5,7],[1,5,8],[1,5,9],
      [1,6,7],[1,6,8],[1,6,9],
      [1,4,7],[1,4,8],[1,4,9],
      [1,5,7],[1,5,8],[1,5,9],
      [1,6,7],[1,6,8],[1,6,9],
      [2,4,7],[2,4,8],[2,4,9],
      [2,5,7],[2,5,8],[2,5,9],
      [2,6,7],[2,6,8],[2,6,9],
      [2,4,7],[2,4,8],[2,4,9],
      [2,5,7],[2,5,8],[2,5,9],
      [2,6,7],[2,6,8],[2,6,9],
      [3,4,7],[3,4,8],[3,4,9],
      [3,5,7],[3,5,8],[3,5,9],
      [3,6,7],[3,6,8],[3,6,9],
      [3,4,7],[3,4,8],[3,4,9],
      [3,5,7],[3,5,8],[3,5,9],
      [3,6,7],[3,6,8],[3,6,9]
    ]
    tgt_array = []

    orig_array.one_member_of_each_member do |m|
      tgt_array << m
    end

    assert_group_equality expected_array, tgt_array
  end

  def test_scheduling
    lec_groups = courses(:fake_calculus).groups.count { |g| g.type == :lecture }
    tut_groups = courses(:fake_calculus).groups.count { |g| g.type == :tutorial }

    combinations = lec_groups * tut_groups

    scheduler = Scheduler.new([courses(:fake_calculus)],[])

    assert_equal combinations, scheduler.ok_schedules.size

    scheduler.ok_schedules.each do |s|
      assert_equal 1, s.groups.count { |g| g.type == :lecture }
      assert_equal 1, s.groups.count { |g| g.type == :tutorial }
    end
  end

  def rem_test_size
    c = Course.new
    c.number = 236350
    c.name = "hagana"
    c.academic_points = 3.5
    c.groups = []

    d = Course.new
    d.number = 236350
    d.name = "hagana"
    d.academic_points = 3.5
    d.groups = []


    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :lecture
    c.groups << g




    i=0
    c.each_group_selection do |gr|
      i+=1
    end

    assert_equal(1,i)


    s = Scheduler.new([c],[])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(1,i)

    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :lecture
    c.groups << g


    i=0
    c.each_group_selection do |gr|
      i+=1
    end

    assert_equal(2,i)


    s = Scheduler.new([c],[])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(2,i)

    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :tutorial
    c.groups << g


    i=0
    c.each_group_selection do |gr|
      i+=1
    end

    assert_equal(2,i)


    s = Scheduler.new([c],[])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(2,i)

    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :tutorial
    c.groups << g


    i=0
    c.each_group_selection do |gr|
      i+=1
    end

    assert_equal(4,i)


    s = Scheduler.new([c],[])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(4,i)

    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :tutorial
    d.groups << g

    g = Group.new
    g.day = Day.new("sun")
    g.start_time = Hour.new("8:30")
    g.end_time = Hour.new("10:30")
    g.room = "taub 7"
    g.lecturer = "orr dunkleman"
    g.type = :tutorial
    d.groups << g


    s = Scheduler.new([c,d],[])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(8,i)

  s = Scheduler.new([c,d],[TrueCon.new])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(8,i)

  s = Scheduler.new([c,d],[FalseCon.new])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(0,i)



  s = Scheduler.new([c,d],[EvenCon.new,TrueCon.new])
    i=0
    s.each_ok_schedule do |s|
      i+=1
    end
    assert_equal(2,i)





  end
end

