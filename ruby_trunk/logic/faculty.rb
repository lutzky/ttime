require 'logic/course'

module TTime
  module Logic
    class Faculty
      attr_accessor :name, :courses

      def initialize(name, courses = [])
        @name = name
        @courses = courses
      end

      def get_course(course_number)
        @courses.each do |course|
          return course if course.number.to_i == course_number.to_i
        end
        nil
      end
    end
  end
end
