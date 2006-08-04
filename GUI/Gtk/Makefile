%.exe: %.cs
	mcs -r:../UDonkey.dll $< -out:$@ -pkg:glade-sharp-2.0

treeview: TreeViewExample.exe

test: DBbrowser.exe
	MONO_PATH=.. ./DBbrowser.exe
