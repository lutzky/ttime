require 'iconv'
require 'gtk2'

require 'course'

RawCourse= Struct.new(:header,:body)

def gtk_debug_output(msg)
  $tb_dest.text = $tb_dest.text + msg.to_s + "\n"
end

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
      each_raw_course(contents) do |course|
        gtk_debug_output course.header
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

def get_faculty(repy_text)
end

Gtk.init

w = Gtk::Window.new


$tb_dest = Gtk::TextBuffer.new
tv_dest = Gtk::TextView.new $tb_dest
sw_dest = Gtk::ScrolledWindow.new nil, nil
sw_dest.set_policy Gtk::POLICY_AUTOMATIC, Gtk::POLICY_AUTOMATIC
sw_dest.shadow_type = Gtk::SHADOW_IN
sw_dest.add tv_dest

my_text = open("REPY") { |f| f.read }

my_repy = Repy.new(my_text)

$tb_dest.text = ""

my_repy.hash

w.border_width = 5

w.title = 'ICQ Hebrew Fixer'

w.add sw_dest

w.signal_connect('remove') do
  Gtk.main_quit
end

w.set_default_size 600, 300

w.show_all

Gtk.main
