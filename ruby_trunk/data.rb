require 'logic/repy'
#require 'yaml'

module TTime
  module Data
    USE_YAML = false

    REPY_File = "data/REPY"
    YAML_File = "data/technion.yml"
    MARSHAL_File = "data/technion.mrshl"

    def self.load
      if USE_YAML && File::exists?(YAML_File)
        File.open(YAML_File) { |yf| YAML::load(yf.read) }
      elsif File::exists?(MARSHAL_File)
        File.open(MARSHAL_File) { |mf| Marshal.load(mf.read) }
      elsif File::exists?(REPY_File)
        if USE_YAML
          self.update_yaml
        else
          self.update_marshal
        end
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

    def self.update_marshal
      _repy = load_repy
      open(MARSHAL_File,"w") { |f| f.write Marshal.dump(_repy.hash) }
      _repy.hash
    end
  end
end
