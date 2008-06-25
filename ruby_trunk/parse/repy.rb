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

      GROUP_TYPES = {
        "הרצאה" => :lecture,
        "מעבדה" => :lab,
        "תרגיל" => :tutorial,
        "קבוצה" => :set,
      }

      DAY_NAMES = {
        'א' => 1,
        'ב' => 2,
        'ג' => 3,
        'ד' => 4,
        'ה' => 5,
        'ו' => 6,
      }


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
              grp.type = GROUP_TYPES[m[2]]
              grp.number = m[1].reverse.to_i

              if grp.number == 0
                grp.number = 10 * current_lecture_group_number
                current_lecture_group_number += 1
              end

              add_events_to_group(grp, m[3])
              state = :details
            end
          when :details:
            if line !~ /:/
              if line =~ /\| +\|/ || line =~ /\+\+\+\+\+\+/ || line =~ /----/
                course.groups << grp unless grp.events.empty?
                state = :thing
              else
                m=/\| +(.*?) +\|/.match(line)
                add_events_to_group(grp, m[1]) if m
              end
            else
              (property,value) = /\| +(.*?) +\|/.match(line)[1].split(/:/)
              set_from_heb(grp, property, value)
            end
          end
        end
        if state == :details
          course.groups << grp unless grp.events.empty?
        end

        return course
      end

      def set_from_heb(group,x,y)
        case x.strip
        when 'מרצה'
          group.lecturer = y.strip.single_space
        end
      end

      def add_events_to_group group, event_line
        if event_line =~ /^ *- *$/; return; end

        event = TTime::Logic::Event.new(group)

        return if event_line.nil?
        begin
          m=/(.+)'(\d+.\d+) ?-(\d+.\d+) *(.*)/.match(event_line)
          event.day = DAY_NAMES[m[1]]
          event.start = m[3].reverse.gsub(".","").to_i
          event.place = place_convert(m[4])
          event.end = m[2].reverse.gsub(".","").to_i
        rescue
          if $DEBUG
            $stderr.puts '-----------------------------------------------------'
            $stderr.puts 'Parse error! Could not figure out the following line:'
            $stderr.puts line
            $stderr.puts 'Match object:'
            $stderr.puts m.inspect
            $stderr.puts '-----------------------------------------------------'
          end
          raise
        end

        group.events << event
      end

      def place_convert(s)
        # TODO This relies on a very fragile assumption that places in the REPY
        # file consist of two words - a building and a room, and the room is
        # sometimes numeric

        s = s.strip.single_space

        room, building = s.split(' ')

        return s if room.to_i == 0 # Room isn't a number - do not touch

        [ building, room.reverse ].join(' ')
      end

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
