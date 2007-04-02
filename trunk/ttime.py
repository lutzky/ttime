#!/usr/bin/env python

import os
import sys

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

if __name__ == '__main__':
    parser = OptionParser("usage: %prog { -c }")
    parser.add_option("-c", "--cache", dest="usecache", help="Use cached REPY",
            default = False, action="store_true")
    (options,args) = parser.parse_args()
    prefs.options = options

    m = MainWindowStarter()
    print 'Letting all threads die...'
