require 'date'

require 'ttime/logic/group'

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

module TTime
  module Logic
    class Course
      attr_accessor :number, :name, :academic_points, :hours, :lecturer_in_charge,
        :first_test_date, :second_test_date, :groups

      def initialize number = nil, name = nil
        @number = number
        @name = name
        @groups = []
        @hours = []
      end

      def == other
        # We assume course numbers are unique identifiers for a course
        return ((other.is_a? Course) and (@number == other.number))
      end

      def inspect
        "#<Course #@number #{@name.inspect}>"
      end

      def to_s
        "#@number #@name"
      end

      def each_group_selection(constraints = [])
        groups_by_type = []
        TTime::Data::GROUP_TYPES.keys.each do |type|
          g = groups_of_type(type)
          constraints.each do |constraint|
            g.reject! { |grp| not constraint.evaluate_group(grp) }
          end
          groups_by_type << g if not g.empty?
        end

        if groups_by_type.empty?
          yield []
        else
          groups_by_type.one_member_of_each_member do |m|
            yield m
          end
        end
      end

      def groups_of_type (type)
        ret = []
        groups.each do |group|
          ret << group if (group.type_is?(type) && (not group.events.empty?))
        end
        return ret
      end
    end
  end
end
