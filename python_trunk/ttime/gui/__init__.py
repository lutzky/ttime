from ttime.localize import _
import gtk
from ProgressDialog import ProgressDialog
from MainWindow import MainWindow

def warning(error_string):
    error_dlg = gtk.MessageDialog(
            type=gtk.MESSAGE_WARNING,
            buttons=gtk.BUTTONS_OK)
    error_dlg.set_markup(error_string)

    error_dlg.run()
    error_dlg.destroy()

def error(error_string):
    error_dlg = gtk.MessageDialog(
            type=gtk.MESSAGE_ERROR,
            buttons=gtk.BUTTONS_OK)
    error_dlg.set_markup(error_string)

    error_dlg.run()
    error_dlg.destroy()
    gtk.main_quit()
