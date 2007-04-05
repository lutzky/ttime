#!/usr/bin/env python

import os
import sys

import pango
import pygtk
pygtk.require('2.0')
import gtk
import gobject

from optparse import OptionParser

from ttime.localize import _

from ttime import gui
from ttime import logic
from ttime import prefs

class MainWindowStarter:
    def start_main_window(self):
        logic.repy.parse_repy_data()

        print _("Data has been downloaded, starting main window")
        self.main_window = gui.MainWindow()

    def get_data(self):
        self.repy_data = logic.data.repy_data()

    def __init__(self):
        gui.ProgressDialog(_("Downloading REPY data"), self.get_data,
                           self.start_main_window)
	
        gtk.main()

def show_parsed_repy():
    w = gtk.Window()
    w.set_default_size(600,600)
    s = gtk.ScrolledWindow()
    tb = gtk.TextBuffer()
    tv = gtk.TextView(tb)
    tv.modify_font(pango.FontDescription ("Mono"))
    s.add(tv);
    s.set_policy(gtk.POLICY_NEVER, gtk.POLICY_ALWAYS)
    tb.set_text(logic.data.repy_data())
    w.add(s)
    w.show_all()
    w.connect('destroy',gtk.main_quit)
    gtk.main()

if __name__ == '__main__':
    parser = OptionParser("usage: %prog { -c }")
    parser.add_option("-c", "--cache", dest="usecache", help="Use cached REPY",
            default = False, action="store_true")
    parser.add_option("-p", "--parse", dest="do_parsing", help="Actually parse",
            default = False, action="store_true")
    parser.add_option("-v", "--view_repy", dest="viewrepy", help="Jusy show the REPY file",
            default = False, action="store_true")
    (options,args) = parser.parse_args()
    prefs.options = options

    if options.viewrepy:
        show_parsed_repy()
    else:
        m = MainWindowStarter()
    print 'Letting all threads die...'
