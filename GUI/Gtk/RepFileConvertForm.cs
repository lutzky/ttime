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

		public void Show()
		{
			Console.WriteLine("RepFileConvertForm.Show()");
			mDialog.ShowAll();
		}

#region Event handlers

		private void on_show(object sender, EventArgs args)
		{
			mDialog.ShowAll();
		}
		
		private void on_close(object sender, EventArgs args)
		{
			//Application.Quit();
		}
		
		private void on_response(object sender, EventArgs args)
		{
			//Application.Quit();
		}

		private void on_progress(object o, EventArgs args)
		{
			int prec = (int)o; 
			progressbar.Fraction = prec / 100.0;
			if (prec == 100)
				mDialog.Destroy();
		}

		private void RepToXML_Progress(int prec)
		{
			//Application.Invoke(prec, null, new EventHandler(on_progress));
		}

		private void RepToXML_StartConvertion(object sender, EventArgs args)
		{
			//Application.Invoke(new EventHandler(on_show));
		}
#endregion

#region Event 
#endregion
	}
	
}
