MainForm.exe: MainForm.cs DBbrowser.cs SearchControl.cs ../CustomEvents.cs 
	mcs -r:../../UDonkey.dll $^ -out:$@ -pkg:glade-sharp-2.0 /main:UDonkey.GUI.MainForm \
		$(patsubst %,-resource:%, $(wildcard ../*.bmp))

#%.exe: %.cs
#	mcs -r:../../UDonkey.dll $< -out:$@ -pkg:glade-sharp-2.0 


treeview: TreeViewExample.exe

test: DBbrowser.exe
	MONO_PATH=.. ./DBbrowser.exe
