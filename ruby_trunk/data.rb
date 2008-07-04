require 'open-uri'
require 'tempfile'
require 'pathname'
require 'parse/repy'

begin
  require 'gettext'
  GetText::bindtextdomain("ttime", "locale", nil, "utf-8")
rescue LoadError
  module GetText; def _ s; s; end; end
end
#require 'yaml'


module TTime
  class NoSuchCourse < Exception; end

  class Data
    include GetText

    attr_reader :data

    class << self
    end

    GROUP_TYPES = {
      :lecture => _('Lecture'),
      :tutorial => _('Tutorial'),
      :lab => _('Lab'),
      :other => ('Other'),
    }

    def initialize(force = false, &status_report_proc)
      @status_report_proc = status_report_proc
      @status_report_proc = proc {} if @status_report_proc.nil?

      if force == :convert and File::exists?(REPY_File)
        @data = convert_repy
      elsif force
        @data = download_repy
      else
        if USE_YAML && File::exists?(YAML_File)
          report _("Loading technion data from YAML")
          @data = File.open(YAML_File) { |yf| YAML::load(yf.read) }
        elsif File::exists?(UDonkey_XML_File)
          report _("Loading UDonkey XML data")
          require 'parse/udonkey_xml'
          @data = TTime::Parse::UDonkeyXML.convert_udonkey_xml UDonkey_XML_File
        elsif File::exists?(MARSHAL_File)
          report _("Loading technion data")
          @data = File.open(MARSHAL_File) { |mf| Marshal.load(mf.read) }
        elsif File::exists?(REPY_File)
          @data = convert_repy
        else
          @data = download_repy
        end
      end
    end

    def find_course_by_num(course_num)
      data.each do |faculty|
        faculty.courses.each do |course|
          return course if course.number.to_s == course_num.to_s
        end
      end

      raise NoSuchCourse, "There is no course with number #{course_num}"
    end

    def [](*args)
      data.[](*args)
    end

    def []=(*args)
      data.[]=(*args)
    end

    def each(*args, &block)
      data.each(*args, &block)
    end

    def each_with_index(*args, &block)
      data.each_with_index(*args, &block)
    end

    def size
      data.size
    end

    private

    USE_YAML = false

    DATA_DIR = Pathname.new "data/"

    REPY_Zip_filename = "REPFILE.zip"
    REPY_Zip = DATA_DIR + REPY_Zip_filename
    REPY_File = DATA_DIR + "REPY"
    REPY_URI = "http://ug.technion.ac.il/rep/REPFILE.zip"
    YAML_File = DATA_DIR + "technion.yml"
    MARSHAL_File = DATA_DIR + "technion.mrshl"
    UDonkey_XML_File = DATA_DIR + "MainDB.xml"

    def convert_repy
      report _("Loading data from REPY")
      if USE_YAML
        update_yaml
      else
        update_marshal
      end
    end

    def download_repy
      report _("Downloading REPY file from Technion")

      DATA_DIR.mkpath

      Tempfile.open("rwb") do |tf|
          tf.binmode
          open(REPY_URI) do |in_file|
              tf.write in_file.read
          end

          tf.seek(0)

          report _("Extracting REPY file"), 0.5

          require 'zip/zip'

          Zip::ZipInputStream.open(tf.path) do |zis|
              entry = zis.get_next_entry
              open(REPY_File, "w") do |dest_file|
                  dest_file.write zis.read
              end
          end
      end

      convert_repy
    end

    def load_repy
      Parse::Repy.new(open(REPY_File) { |f| f.read }, &@status_report_proc)
    end

    def update_yaml
      _repy = load_repy
      open(YAML_File,"w") { |f| f.write YAML::dump(_repy.hash) }
      _repy.hash
    end

    def update_marshal
      _repy = load_repy
      open(MARSHAL_File,"w") { |f| f.write Marshal.dump(_repy.hash) }
      _repy.hash
    end

    def report(text,frac = 0)
      @status_report_proc.call(text,frac)
    end
  end
end
