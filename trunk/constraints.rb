require 'pp'
require 'gettext'

module TTime
  module Constraints
    class AbstractConstraint
      include GetText

      attr_reader :schedule

      def event_list
        @schedule.flatten.collect { |grp| grp.events }.flatten
      end

      def accepts?(schedule)
        @schedule = schedule

        evaluate_schedule
      end

      # Checks whether the given (partial) schedule is appropriate. This
      # is verified each time a schedule is generated.
      def evaulate_schedule
        true
      end

      # Checks whether the given group is appropriate. This is verified
      # before the generation of schedules.
      def evaluate_group(grp)
        true
      end
    end

    def Constraints.initialize
      Dir.glob('constraints/*.rb').each do |constraint|
        require constraint
      end
    end

    def Constraints.get_constraints
      constraint_classes = Constraints.constants.collect do |c|
        Constraints.module_eval(c)
      end - [ AbstractConstraint ]

      constraint_classes.collect do |c|
        c.new
      end
    end
  end
end
