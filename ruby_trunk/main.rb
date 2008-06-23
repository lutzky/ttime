#!/usr/bin/env ruby

require 'pathname'

p = Pathname.new($0)

if p.basename.to_s == 'main.rb'
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
a = TTime::GUI::MainWindow.instance
Gtk.main
