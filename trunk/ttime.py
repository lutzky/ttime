#!/usr/bin/env python

import os
import sys

import pygtk
pygtk.require('2.0')
import gtk
import gobject
gobject.threads_init()
import threading

import ttime.logic.data
import ttime.gui.MainWindow
import ttime.gui.ProgressDialog

class MainWindowStarter:
    def start_main_window(self):
        print "Data has been downloaded, starting main window"
        self.main_window = ttime.gui.MainWindow.MainWindow()

    def get_data(self):
        self.repy_data = ttime.logic.data.repy_data()

    def __init__(self):
        progress = ttime.gui.ProgressDialog.ProgressDialog(
                'Loading REPY data', self.get_data,
                callback_func = self.start_main_window
                )

        gtk.main()

if __name__ == '__main__':
    m = MainWindowStarter()
