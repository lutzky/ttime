using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using UDonkey.Logic;

namespace UDonkey.GUI
{
    /// <summary>
    /// Summary description for RepFileConvertForm.
    /// </summary>
    public class SchedulingProgressbar : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label countTextLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label countTextlabel2;
        private System.Windows.Forms.Label countMaxlabel;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private CoursesScheduler mScheduler;

        public SchedulingProgressbar(CoursesScheduler scheduler)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.label1.Text = string.Format("{0}",
                "כעת מתבצעת יצירת המערכות");

            mScheduler = scheduler;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

		#region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.countTextLabel = new System.Windows.Forms.Label();
			this.countLabel = new System.Windows.Forms.Label();
			this.countTextlabel2 = new System.Windows.Forms.Label();
			this.countMaxlabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(8, 80);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(296, 24);
			this.progressBar1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 24);
			this.label1.TabIndex = 1;
			// 
			// countTextLabel
			// 
			this.countTextLabel.Location = new System.Drawing.Point(216, 47);
			this.countTextLabel.Name = "countTextLabel";
			this.countTextLabel.Size = new System.Drawing.Size(80, 20);
			this.countTextLabel.TabIndex = 2;
			this.countTextLabel.Text = "בודק מערכת מס\'";
			// 
			// countLabel
			// 
			this.countLabel.Location = new System.Drawing.Point(136, 48);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(75, 20);
			this.countLabel.TabIndex = 3;
			// 
			// countTextlabel2
			// 
			this.countTextlabel2.Location = new System.Drawing.Point(96, 47);
			this.countTextlabel2.Name = "countTextlabel2";
			this.countTextlabel2.Size = new System.Drawing.Size(30, 20);
			this.countTextlabel2.TabIndex = 4;
			this.countTextlabel2.Text = "מתוך";
			// 
			// countMaxlabel
			// 
			this.countMaxlabel.Location = new System.Drawing.Point(16, 47);
			this.countMaxlabel.Name = "countMaxlabel";
			this.countMaxlabel.Size = new System.Drawing.Size(75, 20);
			this.countMaxlabel.TabIndex = 5;
			// 
			// SchedulingProgressbar
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 123);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.countMaxlabel,
																		  this.countTextlabel2,
																		  this.countLabel,
																		  this.countTextLabel,
																		  this.label1,
																		  this.progressBar1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(320, 150);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(320, 150);
			this.Name = "SchedulingProgressbar";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "יצירת מערכות";
			this.ResumeLayout(false);

		}
		#endregion
        public void Reset( )
        {
            this.progressBar1.Value = 0;
        }
        public void SetMax( int max )
        {
            this.countLabel.Visible      = true;
            this.countMaxlabel.Visible   = true;
            this.countTextLabel.Visible  = true;
            this.countTextlabel2.Visible = true;
            this.countLabel.Text         = (0).ToString();
            this.countMaxlabel.Text      = max.ToString();
            this.progressBar1.Maximum = max;
            this.Refresh();
        }
        public void Progress( int progress )
        {
            this.progressBar1.Value += progress;
            this.countLabel.Text    = this.progressBar1.Value.ToString();
            this.Refresh();
        }
        public void CreateSchedules()
        {
          Reset();
          Show();
          Thread thread  = new Thread( new ThreadStart( this.mScheduler.CreateSchedules ) );
          thread.Start();
          Application.Run();
          thread.Join();
          Close();
        }
        private int progressCounter;
    private void StartScheduling( int progress )
    {
      progressCounter = 0;
      SetMax( progress );
    }
    private void ContinueScheduling( int progress )
    {
      progressCounter+=progress;
      if (progressCounter>5000)
      {
        Progress( progressCounter );
        progressCounter =0;
      }

    }

    private void EndScheduling( int progress )
    {
      Close();

    }
    }
}
