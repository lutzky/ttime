#!/usr/bin/env ruby

require 'pathname'

ENV["LC_ALL"]="he_IL.UTF-8"
ENV["LANG"]="he_IL.UTF-8"
ENV["LANGUAGE"]="he_IL.UTF-8"

#hack for settings
ENV["HOME"] = Dir.pwd


p = Pathname.new($0)

if p.basename.to_s == 'main-win32.rb'
  Dir.chdir p.parent.to_s
end

require 'gui/main_window'

# Standard unicode support activation
require 'jcode'
$KCODE = 'u'

require 'gettext'

Thread.abort_on_exception = true

include GetText

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
