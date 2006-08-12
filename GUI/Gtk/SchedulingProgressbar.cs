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
	public class SchedulingProgressbar 
	{
        private int maximum = 1;
#region Glade Widgets
		[Widget] Dialog mDialog;
		[Widget] ProgressBar progressbar;
		[Widget] Label countLabel;
#endregion

		public SchedulingProgressbar() 
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "SchedulingProgressbar", null); 
			gxml.Autoconnect (this);

			mDialog = (Dialog)gxml.GetWidget("SchedulingProgressbar");
		}

        public void Reset()
        {
            Progress(0);
        }

        public void SetMax(int max)
        {
            maximum = max;
            Progress(0);
        }

        public void Progress(int progress)
        {
            progressbar.Fraction = progress / (float)maximum;
            countLabel.Text = String.Format("בודק מערכת {0} מתוך {1}", progress, maximum);
        }

        public void Show()
        {
            mDialog.Show();
        }

        public void Close()
        {
            mDialog.Destroy();
        }


#region Event handlers
		private void on_close(object sender, EventArgs args)
		{
			//Application.Quit();
		}
		
		private void on_response(object sender, EventArgs args)
		{
			//Application.Quit();
		}
#endregion

#region Event 
#endregion
	}

}
