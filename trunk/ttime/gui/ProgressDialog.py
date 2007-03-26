import gtk

# FIXME: Make this use gettext instead
_ = str

class ProgressDialog(gtk.Dialog):
    def __init__(self, stop_callback = None):
        gtk.Dialog.__init__(self)
        self.set_title(_('Progress'))
        self.set_modal(True)

        self.label = gtk.Label('Label')
        self.progressbar = gtk.ProgressBar()

        if stop_callback:
            btn_stop = gtk.Button(stock = 'gtk-stop')
            self.stop_callback = stop_callback
            btn_stop.connect('clicked', self.stop_clicked)
            self.connect('response', self.dlg_response)
            self.action_area.pack_end(btn_stop)

        self.vbox.pack_start(self.label)
        self.vbox.pack_end(self.progressbar)

        self.show_all()

    def stop_clicked(self, obj):
        self.stop_callback()

    def dlg_response(self, dialog, response_id):
        self.stop_callback()

    def get_status_func(self):
        def status_func(label = None,fraction = None):
            if label: self.label.set_label(label)
            if fraction: self.progressbar.set_fraction(fraction)
            else: self.progressbar.pulse()

        return status_func

# FIXME: Remove this
def bla():
    print 'click!'
if __name__ == '__main__':
    g = ProgressDialog(stop_callback = bla)
    d = g.get_status_func()
    d('bla',0.5)
    gtk.main()
