require 'logic/course'

module TTime
  module Logic
    class Faculty
      attr_accessor :name, :courses

      def initialize(name, contents = nil)
        @name = name
        @courses = []
        if contents
          each_raw_course(contents) do |course_contents|
            @courses << Course.new(course_contents)
          end
        end
      end

      def get_course(course_number)
        @courses.each do |course|
          return course if course.number.to_i == course_number.to_i
        end
        nil
      end

      private
      def each_raw_course(contents)
        courses = contents.split('+------------------------------------------+')

        1.upto(courses.size/2) do |i|
          i*=2
          c = RawCourse.new
          c.header = courses[i-1]
          c.body = courses[i]
          yield c
        end
      end
    end
  end
end
