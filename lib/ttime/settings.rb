require 'pathname'
require 'yaml'
require 'singleton'

module TTime
  class Settings
    include Singleton

    SETTINGS_FILE = Pathname.new(ENV['HOME']) + '.ttime.yml'

    attr_accessor :hash

    def initialize
      load_settings
    end

    def load_settings(settings_file = nil)
      if settings_file
        settings_file = Pathname.new(settings_file)
        raise Errno::ENOENT.new(settings_file) unless settings_file.exist?
      else
        settings_file = SETTINGS_FILE
        unless settings_file.exist?
          @hash = {}
          return
        end
      end

      @hash = settings_file.open { |f| YAML::load(f.read) }
    end

    def save(settings_file = nil)
      settings_file ||= SETTINGS_FILE
      settings_file = Pathname.new(settings_file)
      settings_file.open('w') { |f| f.write YAML::dump(@hash) }
    end

    def selected_courses
      return Array.new if hash['selected_courses'].nil?
      hash['selected_courses']
    end

    def [](*args)
      hash.[](*args)
    end

    def []=(*args)
      hash.[]=(*args)
    end
  end
end
