require 'pp'
require 'pathname'
require 'ttime/gettext_settings'
require 'set'

module TTime
  module Constraints
    class AbstractConstraint
      attr_reader :schedule

      class ConstraintMenuItem
        attr_accessor :caption, :method_name

        def initialize(caption, method_name, event_required = false)
          @caption, @method_name, @event_required = caption, method_name, event_required
        end

        def event_required?
          @event_required
        end
      end

      class << self
        def menu_item(method_name, caption, event_required = false)
          @menu_items ||= []
          @menu_items << ConstraintMenuItem.new(caption, method_name, event_required)
        end

        attr_reader :menu_items
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

    # Constraint directories are given either relative to $0's directory or
    # absolutely. All paths are searched.
    ConstraintPathCandidates = [
      '../lib/ttime/constraints',
      '/usr/lib/ttime/constraints',
      '/usr/share/ttime/constraints',
      '/usr/local/share/ttime/constraints',
    ] + $LOAD_PATH.collect { |p| File::join(p, 'ttime/constraints') }

    def Constraints.initialize
      my_path = Pathname.new($0).dirname
      already_loaded_constraints = Set.new
      ConstraintPathCandidates.collect { |p| my_path + p }.each do |path|
        Dir.glob(path + '*.rb').each do |constraint|
          constraint_name = File.basename(constraint)
          unless already_loaded_constraints.include? constraint_name
            already_loaded_constraints << constraint_name
            require constraint
          end
        end
      end
    end

    def Constraints.get_constraints
      constraint_class_names = Constraints.constants - \
        [ "AbstractConstraint", "ConstraintPathCandidates" ]

      constraint_classes = constraint_class_names.collect do |c|
        Constraints.module_eval(c)
      end

      constraint_classes.collect do |c|
        c.new
      end
    end
  end
end
