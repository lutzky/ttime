#!/usr/bin/env ruby

class Array

  def one_member_of_each_member
    # this is a tricky one...
    #
    # what we want is one member of each array
    # that is a member of the array we are currently
    # proccessing.
    # a recursive definition of thisw would be
    # each member of our first array added to all the 
    # possible such selections of our other arrays,
    # recusively.
    # or just each member if it is alone

    first = self[0]
    rest = self - [first]
    if rest.empty?
      first.each do |member|
        yield [member]
      end
    else
      first.each do |member|
        rest.one_member_of_each_member do |other_members|
          yield [member] + other_members
        end
      end
    end
  end
end

class Course
  attr_reader :number, :name, :academic_points, :lecture_hours, :tutorial_hours, :lecturer_in_charge, :first_test_date, :second_test_date, :groups
  attr_writer :number, :name, :academic_points, :lecture_hours, :tutorial_hours, :lecturer_in_charge, :first_test_date, :second_test_date, :groups


  def each_group_selection
    groups_by_type = []
    group_types.each do |type|
      g = groups_of_type(type)
      groups_by_type << g if not g.empty?
    end
    groups_by_type.one_member_of_each_member do |m|
      yield m
    end
  end

  def groups_of_type (type)
    ret = []
    groups.each do |group|
      ret << group if group.type_is? type
    end
    return ret

  end

  # the group type constants
  # FIXME: move me!
  def group_types
    [:lecture,:tutorial,:lab]
  end

def initialize(x)
# do something here!

end

end

