@echo off
set RUBYOPT=rubygems
cd ruby-win\lib\GTK\bin
pango-querymodules > ..\etc\pango\pango.modules
gtk-query-immodules-2.0.exe > ..\etc\gtk-2.0\gtk.immodules
gdk-pixbuf-query-loaders > ..\etc\gtk-2.0\gdk-pixbuf.loaders
cd ..\..\..\..\
ruby-win\bin\ruby bin\ttime --win32-hack
