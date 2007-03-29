#!/usr/bin/env python

import gtk.glade
import os
import sys
import locale
import gettext

local_path = os.path.join(os.path.realpath(os.path.dirname(sys.argv[0])),'locale')

langs = []
lc, encoding = locale.getdefaultlocale()
if (lc):
    langs = [lc]
language = os.environ.get('LANGUAGE', None)
if (language):
    langs += language.split(":")
langs += ["en_US", "en_US.UTF-8", "C", "he_IL.UTF-8", "he_IL"]

gettext.bindtextdomain("ttime", local_path)
gettext.bindtextdomain("ttime")
lang = gettext.translation("ttime", local_path, languages = langs, fallback = True)

gtk.glade.bindtextdomain("ttime", local_path)
gtk.glade.textdomain("ttime")
_ = lang.gettext
