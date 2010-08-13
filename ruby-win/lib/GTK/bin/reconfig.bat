@echo off
set PATH=c:\ruby\lib\GTK\bin;%PATH%
c:\ruby\lib\GTK\bin\gdk-pixbuf-query-loaders.exe > c:\ruby\lib\GTK\etc\gtk-2.0\gdk-pixbuf.loaders
c:\ruby\lib\GTK\bin\gtk-query-immodules-2.0.exe > c:\ruby\lib\GTK\etc\gtk-2.0\gtk.immodules
c:\ruby\lib\GTK\bin\pango-querymodules.exe > c:\ruby\lib\GTK\etc\pango\pango.modules
