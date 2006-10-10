require 'iconv'
require 'yaml'
require 'logic/course'

RawCourse = Struct.new(:header,:body)

class Repy
  attr_reader :raw
  attr_reader :unicode
  attr_reader :hash

  def initialize(_raw)
    @hashed = false
    @raw = _raw
    convert_to_unicode
  end

  def load_to_ruby
    return @hash if @hashed

    @hash = []

    each_raw_faculty do |name, contents|
      each_raw_course(contents) do |course|
        @hash << Course.new(course)
      end
    end

    @hashed = true
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

    puts raw_faculties.size
  end
end

def each_raw_course(faculty)
  courses = faculty.split('+------------------------------------------+')

  1.upto(courses.size/2) do |i|
    i*=2
    c = RawCourse.new
    c.header = courses[i-1]
    c.body = courses[i]
    yield c
  end
end
