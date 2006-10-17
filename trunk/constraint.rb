require 'pp'

module TTime
  class Constraint
    attr_reader :schedule

    def event_list
      @schedule.flatten.collect { |grp| grp.events }.flatten
    end

    def accepts?(schedule)
      @schedule = schedule

      evaluate_schedule
    end

    def evaulate_schedule
      true
    end
  end
end
