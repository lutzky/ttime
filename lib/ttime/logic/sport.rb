require 'ttime/logic/faculty'
require 'ttime/logic/course'

module TTime
  module Logic
    class Sport < Faculty
      attr_accessor :name, :courses
      def initialize(name,contents)
        @name = name
        @courses = []
        each_raw_course do |course_line,group_line|
          @courses << SportCourse.new(course_line,group_line)
        end


        private
        def each_raw_course
          courses = contents.split('+---------------------------------------------------------------+')
          1.upto(courses.size/2) do |i|
          i*=2
          header = courses[i-1]
          body = courses[i]
          course_lines = body.split('|                                                               |')
          course_lines.each_with_index do |line,i|
            next if i == 1
            yield header,line
          end
      end

    end
  end
end
