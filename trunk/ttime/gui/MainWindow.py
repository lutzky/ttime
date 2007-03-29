import os
import sys
import gtk
import gtk.glade
import gobject
import locale
import gettext

locale.setlocale(locale.LC_ALL, '')

local_path = os.path.realpath(os.path.dirname(sys.argv[0]))
langs = []
lc, encoding = locale.getdefaultlocale()
if (lc):
    langs = [lc]
language = os.environ.get('LANGUAGE', None)
if (language):
    langs += language.split(":")
langs += ["he_IL", "en_US"]

gettext.bindtextdomain("ttime", local_path)
gettext.bindtextdomain("ttime")
lang = gettext.translation("ttime", local_path, languages = langs, fallback = True)

gtk.glade.bindtextdomain("ttime", local_path)
gtk.glade.textdomain("ttime")
_ = lang.gettext


class MainWindow:
    def __init__(self):
        gladefile = os.path.join(os.path.dirname(os.path.realpath(__file__)),
            "ttime.glade")
        print _("_File")
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
