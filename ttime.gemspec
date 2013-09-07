require 'date'

dpkg = {}

IO.popen('dpkg-parsechangelog').each do |line|
  dpkg[:version] = line.split[1] if line.start_with?('Version:')
  if line.start_with?('Date:')
    dpkg[:date] = Date.parse(line.split[1..-1].join(' '))
  end
end

dpkg_description_started = false
description_arr = []
File.open('debian/control').each do |line|
  if dpkg_description_started
    if line.start_with?(' ')
      description_arr += [line[1..-1]]
    else
      dpkg_description_started = false
    end
  end

  if line.start_with?('Description:')
    dpkg[:summary] = line.split[1..-1].join(' ')
    dpkg_description_started = true
  end
end
dpkg[:description] = description_arr.join('\n')

Gem::Specification.new do |s|
  s.name         = 'ttime'
  s.version      = dpkg[:version]
  s.date         = dpkg[:date]
  s.summary      = dpkg[:summary]
  s.description  = dpkg[:description]
  s.authors      = ['Ohad Lutzky', 'Boaz Goldstein', 'Haggai Eran']
  s.email        = 'ohad@lutzky.net'
  s.homepage     = 'http://github.com/lutzky/ttime'
  s.files        = Dir.glob('{bin,data,lib}/**/*') + %w(README.rdoc)
  s.executables  = ['ttime']
  s.require_path = 'lib'

  s.add_runtime_dependency "gtk2",        [">= 1.1"]
  s.add_runtime_dependency "gettext",     [">= 2.2"]
  s.add_runtime_dependency "rsvg2",       [">= 1.1"]
  s.add_runtime_dependency "zip",         [">= 2.0"]
  s.add_runtime_dependency "libxml-ruby", [">= 2.3"]
  s.add_runtime_dependency "tzinfo",      [">= 0.3"]
end
