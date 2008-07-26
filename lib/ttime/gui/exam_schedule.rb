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

      find_exam_dates

      hbox = Gtk::HBox.new

      @cal = Gtk::Calendar.new
      @cal.signal_connect("month_changed") { month_changed_cb }
      @cal.signal_connect("day_selected") { day_selected_cb }

      hbox.pack_start make_exam_list_widget
      hbox.pack_start @cal

      @text_buffer = Gtk::TextBuffer.new

      tv = Gtk::TextView.new(@text_buffer)
      tv.sensitive = false

      self.vbox.pack_start hbox
      self.vbox.pack_end tv

      self.vbox.show_all

      first_test = @moed_a_hash.keys.min
      set_cal_date(first_test)
    end

    # Set the currently displayed date in @cal to +d+.
    def set_cal_date(d)
      @cal.set_year(d.year)
      @cal.set_month(d.month - 1)
      @cal.set_day(d.day)
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
        unless moed_a.nil?
          if moed_a.year == selected_year and moed_a.month == selected_month
            @cal.mark_day moed_a.day
          end
        end
        unless moed_b.nil?
          if moed_b.year == selected_year and moed_b.month == selected_month
            @cal.mark_day moed_b.day
          end
        end
      end
    end

    private

    # Prepare @moed_a_hash, @moed_b_hash, @test_dates and @colliding_dates
    def find_exam_dates
      @moed_a_hash = {}
      @moed_b_hash = {}

      @test_dates = []

      @last_moed_a = nil

      @courses.each do |course|
        unless course.first_test_date.nil?
          @test_dates << course.first_test_date
          @moed_a_hash[course.first_test_date] ||= []
          @moed_a_hash[course.first_test_date] << course
          if @last_moed_a.nil? or @last_moed_a < course.first_test_date
            @last_moed_a = course.first_test_date
          end
        end

        unless course.second_test_date.nil?
          @test_dates << course.second_test_date
          @moed_b_hash[course.second_test_date] ||= []
          @moed_b_hash[course.second_test_date] << course
        end
      end

      @colliding_dates = @test_dates.select do |d|
        @test_dates.select { |_d| _d == d }.size > 1
      end.uniq.sort

      @test_dates.sort!
      @test_dates.uniq!
    end

    def make_exam_list_widget
      @exam_list = Gtk::ListStore.new String, Date

      no_separator_written = true

      @test_dates.each do |d|
        if d > @last_moed_a and no_separator_written
          no_separator_written = false
          iter = @exam_list.append
          iter[0] = "-----"
          iter[1] = nil
        end

        iter = @exam_list.append
        iter[1] = d
        if @colliding_dates.include? d
          iter[0] = "*#{d.to_s}"
        else
          iter[0] = d.to_s
        end
      end

      view_exam_list = Gtk::TreeView.new(@exam_list)
      col = Gtk::TreeViewColumn.new("Date",                        \
                                    Gtk::CellRendererText.new,     \
                                    :text => 0)

      view_exam_list.append_column col

      view_exam_list.signal_connect('cursor_changed') do |tv|
        iter = tv.selection.selected
        set_cal_date(iter[1]) if iter and iter[1]
      end

      sc = Gtk::ScrolledWindow.new
      sc.set_policy(hscrollbar_policy = Gtk::POLICY_NEVER, \
                    vscrollbar_policy = Gtk::POLICY_AUTOMATIC)

      sc << view_exam_list

      return sc
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
