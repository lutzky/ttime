#!/usr/bin/env ruby

TimeGranularity = 15

# FIXME: This is already available in times.rb
#
# Explanation: A 'grid' time is essentially a row in the grid, which
# would depend on TimeGranularity. Military time is human-readable 24h-time
# without the colon - for example, 1730 is Half past Five PM. An hour would
# then be 100 'military time units'
class Hour
  class << self
    def military_to_grid(hour, granularity = TimeGranularity)
      (60 / granularity) * (hour / 100) + (hour % 100) / granularity
    end

    def grid_to_human(grid, granularity = TimeGranularity)
      sprintf("%02d:%02d", (grid * granularity) / 60,
              (grid * granularity) % 60)
    end

    # Check whether the given grid time should display a time divisor
    # For Technion uses, we want it at the 30 minute mark. This must divide
    # by TimeGranularity!
    def is_divisor(grid, granularity = TimeGranularity, dividepoint = 30)
      (grid * granularity) % 60 == dividepoint
    end
  end
end

EventBox = Struct.new(:name, :location, :day, :start_time, :length)

class EventBox
  def start_box
    Hour.military_to_grid(start_time)
  end
  def box_length
    Hour.military_to_grid(length)
  end
  def end_box
    Hour.military_to_grid(start_time) + Hour.military_to_grid(length) - 1
  end
end

Events = [
  # Sunday
  EventBox.new("Complex Functions", "Ullman 310", 1, 1030, 200),
  EventBox.new("ICT", "Phisbach 403", 1, 1230, 100),
  EventBox.new("Semiconductors", "Meyer 351", 1, 1330, 100),
  EventBox.new("Logic and Set Theory", "Ullman 304", 1, 1430, 200),
  
  # Monday
  EventBox.new("Logic and Set Theory", "Taub 9", 2, 930, 300),
  EventBox.new("Physics 3H", "Physics 1", 2, 1230, 200),

  # Tuesday
  EventBox.new("Complex Functions", "Ullman 602", 3, 930, 100),
  EventBox.new("PDEs", "Ullman 205", 3, 1030, 200),
  EventBox.new("ICT", "Meyer 280", 3, 1430, 300),

  # Wednesday
  EventBox.new("Semiconductors", "Meyer 165", 4, 830, 200),
  EventBox.new("Physics 3H", "Physics 323", 4, 1130, 100),
  EventBox.new("MAS", "Blumfield 424", 4, 1330, 300),
  EventBox.new("MAS", "Blumfield 310", 4, 1630, 100),

  # Thursday
  EventBox.new("Semiconductors", "Meyer 165", 5, 930, 100),
  EventBox.new("PDEs", "Ullman 201", 5, 1030, 100),
  EventBox.new("Physics 3H", "Ullman 201", 5, 1230, 100)
]

EventColors = ["#ccf","#cfc","#cff","#fcc","#fcf","#ffc","#ccc","#fff"]

def color_for_course(course_name)
  course_names = Events.collect { |ev| ev.name }.uniq

  EventColors[course_names.index(course_name)]
end

# Figures out which days are to be displayed on the schedule
def day_range
  day_comparison = Proc.new { |a,b| a.day <=> b.day }
  Events.min(&day_comparison).day .. Events.max(&day_comparison).day
end

# Figures out which timeslots are to be displayed
def timeslot_range
  min = Events.min do |a,b|
    a.start_box <=> b.start_box
  end.start_box

  # The first row of the table must draw a divisor, so compensate
  # if earliest event isn't on a divisor row
  until Hour.is_divisor(min)
    min -= 1
    raise "Divisability error in schedule rendering" if min < 0
  end

  max = Events.max do |a,b|
    a.end_box <=> b.end_box
  end.end_box

  min..max
end

def get_event(params = {})
  raise 'Day not specified' unless params[:day]
  
  if params[:start_box]
    Events.each do |ev|
      return ev if ev.day == params[:day] && ev.start_box == params[:start_box]
    end
    return nil
  end

  if params[:continue]
    Events.each do |ev|
      return ev if ev.day == params[:day] &&
        (ev.start_box + 1..ev.end_box).include?(params[:continue])
    end
      return nil
  end

  raise 'No timeslot specified'
end

puts <<EOF
<head><link rel="stylesheet" href="sched.css" /></head>
<table>

<tr>
  <th></th>
EOF

DayNames = ['Nilday','Sunday','Monday','Tuesday','Wednesday','Thursday','Friday', 'Saturday']

puts "<tr><th></th>"
puts day_range.collect { |i| "<th>#{DayNames[i]}</th>" }.join
puts "</tr>"

timeslot_range.each do |timeslot|
  puts "<tr>"
  if Hour.is_divisor(timeslot)
    puts "<td rowspan=\"#{60 / TimeGranularity}\">#{Hour.grid_to_human(timeslot)}</td>"
  end
  day_range.each do |day|
    ev = get_event(:day => day, :start_box => timeslot)
    if ev.nil?
      blocker = get_event(:day => day, :continue => timeslot)
      if blocker.nil?
        puts "<td></td>"
      end
    else
      puts "<td class=\"course\" style=\"background-color: #{color_for_course ev.name}\" rowspan=\"#{ev.box_length}\">#{ev.name}<br />#{ev.location}</td>"
    end
  end
  puts "</tr>"
end
puts "</table>"
