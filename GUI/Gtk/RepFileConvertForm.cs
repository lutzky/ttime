//
using System;
using System.Threading;
using GLib;
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
			Console.WriteLine("RepFileConvertForm.MDialog = " + mDialog.Handle);

			RepFile.RepToXML.StartConvertion += new EventHandler(RepToXML_StartConvertion);
			RepFile.RepToXML.Progress += new RepFile.RepToXML.ConvertProgress(RepToXML_Progress);
		}

		private string convertFileName, convertToFile;
		System.Threading.Thread mThread;
		public void Convert( string fileName, string toFile )
		{
			convertFileName = fileName;
			convertToFile = toFile;
			mThread = new System.Threading.Thread(new ThreadStart( this.DoConvert ) );
			mThread.Start();
			mDialog.ShowAll();
            Application.Run();
			mThread.Join();
		}

		private void DoConvert()
		{
			RepToXML.Convert(convertFileName, convertToFile);
		}

#region Event handlers
		private void on_progress(object obj, EventArgs args)
		{
			int prec = (args as ProgressArgs).prec; 
			Console.WriteLine("RepFileConvertForm on_progress("+prec+")");
			progressbar.Fraction = prec / 100.0;
			if (prec == 100)
			{
				mDialog.Destroy();
				Application.Quit();
			}
		}

		private void RepToXML_Progress(int prec)
		{
			Console.WriteLine("RepFileConvertForm Progress("+prec+")");
			Application.Invoke(this, new ProgressArgs(prec), new EventHandler(on_progress));
		}

		private void RepToXML_StartConvertion(object sender, EventArgs args)
		{
			//Application.Invoke(new EventHandler(on_show));
		}
		
		private void on_close(object sender, EventArgs args)
		{
			Application.Quit();
			mThread.Abort();
		}
		
		private void on_response(object sender, EventArgs args)
		{
			Application.Quit();
			mThread.Abort();
		}
#endregion

#region Event 
#endregion
		public class ProgressArgs : EventArgs
		{
			public ProgressArgs(int p) 
			{
				prec = p;
			}
			public int prec;
		}
	}

}
