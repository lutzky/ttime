require 'pp'
require 'gettext'

module TTime
  module Constraints
    class AbstractConstraint
      include GetText

      attr_reader :schedule
      attr_reader :menu_items

      class ConstraintMenuItem
        attr_accessor :caption, :block

        def initialize(caption, block, event_required = false)
          @caption, @block, @event_required = caption, block, event_required
        end

        def event_required?
          @event_required
        end
      end

      def initialize
        @menu_items = []
      end

      def add_menu_item(caption, event_required = false, &block)
        @menu_items << ConstraintMenuItem.new(caption, block, event_required)
      end

      def event_list
        @schedule.flatten.collect { |grp| grp.events }.flatten
      end

      def accepts?(schedule)
        return true unless self.enabled?
        @schedule = schedule

        evaluate_schedule
      end

      # Is this constraint currently enabled?
      def enabled?
        true
      end

      # Handles an update in the course list (if the constraint needs it)
      def update_courses(course_list)
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
