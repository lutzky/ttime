require 'pp'
require 'pathname'
require 'ttime/gettext_settings'
require 'ttime/logging'
require 'set'
require 'yaml'

module TTime
  module Ratings
    class AbstractRating
      attr_reader :schedule

      # A name for the settings object for the current constraint. Example:
      #
      #   class MyConstraint < AbstractConstraint
      #     settings_name :my_constraint
      #     ...
      #   end
      def AbstractRating.settings_name(settings_name = nil)
        @settings_name = settings_name.to_sym unless settings_name.nil?
        @default_settings ||= nil
        return @settings_name
      end

      # Set default settings for this object. You probably want the form
      # of a hash. Example:
      #
      #   class MyConstraint < AbstractConstraint
      #     settings_name :my_constraint
      #     default_settings :enabled => true
      #     ...
      #   end
      def AbstractRating.default_settings(defaults = nil)
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

      class RatingMenuItem
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
          @menu_items << RatingMenuItem.new(caption, method_name, event_required)
        end

        attr_reader :menu_items
      end

      def event_list
        @schedule.flatten.collect { |grp| grp.events }.flatten
      end

      def rating(schedule)
        return 0 unless self.enabled?

        @schedule = schedule

        rate_schedule() * weight()
      end

      # Is this constraint currently enabled?
      def enabled?
        true
      end

      def weight
        5
      end

      # Handles an update in the course list (if the constraint needs it)
      def update_courses(course_list)
      end

      # Checks whether the given (partial) schedule is appropriate. This
      # is verified each time a schedule is generated.
      def evaulate_schedule
        5
      end

    end

    # Constraint directories are given either relative to $0's directory or
    # absolutely. All paths are searched.
    RatingPathCandidates = [
      '../lib/ttime/ratings',
      '/usr/lib/ttime/ratings',
      '/usr/share/ttime/ratings',
      '/usr/local/share/ttime/ratings',
    ] + $LOAD_PATH.collect { |p| File::join(p, 'ttime/ratings') }

    def Ratings.initialize
      my_path = Pathname.new($0).dirname
      already_loaded_ratings = Set.new
      RatingPathCandidates.collect { |p| my_path + p }.each do |path|
        Dir.glob(path + '*.rb').each do |rating|
          rating_name = File.basename(rating)
          unless already_loaded_ratings.include? rating_name
            already_loaded_ratings << rating_name
            log.info "Loading rating #{rating}"
            require rating
          end
        end
      end
    end

    def Ratings.get_ratings
      rating_class_names = Ratings.constants - \
        [ "AbstractRating", "RatingPathCandidates" ]

      rating_classes = rating_class_names.collect do |c|
        Ratings.module_eval(c)
      end

      rating_classes.collect do |c|
        c.new
      end
    end
  end
end
