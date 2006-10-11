#!/usr/bin/env ruby

require 'logic/scheduler'
require 'test/unit'


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



class TestSchedule < Test::Unit::TestCase
  include TTime::Logic

  def test_size
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

