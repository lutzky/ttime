#!/bin/bash

intltool-extract --type=gettext/glade ttime/gui/ttime.glade
xgettext -kN_ ttime.py ttime/gui/ttime.glade.h ttime/gui/*.py -o locale/ttime.pot
rm ttime/gui/ttime.glade.h
if [ ! -f locale/he_IL/he_IL.po ]; then
	msginit -l he_IL.UTF-8 -o locale/he_IL/he_IL.po -i locale/ttime.pot;
fi
intltool-update --dist --gettext-package=locale/ttime locale/he_IL/he_IL

msgfmt locale/he_IL/he_IL.po -o locale/he_IL/LC_MESSAGES/ttime.mo
