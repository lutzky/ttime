#!/usr/bin/env ruby

require 'gtk2'

$LOAD_PATH.unshift File::dirname($0)

require 'tcal'

window = Gtk::Window.new

window.signal_connect("delete-event") do
  Gtk.main_quit
  true
end

area = TCal::Calendar.new()
area.set_size_request(500,380)
area.add_event("מערכות סיפרתיות\nתרגול\nטאוב 2",2,10.5,3.5,1)
area.add_event("מערכות סיפרתיות\nתרגול\nטאוב 2",5,12.0,2.5,1)
area.add_event("לוגיקה\nהרצאה\n אולמן 404",5,14.0,2.5,2)
area.add_event("לוגיקה\nהרצאה\n אולמן 404",5,13.0,2.5,2)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      5,10.5 ,2.0,3)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      5,11.5,1.5,3)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      5,9.0 ,3.0,4)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      1,9.0 ,3.0,4)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      3,9.0 ,3.0,5)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      4,9.0 ,3.0,5)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      4,14.5 ,3.0,5)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      4,14.5 ,3.0,6)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      3,14.5 ,3.0,7)
area.add_event("מת\"מ\nהרצאה\nטאוב 2",      2,14.5 ,3.0,8)


window.add(area)
window.show_all


Gtk.main
