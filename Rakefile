require 'fileutils'
require 'tempfile'
require 'rake/testtask'

task :default => [:test]

VERSION_FILE = "lib/ttime/version.rb"

def write_version_file(version)
  open(VERSION_FILE, "w") do |f|
    f.puts <<-EOF
# DO NOT EDIT
# This file is auto-generated by build scripts.
# See: with_version
module TTime
  Version = %q{#{version}}
end
    EOF
  end
end

Rake::TestTask.new do |t|
  t.libs << "test"
  t.test_files = FileList['test/test*.rb']
  t.verbose = true
end

# Base path for SSH uploads (in scp syntax)
WebsiteSSHBasePath = "lutzky.net:public_html/ttime/"

desc "Write version number according to Debian changelog"
task :set_deb_version do
  version=`dpkg-parsechangelog | awk '/^Version:/ {print $2}'`.rstrip
  write_version_file version
end

desc "Write 'git' version number"
task :set_git_version do
  write_version_file '[git]'
end

desc "Create mo-files for L10n"
task :makemo do
  require 'gettext'
  require 'gettext/version'
  if GetText::VERSION >= "2.1.0"
    require 'gettext/tools'
    GetText.create_mofiles({:verbose => true,
                            :po_root => "po",
                            :targetdir => "data/locale"})
  else
    require 'gettext/utils'
    GetText.create_mofiles(true, "po", "data/locale")
  end
end

desc "Update pot/po files to match new version"
task :updatepo do
  require 'gettext/utils'
  GetText.update_pofiles("ttime",
                         Dir.glob("lib/**/*.rb") +
                         Dir.glob("data/ttime/*.ui") +
                         [ "bin/ttime" ],
                         "ttime 0.x.x")
end
