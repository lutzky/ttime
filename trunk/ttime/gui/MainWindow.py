import os
import sys
import gtk
import gtk.glade
import gobject

class MainWindow:
    def __init__(self):
        gladefile = os.path.join(os.path.dirname(os.path.realpath(__file__)),
            "ttime.glade")
        self.glade_xml = gtk.glade.XML(gladefile, None, "ttime")
        self.glade_xml.signal_autoconnect(self)
        notebook = self.glade_xml.get_widget('notebook')

    def on_quit_activate(self, *args):
        # FIXME: Save settings
        gtk.main_quit()

    def on_about_activate(self, *args):
        self.glade_xml.get_widget('AboutDialog').run()
        self.glade_xml.get_widget('AboutDialog').hide()

# FIXME: This is debug code, remove it
if __name__ == '__main__':
    mw = MainWindow()
    gtk.main()
