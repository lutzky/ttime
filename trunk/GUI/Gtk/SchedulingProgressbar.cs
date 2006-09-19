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
	public class SchedulingProgressbar 
	{
        private int maximum = 1;
        private CoursesScheduler mScheduler;
#region Glade Widgets
		[Widget] Dialog mDialog;
		[Widget] ProgressBar progressbar;
		[Widget] Label countLabel;
#endregion

		public SchedulingProgressbar(CoursesScheduler scheduler)
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "SchedulingProgressbar", null); 
			gxml.Autoconnect (this);

			mDialog = (Dialog)gxml.GetWidget("SchedulingProgressbar");

            mScheduler = scheduler;
            mScheduler.StartScheduling += new SchedulingProgress( StartScheduling );
            mScheduler.ContinueScheduling += new SchedulingProgress( ContinueScheduling);
            mScheduler.EndScheduling += new SchedulingProgress( EndScheduling);
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
            Console.WriteLine("SchedulingProgressbar.Show()");
            mDialog.ShowAll();
        }

        public void Close()
        {
            mDialog.Destroy();
            Application.Quit();
        }

        public void CreateSchedules()
        {
          Reset();
          Show();
          Thread thread  = new Thread( new ThreadStart( this.mScheduler.CreateSchedules ) );
          thread.Start();
          Application.Run();
          thread.Join();
        }

#region Event handlers
		private void on_close(object sender, EventArgs args)
		{
			Close();
		}
		
		private void on_response(object sender, EventArgs args)
		{
            Close();
		}
        
        private void on_progress(object obj, EventArgs args)
        {
            int progress = (args as ProgressArgs).prec;
              progressCounter+=progress;
              if (progressCounter>5000)
              {
                Progress( progressCounter );
                progressCounter =0;
              }
        }

        private int progressCounter;
    private void StartScheduling( int progress )
    {
      progressCounter = 0;
      SetMax( progress );
    }
    private void ContinueScheduling( int progress )
    {
        Application.Invoke(this, new ProgressArgs(progress), new EventHandler(on_progress)); 
    }

    private void EndScheduling( int progress )
    {
        Application.Invoke(this, new EventArgs(), new EventHandler(on_close));
    }

#endregion

#region Event 
#endregion
	}

}
