require 'logger'
require 'singleton'

module TTime::Logging
  class Log < Logger
    include Singleton

    def initialize
      super(STDERR)
      self.level = Logger::ERROR
      self.datetime_format = "%H:%M:%S "
    end
  end

  def log
    TTime::Logging::Log.instance
  end
end

include TTime::Logging
