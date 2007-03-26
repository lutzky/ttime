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

class DataThread(threading.Thread):
    def __init__(self, callback):
        threading.Thread.__init__(self)
        self.callback = callback
        self.proceed = True

    def abort(self):
        self.proceed = False

    def run(self):
        str = ttime.logic.data.repy_data()
        if self.proceed: self.callback(str)

# FIXME: This is EXTREMELY messy :(
if __name__ == '__main__':
    def start_main_window(throw_away_string):
        print "Data has been downloaded, starting main window"
        stat_flag = False
        progress.hide()
        m = ttime.gui.MainWindow.MainWindow()
    def stat_true():
        stat()
        return stat_flag
    stat_flag = True
    d = DataThread(start_main_window)
    d.start()
    progress = ttime.gui.ProgressDialog.ProgressDialog()
    stat = progress.get_status_func()
    stat('Downloading REPY data...')
    gobject.timeout_add(100,stat_true)

    gtk.main()
