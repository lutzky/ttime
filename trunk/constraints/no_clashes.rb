require 'constraint'
require 'logic/times'

class NoClashes < TTime::Constraint
  def evaluate_schedule
    # Note: Do not use Array.new(8,[]), that makes only one [] copy
    # and 8 pointers to it
    event_grid = Array.new(8) { [] }
    earliest_start = 100000000
    latest_finish = 0
    event_list.each do |ev|
      start_time = TTime::Logic::Hour::military_to_grid ev.start
      end_time = TTime::Logic::Hour::military_to_grid ev.end

      earliest_start = start_time if start_time < earliest_start
      latest_finish = end_time if end_time > latest_finish

      # Ending at xx:30 means ending at xx:20, last box isn't taken
      start_time.upto(end_time - 1) do |i|
        return false if event_grid[ev.day][i]
        event_grid[ev.day][i] = true
      end
    end
    true
  end

  def print_event_grid(event_grid)
    puts '-start------------'
    earliest_start.upto(latest_finish) do |h|
      print h
      print ": "
      0.upto(7) do |d|
        if event_grid[d][h]
          print '*'
        else
          print ' '
        end
      end
      print "\n"
    end
    puts '--------------end-'
  end
end
