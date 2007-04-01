#!/usr/bin/env python

import os
import sys

import pygtk
pygtk.require('2.0')
import gtk
import gobject

from ttime.localize import _

from ttime import gui
from ttime import logic

class MainWindowStarter:
    def start_main_window(self):
        print _("Data has been downloaded, starting main window")
        self.main_window = gui.MainWindow()

    def get_data(self):
        self.repy_data = logic.data.repy_data()

    def __init__(self):
        gui.ProgressDialog(_("Downloading REPY data"), self.get_data,
                           self.start_main_window)
	
        gtk.main()

if __name__ == '__main__':
    m = MainWindowStarter()
    print 'Letting all threads die...'
