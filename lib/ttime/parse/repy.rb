require 'iconv'
require 'ttime/logic/faculty'
require 'ttime/gettext_settings'

$KCODE='u'
require 'jcode'

class String
  def u_leftchop! n
    # Essentially the same as str.slice!(0..n-1), but works for unicode
    tmp = self.scan(/./m)[0..n-1].join("")
    self[0..-1] = self.scan(/./m)[n..-1].join("")
    tmp
  end
end

module TTime
  module Parse
    RawCourse = Struct.new(:header,:body)

    class Repy
      include GetText

      Expr = {
        :faculty => /
        # Contents: 1 is the faculty name
        \+==========================================\+\n
        \|\sמערכת\sשעות\s-\s(.*)\s+\|\n
        \|.*\|\n
        \+==========================================\+/x,

        :sports => /
        \s*\+===============================================================\+\n
        \|.*מקצועות\sספורט.*\|\n
        \+===============================================================\+
        /x,

        :course_head => /
        # Contents: 1 is the course number, in reverse
        #           2 is the course name
        #           3 is the alleged number of weekly hours
        #           4 is the amount of academic points
        \|\s+(\d{6})\s+(.*?)\s*\|\n
        \|\s+שעות\sהוראה\sבשבוע\:(\s*[א-ת].+?[0-9]+)*\s+נק:\s(\d\.?\d)\s*\|
        /x,

        :teacher_in_charge => /
          מורה\s+אחראי\s*:\s*
          (.*?) # Teacher's name
          \s*\n
          \s*-+\s*\n
          /x,

        :course_notes => /
          # If we have notes at the beginning of a course, the list starts
          # with a "1.", and ends with a double newline
          \s*1\.
          (.|\n)*?
          \n\s*\n # Double newlines have leading spaces :(
          /x,

        :sports_group => /
          \s*
          (\d\d)            # Group number
          \s+
          (.*)              # Group description
          \s+
          /x,

        :sports_event => /
          \s*
          (א|ב|ג|ד|ה|ו|ש)'  # Day of week (Note, using [אבגדהוש] doesn't work?)
          (\d+\.\d+)        # END time
          [ -]*
          (\d+\.\d+)        # START time
          \s+
          (.*)             # Place
          /x,

        :sports_instructor => /\s+מדריך:\s(.*)/
      }

      # Sports groups, number + description, are always exactly this long
      # (with padding after)
      SPORTS_GROUP_LEN = 20

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

      def inspect
        "#<Repy>"
      end

      def hash
        return @hash if @hashed

        @hash = []

        each_raw_faculty do |name, contents,is_sport|
          faculty = TTime::Logic::Faculty.new(name)
          each_raw_course(contents) do |raw_course|
            if raw_course.body
              if is_sport
                faculty.courses << parse_raw_sports_course(raw_course)
              else
                faculty.courses << parse_raw_course(raw_course)
              end
            end
          end
          @hash << faculty
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
          banner = raw_faculty.slice!(Expr[:faculty])

          if banner
            name = Expr[:faculty].match(banner)[1].strip.squeeze(" ")
            yield name, raw_faculty, false
          elsif raw_faculty != ""
              banner = raw_faculty.slice!(Expr[:sports])
              if banner
                name = "חינוך גופני"
                yield name, raw_faculty, true
              end
          end
        end
      end

      def each_raw_course contents
        courses = contents.split(/\+-+\+/)

        1.upto(courses.size/2) do |i|
          i*=2
          c = RawCourse.new
          c.header = courses[i-1]
          c.body = courses[i]
          yield c
        end
      end

      def parse_course_header header
        arr = Expr[:course_head].match header
        begin
          number = arr[1].reverse
          name = arr[2].strip.squeeze(" ")
          course = TTime::Logic::Course.new number, name
          course.academic_points = arr[arr.size-1].reverse.to_f
          4.upto(arr.size-2) do |i|
            course.hours << arr[i]
          end
        rescue
          raise
        end

        return course
      end

      def parse_raw_course contents
        grp = nil

        course = parse_course_header contents.header

        current_lecture_group_number = 1

        state = :start
        #puts contents.body
        contents.body.each do |line|
          case state
          when :start:
            if line[3] != '-'
              if m=/\| מורה  אחראי :(.*?) *\|/.match(line)
                course.lecturer_in_charge = m[1].strip.squeeze(" ")
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

              add_event_to_group(grp, m[3])
              state = :details
            end
          when :details:
            if line !~ /:/
              if line =~ /\| +\|/ || line =~ /\+\+\+\+\+\+/ || line =~ /----/
                course.groups << grp unless grp.events.empty?
                state = :thing
              else
                m=/\| +(.*?) +\|/.match(line)
                add_event_to_group(grp, m[1]) if m
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

      def parse_raw_sports_course(raw_course)
        course = parse_course_header(raw_course.header)
        body = raw_course.body
        body.gsub!("|","")
        body.lstrip!

        body =~ Expr[:teacher_in_charge]
        course.lecturer_in_charge = $1.squeeze(" ") if $1
        body.slice!(Expr[:teacher_in_charge])

        course_notes = body.slice!(Expr[:course_notes])
        # TODO: Well... we have the course notes. Do we want them?

        raw_groups = body.split(/\n\s+\n/)

        raw_groups.each do |raw_group|
          group = parse_raw_sports_group raw_group, course
          course.groups << group
        end

        course
      end

      def parse_raw_sports_group(raw_group, course)
        group = TTime::Logic::Group.new
        group.course = course
        group.type = :other
        raw_group.lstrip!
        header = raw_group.u_leftchop!(SPORTS_GROUP_LEN)

        header =~ Expr[:sports_group]
        group.number, group.description = $1.to_i, $2.rstrip

        raw_group.split("\n").each do |raw_event|
          if raw_event =~ Expr[:sports_instructor]
            group.lecturer = $1.squeeze(" ")
            # Only a dash line should remain, so we stop the loop here
            break
          elsif raw_event =~ Expr[:sports_event]
            event = TTime::Logic::Event.new(group)
            event.day, raw_start, raw_end, event.place =
              DAY_NAMES[$1], $3, $2, $4.rstrip

            event.start, event.end = [raw_start, raw_end].collect do |s|
              s.reverse.gsub(".", "").to_i
            end

            group.events << event
          else
            $stderr.write "Ignoring bad event line " \
              "for course #{group.course.number}, " \
              "group #{group.number} (#{group.description})"
            if $DEBUG
              $stderr.write ":\n#{raw_event}"
            end
            $stderr.write("\n")
          end
        end

        group
      end

      def set_from_heb(group,x,y)
        case x.strip
        when 'מרצה'
          group.lecturer = y.strip.squeeze(" ")
        end
      end

      def add_event_to_group group, event_line
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
          parse_error line, m
        end

        group.events << event
      end

      def parse_error line, match
          $stderr.puts '-----------------------------------------------------'
          $stderr.puts 'Parse error! Could not figure out the following line:'
          $stderr.puts line
          $stderr.puts 'Match object:'
          $stderr.puts match.inspect
          $stderr.puts '-----------------------------------------------------'
        raise "Parse error when reading REPY file"
      end

      def place_convert(s)
        # TODO This relies on a very fragile assumption that places in the REPY
        # file consist of two words - a building and a room, and the room is
        # sometimes numeric

        s = s.strip.squeeze(" ")

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
