require 'libglade2'

class MainWindow
  def initialize
    @glade = GladeXML.new("ttime.glade", "MainWindow") { |handler| method(handler) }

    @main_window = @glade["MainWindow"]
  end

  def on_quit_activate
    p 'bye'
    Gtk.main_quit
  end
end

Gtk.init
MainWindow.new
Gtk.main
