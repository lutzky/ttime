require 'pp'
require 'pathname'
require 'ttime/gettext_settings'
require 'ttime/logging'
require 'set'

module TTime
  module Constraints
    class AbstractConstraint
      attr_reader :schedule

      # A name for the settings object for the current constraint. Example:
      #
      #   class MyConstraint < AbstractConstraint
      #     settings_name :my_constraint
      #     ...
      #   end
      #
      # Note: The default value is the class name, without module hierarchy.
      def AbstractConstraint.settings_name(settings_name = nil)
        @settings_name = settings_name.to_sym unless settings_name.nil?
        @default_settings ||= nil
        return @settings_name || self.name.split("::")[-1]
      end

      # Set default settings for this object. You probably want the form
      # of a hash. Example:
      #
      #   class MyConstraint < AbstractConstraint
      #     settings_name :my_constraint
      #     default_settings :enabled => true
      #     ...
      #   end
      def AbstractConstraint.default_settings(defaults = nil)
        unless defaults.nil?
          @default_settings = defaults
        end
        @default_settings
      end

      # Quick access to the Settings class. Set settings_name beforehand.
      def settings
        settings_name = self.class.settings_name
        unless settings_name
          raise Exception.new("settings_name undefined for #{self.class}")
        end
        if Settings.instance[settings_name].nil?
          Settings.instance[settings_name] = self.class.default_settings
        end
        return Settings.instance[settings_name]
      end

      # Shorthand for the class's "enabled" setting
      def enabled; self.settings[:enabled]; end
      def enabled=(enabled); self.settings[:enabled] = enabled; end

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
            log.info { "Loading constraint #{constraint}" }
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
