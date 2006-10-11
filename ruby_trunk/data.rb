require 'logic/repy'
#require 'yaml'

module TTime
  module Data
    USE_YAML = false

    REPY_File = "data/REPY"
    YAML_File = "data/technion.yml"
    MARSHAL_File = "data/technion.mrshl"

    def self.load(&status_report_proc)
      status_report_proc = proc {} if status_report_proc.nil?

      if USE_YAML && File::exists?(YAML_File)
        status_report_proc.call("Loading technion data from YAML",0,1)
        File.open(YAML_File) { |yf| YAML::load(yf.read) }
      elsif File::exists?(MARSHAL_File)
        status_report_proc.call("Loading technion data",0,1)
        File.open(MARSHAL_File) { |mf| Marshal.load(mf.read) }
      elsif File::exists?(REPY_File)
        status_report_proc.call("Loading data from REPY",0,1)
        if USE_YAML
          self.update_yaml(&status_report_proc)
        else
          self.update_marshal(&status_report_proc)
        end
      else
        raise Errno::ENOENT # FIXME Try downloading, with notification
      end
    end

    def self.load_repy(&status_report_proc)
      Logic::Repy.new(open(REPY_File) { |f| f.read }, &status_report_proc)
    end

    def self.update_yaml(&status_report_proc)
      _repy = load_repy(status_report_proc)
      open(YAML_File,"w") { |f| f.write YAML::dump(_repy.hash) }
      _repy.hash
    end

    def self.update_marshal(&status_report_proc)
      _repy = load_repy(&status_report_proc)
      open(MARSHAL_File,"w") { |f| f.write Marshal.dump(_repy.hash) }
      _repy.hash
    end
  end
end
