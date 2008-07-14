require 'rake/rdoctask'

class File
  class << self
    def try_unlink(*files)
      files.each do |file_name|
        begin
          File.unlink file_name
        rescue Errno::ENOENT
        end
      end
    end
  end
end

Rake::RDocTask.new("doc") do |rdoc|
  rdoc.rdoc_dir = "doc"
  rdoc.title = "TTime -- A Technion Timetable utility"
  rdoc.main = "README.rdoc"
  rdoc.rdoc_files.include('README.rdoc')
  rdoc.rdoc_files.include('lib/**/*.rb')
end

desc "Find all FIXME comments"
task :fixme do
  puts `grep -r FIXME * | grep -v '.git'`
end

desc "Create mo-files for L10n"
task :makemo do
  require 'gettext/utils'
  GetText.create_mofiles(true, "po", "data/locale")
end

desc "Update pot/po files to match new version"
task :updatepo do
  require 'gettext/utils'
  GetText.update_pofiles("ttime",
                         Dir.glob("lib/**/*.rb") +
                         Dir.glob("data/ttime/*.glade") +
                         [ "bin/ttime" ],
                         "ttime 0.x.x")
end

desc "Build a debian package"
task :debuild => [ :makemo ] do
  `debuild`
end

desc "Build a debian package without signing"
task :debuild_nosign => [ :makemo ] do
  `debuild -uc -us`
end

desc "Zip up relevant windows package files (without Ruby)"
task :winbuild => [ :makemo ] do
  `zip -r ttime_win.zip ttime_win.bat bin data lib README.rdoc`
end
