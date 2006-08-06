//
using System;
using System.Threading;
using Gtk;
using GtkSharp;
using Glade;

using UDonkey.Logic;
using UDonkey.RepFile;

namespace UDonkey.GUI
{
	public class RepFileConvertForm 
	{
#region Glade Widgets
		[Widget] Dialog mDialog;
		[Widget] ProgressBar progressbar;
#endregion

		public RepFileConvertForm() 
		{
			Console.WriteLine("RepFileConvertForm()");
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "RepFileConvertForm", null); 
			gxml.Autoconnect (this);

			mDialog = (Dialog)gxml.GetWidget("RepFileConvertForm");

			RepFile.RepToXML.StartConvertion += new EventHandler(RepToXML_StartConvertion);
			RepFile.RepToXML.Progress += new RepFile.RepToXML.ConvertProgress(RepToXML_Progress);
		}

		public void ShowNow()
		{
			Console.WriteLine("RepFileConvertForm.ShowNow()");
			mDialog.ShowAll();
			Application.Run();
		}

#region Event handlers

		private void RepToXML_Progress(int prec)
		{
			progressbar.Fraction = prec / 100.0;
			if (prec == 100)
				mDialog.Destroy();
		}

		private void RepToXML_StartConvertion(object sender, EventArgs args)
		{
			Thread thread = new Thread(new ThreadStart( this.ShowNow ) );
			thread.Start();
		}
#endregion

#region Event 
#endregion
	}
	
}
