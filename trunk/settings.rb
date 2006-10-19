require 'pathname'
require 'yaml'

module TTime
  class Settings
    SETTINGS_FILE = Pathname.new(ENV['HOME']) + '.ttime.yml'

    attr_accessor :hash

    def initialize
      if SETTINGS_FILE.exist?
        @hash = SETTINGS_FILE.open { |f| YAML::load(f.read) }
      end
    end

    def save
      SETTINGS_FILE.open('w') { |f| f.write YAML::dump(@hash) }
    end

    def [](*args)
      hash.[](*args)
    end

    def []=(*args)
      hash.[]=(*args)
    end
  end
end
