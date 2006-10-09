#!/usr/bin/env ruby
#
# This file is gererated by ruby-glade-create-template 1.1.3.
#
require 'libglade2'

class TtimeGlade
  include GetText

  attr :glade
  
  # Creates tooltips.
  def create_tooltips
    @tooltip = Gtk::Tooltips.new
    @glade['btNew'].set_tooltip(@tooltip, _('New File'))
    @glade['btOpen'].set_tooltip(@tooltip, _('Open File'))
    @glade['btSave'].set_tooltip(@tooltip, _('Save File'))
  end
  def initialize(path_or_data, root = nil, domain = nil, localedir = nil, flag = GladeXML::FILE)
    bindtextdomain(domain, localedir, nil, "UTF-8")
    @glade = GladeXML.new(path_or_data, root, domain, localedir, flag) {|handler| method(handler)}
    
    create_tooltips
  end
  
  def on_quit_activate(widget, arg0)
    puts "on_quit_activate() is not implemented yet."
  end
end

# Main program
if __FILE__ == $0
  # Set values as your own application. 
  PROG_PATH = "ttime.glade"
  PROG_NAME = "YOUR_APPLICATION_NAME"
  Gtk.init
  TtimeGlade.new(PROG_PATH, nil, PROG_NAME)
  Gtk.main
end
