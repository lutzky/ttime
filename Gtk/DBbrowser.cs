//
using System;
using Gtk;
using GtkSharp;
using Glade;

namespace UDonkey.GUI
{
	public class DBbrowser : VPaned 
	{
#region Glade Widgets
		[Widget] Label label44;
#endregion

		public DBbrowser()
		{
			Glade.XML gxml = new Glade.XML("udonkey.glade", "DBbrowser", null); 
			//Glade.XML gxml = Glade.XML.FromAssembly("udonkey.glade", "ConfigControl", null);
			gxml.Autoconnect (this);
		}

#region Events
#endregion

#region Properties

#endregion
		
#region Events

#endregion
		public static void Main()
		{
			Application.Init();
			Window win = new Window("Hello World");
			DBbrowser dbb = new DBbrowser();
			win.Add(dbb);
			win.ShowAll();
			Console.WriteLine(dbb.label44.IsRealized);
			Application.Run();
		}
	}
}
