require 'gtk2'
require 'pathname'
require 'date'

if __FILE__ == $0
  $:.unshift(Pathname.new($0).dirname + "..")
end

require 'ttime/logic/course'
require 'ttime/gettext_settings'

module TTime::GUI
  class ExamSchedule < Gtk::Dialog
    # Initialize an exam schedule with an array of TTime::Course objects.
    # The test dates for the objects will be displayed in the schedule.
    def initialize(courses, parent=nil)
      super(_("Exam Schedule"), parent, Gtk::Dialog::DESTROY_WITH_PARENT,
            [ Gtk::Stock::OK, Gtk::Dialog::RESPONSE_NONE ])

      @courses = courses

      @moed_a_hash = {}
      @moed_b_hash = {}

      @test_dates = []

      @courses.each do |course|
        @test_dates << course.first_test_date
        @test_dates << course.second_test_date

        @moed_a_hash[course.first_test_date] ||= []
        @moed_a_hash[course.first_test_date] << course

        @moed_b_hash[course.second_test_date] ||= []
        @moed_b_hash[course.second_test_date] << course
      end

      @colliding_dates = @test_dates.select do |d|
        @test_dates.select { |_d| _d == d }.size > 1
      end.uniq.sort

      first_test = @moed_a_hash.keys.min

      @cal = Gtk::Calendar.new
      @cal.signal_connect("month_changed") { month_changed_cb }
      @cal.signal_connect("day_selected") { day_selected_cb }

      @text_buffer = Gtk::TextBuffer.new

      tv = Gtk::TextView.new(@text_buffer)
      tv.sensitive = false

      self.vbox.pack_start @cal
      self.vbox.pack_end tv

      self.vbox.show_all

      @cal.set_year(first_test.year)
      # Gtk::Calendar uses 0-based months!
      @cal.set_month(first_test.month - 1)
      @cal.set_day(first_test.day)
    end

    def day_selected_cb
      @text_buffer.text = ""
      iter = @text_buffer.get_iter_at_offset(0)

      unless @colliding_dates.empty?
        tag = @text_buffer.create_tag(nil, {
          :font => 'Sans Bold',
          :foreground => 'Red',
        })

        warning_msg = ngettext(
            "WARNING, collision on ",
            "WARNING, collisions on: ",
          @colliding_dates.size)

        @text_buffer.insert(iter, warning_msg, tag)

        colliding_dates = @colliding_dates.collect { |d| d.strftime }.join(", ")

        @text_buffer.insert(iter, "#{colliding_dates}\n", tag)
      end

      # Gtk::Calendar uses 0-based months!
      selected_date = Date::civil(@cal.year, @cal.month + 1, @cal.day)

      if @moed_a_hash[selected_date]
        @moed_a_hash[selected_date].each do |course|
          @text_buffer.insert(iter, format(_("Moed A: %s\n"), course.name))
        end
      end

      if @moed_b_hash[selected_date]
        @moed_b_hash[selected_date].each do |course|
          @text_buffer.insert(iter, format(_("Moed B: %s\n"), course.name))
        end
      end
    end

    def month_changed_cb
      @cal.clear_marks

      # Gtk::Calendar uses 0-based months!
      selected_month = @cal.month + 1
      selected_year = @cal.year

      @courses.each do |course|
        moed_a = course.first_test_date
        moed_b = course.second_test_date
        if moed_a.year == selected_year and moed_a.month == selected_month
          @cal.mark_day moed_a.day
        end
        if moed_b.year == selected_year and moed_b.month == selected_month
          @cal.mark_day moed_b.day
        end
      end
    end
  end
end

if __FILE__ == $0
  include TTime::GUI

  c1 = TTime::Logic::Course.new("123456", "My first course")
  c1.first_test_date = Date::parse("2008-7-25")
  c1.second_test_date = Date::parse("2008-8-13")

  c2 = TTime::Logic::Course.new("123457", "My second course")
  c2.first_test_date = Date::parse("2008-7-26")
  c2.second_test_date = Date::parse("2008-8-13")

  es = ExamSchedule.new([c1, c2])
  es.signal_connect("destroy") {  Gtk.main_quit }
  es.run
end
