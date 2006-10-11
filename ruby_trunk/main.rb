require 'gui/main_window'

require 'jcode'

$KCODE = 'u'

$DEBUG = false

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
