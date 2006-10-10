require 'logic/course'

module TTime
  module Logic
    class Faculty
      attr_accessor :name, :courses

      def initialize(name, contents)
        @name = name
        @courses = []
        each_raw_course(contents) do |course_contents|
          @courses << Course.new(course_contents)
        end
      end

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
