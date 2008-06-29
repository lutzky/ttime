#!/usr/bin/env ruby

require 'pathname'
require 'optparse'

p = Pathname.new($0)

if p.basename.to_s == 'main.rb'
  Dir.chdir p.parent.to_s
end

OptionParser.new do |opts|
  opts.on("--hebrew", "Force Hebrew locale") do |env|
    ENV["LC_ALL"]="he_IL.UTF-8"
    ENV["LANG"]="he_IL.UTF-8"
    ENV["LANGUAGE"]="he_IL.UTF-8"
  end

  opts.on("--win32-hack", "Hack for win32 compatibility") do |env|
    ENV["HOME"] = Dir.pwd
  end
end.parse!

require 'gui/main_window'

# Standard unicode support activation
require 'jcode'
$KCODE = 'u'

require 'gettext'

Thread.abort_on_exception = true

include GetText

Gtk.init
a = TTime::GUI::MainWindow.instance
Gtk.main
