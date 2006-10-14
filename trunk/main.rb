require 'gui/main_window'

require 'jcode'

$KCODE = 'u'

Thread::abort_on_exception = true

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
