require 'gui/main_window'

require 'jcode'

$KCODE = 'u'

Thread.abort_on_exception = true

include GetText
bindtextdomain("ttime", "locale", nil, "utf-8")

Gtk.init
a = TTime::GUI::MainWindow.new
Gtk.main
