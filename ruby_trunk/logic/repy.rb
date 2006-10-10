require 'iconv'
require 'yaml'

require 'logic/faculty'

module TTime
  module Logic
    RawCourse = Struct.new(:header,:body)

    class Repy
      attr_reader :raw
      attr_reader :unicode

      def initialize(_raw)
        @hashed = false
        @raw = _raw
        convert_to_unicode
      end

      def hash
        return @hash if @hashed

        @hash = []

        each_raw_faculty do |name, contents|
          @hash << Faculty.new(name, contents)
        end

        @hashed = true

        hash
      end

      private

      FACULTY_BANNER_REGEX = /\+==========================================\+
\| מערכת שעות - (.*) +\|
\|.*\|
\+==========================================\+/

      COURSE_BANNER_REGEX = /\+------------------------------------------\+
\| (\d\d\d\d\d\d) +(.*) +\|
\| שעות הוראה בשבוע:ה-(\d) ת-(\d) +נק: (.*) *\|
\+------------------------------------------\+/

      def convert_to_unicode
        converter = Iconv.new('utf-8', 'cp862')
        @unicode = ""
        @raw.each_line do |l|
          @unicode << converter.iconv(l.chomp.reverse) << "\n"
        end
        @unicode
      end

      def each_raw_faculty #:yields: name, raw_faculty
        raw_faculties = @unicode.split(/\n\n/)

        raw_faculties.each do |raw_faculty|
          raw_faculty.lstrip!
          banner = raw_faculty.slice!(FACULTY_BANNER_REGEX)

          if banner
            name = FACULTY_BANNER_REGEX.match(banner)[1]
            yield name, raw_faculty
          end
        end
      end
    end
  end
end
