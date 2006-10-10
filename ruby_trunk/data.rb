require 'logic/repy'

module TTime
  module Data
    REPY_File = "data/REPY"
    YAML_File = "data/technion.yml"

    def self.load
      if File::exists?(YAML_File)
        File.open(YAML_File) { |yf| YAML::load(yf.read) }
      elsif File::exists?(REPY_File)
        self.update_yaml
      else
        raise Errno::ENOENT # FIXME Try downloading, with notification
      end
    end

    def self.load_repy
      Logic::Repy.new(open(REPY_File) { |f| f.read })
    end

    def self.update_yaml
      _repy = load_repy
      open(YAML_File,"w") { |f| f.write YAML::dump(_repy.hash) }
      _repy.hash
    end
  end
end
