require 'gui/main_window'

require 'jcode'
require 'gettext'

$KCODE = 'u'

Thread.abort_on_exception = true

include GetText

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
