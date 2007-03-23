#!/usr/bin/env python

import os
import sys

import pygtk
pygtk.require('2.0')
import gtk

import ttime.gui.MainWindow

if __name__ == '__main__':
    main_window = ttime.gui.MainWindow.MainWindow()
    gtk.main()
