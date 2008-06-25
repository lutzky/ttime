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
            faculty = TTime::Logic::Faculty.new(name)
            each_course(contents) do |course|
              faculty.courses << course
            end
            @hash << faculty
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

      def each_course contents
        courses = contents.split('+------------------------------------------+')

        1.upto(courses.size/2) do |i|
          i*=2
          c = RawCourse.new
          c.header = courses[i-1]
          c.body = courses[i]
          yield parse_raw_course(c)
        end
      end

      def parse_raw_course contents
        grp = nil
        ## sorry for the perl
        arr = /\|\s(\d\d\d\d\d\d) ([א-תףץךןם0-9()\/+#,.\-"'_: ]+?) *\|\n\| שעות הוראה בשבוע\:( *[א-ת].+?[0-9]+)+ +נק: (\d\.?\d) ?\|/.match(contents.header)
        begin
          #puts "course num: #{arr[1]}\n course name #{arr[2]}\n course hrs: #{arr[3]} | points: #{arr[arr.size-1]}\n----------\n"

          number = arr[1].reverse
          name = arr[2].strip.single_space
          course = TTime::Logic::Course.new number, name
          course.academic_points = arr[arr.size-1].reverse.to_f
          4.upto(arr.size-2) do |i|
            course.hours << arr[i]
          end
        rescue
          #puts contents.header
          raise
        end

        current_lecture_group_number = 1

        state = :start
        #puts contents.body
        contents.body.each do |line|
          case state
          when :start:
            if line[3] != '-'
              if m=/\| מורה  אחראי :(.*?) *\|/.match(line)
                course.lecturer_in_charge = m[1].strip.single_space
              elsif m=/\| מועד ראשון :(.*?) *\|/.match(line)
                course.first_test_date = convert_test_date(m[1])
              elsif m = /\| מועד שני   :(.*?) *\|/.match(line)
                course.second_test_date = convert_test_date(m[1])
              elsif line =~ /\|רישום +\|/ or line =~ /\| +\|/
                state = :thing
              end
            end
          when :thing:
            line.strip!
            if line =~ /----/
              #this should not happen
            elsif m=/\| *([0-9]*) *([א-ת]+) ?: ?(.*?) *\|/.match(line)
              grp = TTime::Logic::Group.new
              grp.course = course
              grp.heb_type =  m[2]
              grp.number = m[1].reverse.to_i

              if grp.number == 0
                grp.number = 10 * current_lecture_group_number
                current_lecture_group_number += 1
              end

              grp.add_hours(m[3])
              state = :details
            end
          when :details:
            if line !~ /:/
              if line =~ /\| +\|/ || line =~ /\+\+\+\+\+\+/ || line =~ /----/
                course.groups << grp
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
          course.groups << grp
        end

        return course
      end

      private

      def convert_test_date(s)
        israeli_date = s.split(' ')[2].reverse

        american_date = israeli_date.gsub(/^(\d\d)\/(\d\d)\/(\d\d)/,'\2/\1/\3')

        # TODO: This will only work until 2068, as years are given in two
        # digits. Let's hope REPY doesn't survive until then.
        Date.parse(american_date, true)
      end

    end
  end
end
