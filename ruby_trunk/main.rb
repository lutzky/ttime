require 'gui/main_window'

require 'jcode'

$KCODE = 'u'

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
