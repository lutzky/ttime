#!/usr/bin/env python

"""Provides an interface to run a long operation in a background worker
thread, while displaying an appropriate graphical hint"""

import pygtk
pygtk.require('2.0')
import gtk
import gobject
gobject.threads_init()
import threading

# FIXME: Make this use gettext instead
_ = str

# FIXME: "Stop" button only calls the cancel callback, currently no good
#        way to actually kill off a thread

class ProgressDialogWorker(threading.Thread):
    def __init__(self, worker_func, callback_func):
        threading.Thread.__init__(self)
        self.worker_func = worker_func
        self.callback_func = callback_func

    def run(self):
        self.worker_func()
        self.callback_func()

class ProgressDialog(gtk.Dialog):
    def __init__(self, label, worker_func, callback_func = None,
            cancel_func = None):
        """worker_func is run in a background thread. If cancel_func is
        provided, then a 'stop' button is displayed, and cancel_func is
        called if it is pressed or the window is closed. When the worker
        thread finishes, the dialog is hidden, and if the stop button was
        not pressed, callback_func - if provided - is called."""
        # FIXME: The window should not be closable if cancel_func is not
        # given
        gtk.Dialog.__init__(self)
        self.set_title(label)
        self.set_modal(True)

        self.callback_func = callback_func

        worker = ProgressDialogWorker(worker_func, self.worker_func_done)
        worker.start()

        self.cancelled = False
        self.done = False

        self.label = gtk.Label(label)
        self.progressbar = gtk.ProgressBar()

        gobject.timeout_add(100, self.pulse)

        if cancel_func:
            btn_stop = gtk.Button(stock = 'gtk-stop')
            self.cancel_func = cancel_func
            btn_stop.connect('clicked', self.stop_clicked)
            self.connect('response', self.dlg_response)
            self.action_area.pack_end(btn_stop)

        self.vbox.pack_start(self.label)
        self.vbox.pack_end(self.progressbar)

        self.show_all()

    def pulse(self):
        self.progressbar.pulse()
        return not self.done and not self.cancelled

    def worker_func_done(self):
        self.done = True
        if not self.cancelled:
            self.hide()
            if self.callback_func: self.callback_func()

    def stop_clicked(self, obj):
        if self.cancel_func:
            self.cancelled = True
            self.cancel_func()

    def dlg_response(self, dialog, response_id):
        if self.cancel_func:
            self.cancelled = True
            self.cancel_func()
