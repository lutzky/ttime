set RUBYOPT=rubygems
cd ruby\lib\GTK\bin
pango-querymodules > ..\etc\pango\pango.modules
gtk-query-immodules-2.0.exe > ..\etc\gtk-2.0\gtk.immodules
gdk-pixbuf-query-loaders > ..\etc\gtk-2.0\gdk-pixbuf.loaders
cd ..\..\..\..\ruby_trunk
del data\technion.mrshl
..\ruby\bin\ruby main.rb --hebrew --win32-hack
cd ..
