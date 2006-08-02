DBbrowser.exe: DBbrowser.cs
	mcs -r:../UDonkey.dll $< -out:$@ -pkg:glade-sharp-2.0

test: DBbrowser.exe
	MONO_PATH=.. ./DBbrowser.exe
