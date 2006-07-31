//
using System;
using Gtk;
using GtkSharp;
using Glade;

namespace UDonkey.GUI
{
	public class DBbrowser 
	{
		private Widget mMainWidget;
#region Glade Widgets
		//[Widget] Label label44;
#endregion

		public DBbrowser() 
		{
			Glade.XML gxml = new Glade.XML("udonkey.glade", "DBbrowser", null); 
			//Glade.XML gxml = Glade.XML.FromAssembly("udonkey.glade", "ConfigControl", null);
			gxml.Autoconnect (this);
			mMainWidget = gxml.GetWidget("DBbrowser");
		}

		public static implicit operator Widget(DBbrowser dbb)
		{
			return dbb.mMainWidget; 
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
			Widget w = new DBbrowser();
			win.Add(w);
			win.ShowAll();
			Application.Run();
		}
	}
}
