require 'iconv'
require 'gettext'

require 'logic/faculty'
require 'logic/shared'

GetText::bindtextdomain("ttime", "locale", nil, "utf-8")

module TTime
  module Parse
    RawCourse = Struct.new(:header,:body)

    class Repy
      include GetText

      FACULTY_BANNER_REGEX = /\+==========================================\+
\| מערכת שעות - (.*) +\|
\|.*\|
\+==========================================\+/

      COURSE_BANNER_REGEX = /\+------------------------------------------\+
\| (\d\d\d\d\d\d) +(.*) +\|
\| שעות הוראה בשבוע:ה-(\d) ת-(\d) +נק: (.*) *\|
\+------------------------------------------\+/


     SPORTS_BANNER_REGEX = /\+===============================================================\+
\| *(מקצועות ספורט.*) *\|
\+===============================================================\+/



      attr_reader :raw
      attr_reader :unicode

      def initialize(_raw, &status_report_proc)
        @hashed = false
        @raw = _raw

        @status_report_proc = status_report_proc
        @status_report_proc = proc {} if @status_report_proc.nil?
        convert_to_unicode
      end

      def hash
        return @hash if @hashed

        @hash = []

        each_raw_faculty do |name, contents,is_sport|
          if is_sport
            #@hash << Sport.new(name, contents)
          else
            @hash << TTime::Logic::Faculty.new(name, contents)
          end
        end

        @hashed = true

        hash
      end

      private

      def convert_to_unicode
        converter = Iconv.new('utf-8', 'cp862')
        @unicode = ""
        @raw.each_line do |l|
          @unicode << converter.iconv(l.chomp.reverse) << "\n"
        end
        @unicode
      end

      def each_raw_faculty #:yields: name, raw_faculty, is_sports?
        raw_faculties = @unicode.split(/\n\n/)

        raw_faculties.each_with_index do |raw_faculty,i|
          @status_report_proc.call(_("Loading REPY file..."), i.to_f / raw_faculties.size.to_f)

          raw_faculty.lstrip!
          banner = raw_faculty.slice!(FACULTY_BANNER_REGEX)

          if banner
            name = FACULTY_BANNER_REGEX.match(banner)[1].strip.single_space
            yield name, raw_faculty, false
          else
              banner =  raw_faculty.slice!(SPORTS_BANNER_REGEX)
              if banner
                  name = SPORTS_BANNER_REGEX.match(banner)[1].strip.single_space
                  yield name, raw_faculty, true
              end
          end
        end
      end
    end
  end
end
