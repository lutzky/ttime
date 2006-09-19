// created on 24/07/2006 at 15:09
using System;
using Gtk;
using GtkSharp;
using Glade;

namespace UDonkey.Gtk
{
	public class AboutForm 
	{
		public AboutForm () 
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "AboutForm", null);
			gxml.Autoconnect (this);
		}
	}
}
