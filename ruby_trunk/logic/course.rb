require 'logic/group'

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

      def markup
        <<-EOF
#@number - <b>#@name</b>
#@lecturer_in_charge
        EOF
      end

      def text
        <<-EOF
#@number - #@name
מרצה אחראי: #@lecturer_in_charge
נק' אקדמיות: #@academic_points

מועד א': #@first_test_date
מועד ב': #@second_test_date

        EOF
      end

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
        if not x.is_a? RawCourse
          throw :not_a_raw_course
        end
        @groups = []
        grp = nil
        ## sorry for the perl
        arr = /\|\s(\d\d\d\d\d\d) ([א-תףץךןם0-9()\/+#,.\-"' ]+?) *\|\n\| שעות הוראה בשבוע\:( *[א-ת].+?[0-9]+)+ +נק: (\d\.?\d) ?\|/.match(x.header)
        begin
          #puts "course num: #{arr[1]}\n course name #{arr[2]}\n course hrs: #{arr[3]} | points: #{arr[arr.size-1]}\n----------\n"
          @number = arr[1].reverse
          @name = arr[2].strip
          @academic_points = arr[arr.size-1].reverse.to_f
          @hours = []
          4.upto(arr.size-2) do |i|
            hours << arr[i]
          end
        rescue
          #puts x.header
          raise
        end
        state = :start
        #puts x.body
        x.body.each do |line|
          case state
          when :start:
            if line[3] != '-'
              if m=/\| מורה  אחראי :(.*?) *\|/.match(line)
                @lecturer_in_charge = m[1]
              elsif m=/\| מועד ראשון :(.*?) *\|/.match(line)
                @first_test_date = m[1]
              elsif m = /\| מועד שני   :(.*?) *\|/.match(line)
                @second_test_date = m[1]
              elsif line =~ /\|רישום +\|/ or line =~ /\| +\|/
                state = :thing
              end
            end
          when :thing:
            if line =~ /----/
              #this should not happen
            elsif m=/\| *([0-9]*) *([א-ת]+) ?: ?(.*?) *\|/.match(line)
              grp = Group.new
              grp.heb_type =  m[2]
              grp.number = m[1].to_i
              puts "adding #{m[3]} for #{m[2]} \n" if $DEBUG
              grp.add_hours(m[3])
              state = :details
            end
          when :details:
            if line !~ /:/
              if line =~ /\| +\|/ || line =~ /\+\+\+\+\+\+/ || line =~ /----/
                @groups << grp
                state = :thing
              else
                m=/\| +(.*?) +\|/.match(line)
                grp.add_hours(m[1]) if m
              end
            else
              (property,value) = /\| +(.*?) +\|/.match(line)[1].split(/:/)
              grp.set_from_heb(property,value)
            end
          end
        end
        if state == :details
          @groups << grp
        end
      end
    end
  end
end
