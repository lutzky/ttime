using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for UsersEventForm.
	/// </summary>
	public class UsersEventForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TextBox EventNameTextBox;
        private System.Windows.Forms.NumericUpDown EventDurationNumericUpDown;
        private bool                         OKButtonPushed;
        private System.Windows.Forms.Button OKButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label EventNameLabel;
        private System.Windows.Forms.Label EventDurationLabel;
        private System.Windows.Forms.DomainUpDown EventDayDomainUpDown;
        private System.Windows.Forms.Label EventDayLabel;
        private System.Windows.Forms.NumericUpDown EventHourNumericUpDown;
        private System.Windows.Forms.Label EventHourLabel;
        private System.Windows.Forms.StatusBar StatusBar;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UsersEventForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.EventNameLabel = new System.Windows.Forms.Label();
			this.EventDurationLabel = new System.Windows.Forms.Label();
			this.EventNameTextBox = new System.Windows.Forms.TextBox();
			this.OKButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.EventDurationNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.EventDayDomainUpDown = new System.Windows.Forms.DomainUpDown();
			this.EventDayLabel = new System.Windows.Forms.Label();
			this.EventHourNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.EventHourLabel = new System.Windows.Forms.Label();
			this.StatusBar = new System.Windows.Forms.StatusBar();
			((System.ComponentModel.ISupportInitialize)(this.EventDurationNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.EventHourNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// EventNameLabel
			// 
			this.EventNameLabel.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventNameLabel.Location = new System.Drawing.Point(240, 13);
			this.EventNameLabel.Name = "EventNameLabel";
			this.EventNameLabel.Size = new System.Drawing.Size(90, 15);
			this.EventNameLabel.TabIndex = 0;
			this.EventNameLabel.Text = "שם האירוע:";
			// 
			// EventDurationLabel
			// 
			this.EventDurationLabel.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventDurationLabel.Location = new System.Drawing.Point(240, 104);
			this.EventDurationLabel.Name = "EventDurationLabel";
			this.EventDurationLabel.Size = new System.Drawing.Size(90, 15);
			this.EventDurationLabel.TabIndex = 1;
			this.EventDurationLabel.Text = "משך האירוע:";
			// 
			// EventNameTextBox
			// 
			this.EventNameTextBox.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventNameTextBox.Location = new System.Drawing.Point(30, 10);
			this.EventNameTextBox.MaxLength = 25;
			this.EventNameTextBox.Name = "EventNameTextBox";
			this.EventNameTextBox.Size = new System.Drawing.Size(200, 23);
			this.EventNameTextBox.TabIndex = 2;
			this.EventNameTextBox.Text = "שם האירוע";
			// 
			// OKButton
			// 
			this.OKButton.BackColor = System.Drawing.SystemColors.Info;
			this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKButton.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.OKButton.Location = new System.Drawing.Point(32, 96);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(70, 24);
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "אישור";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.BackColor = System.Drawing.SystemColors.Info;
			this.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CancelButton.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.CancelButton.Location = new System.Drawing.Point(32, 48);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(70, 24);
			this.CancelButton.TabIndex = 5;
			this.CancelButton.Text = "ביטול";
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// EventDurationNumericUpDown
			// 
			this.EventDurationNumericUpDown.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventDurationNumericUpDown.Location = new System.Drawing.Point(152, 100);
			this.EventDurationNumericUpDown.Maximum = new System.Decimal(new int[] {
																					   24,
																					   0,
																					   0,
																					   0});
			this.EventDurationNumericUpDown.Minimum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.EventDurationNumericUpDown.Name = "EventDurationNumericUpDown";
			this.EventDurationNumericUpDown.Size = new System.Drawing.Size(80, 23);
			this.EventDurationNumericUpDown.TabIndex = 6;
			this.EventDurationNumericUpDown.Value = new System.Decimal(new int[] {
																					 1,
																					 0,
																					 0,
																					 0});
			// 
			// EventDayDomainUpDown
			// 
			this.EventDayDomainUpDown.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.ראשון);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.שני);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.שלישי);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.רביעי);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.חמישי);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.שישי);
			this.EventDayDomainUpDown.Items.Add(UDonkey.DayOfWeek.שבת);
			this.EventDayDomainUpDown.Location = new System.Drawing.Point(152, 40);
			this.EventDayDomainUpDown.Name = "EventDayDomainUpDown";
			this.EventDayDomainUpDown.Size = new System.Drawing.Size(80, 23);
			this.EventDayDomainUpDown.TabIndex = 8;
			this.EventDayDomainUpDown.Text = "ראשון";
			// 
			// EventDayLabel
			// 
			this.EventDayLabel.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventDayLabel.Location = new System.Drawing.Point(240, 42);
			this.EventDayLabel.Name = "EventDayLabel";
			this.EventDayLabel.Size = new System.Drawing.Size(90, 15);
			this.EventDayLabel.TabIndex = 7;
			this.EventDayLabel.Text = "יום האירוע:";
			// 
			// EventHourNumericUpDown
			// 
			this.EventHourNumericUpDown.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventHourNumericUpDown.Location = new System.Drawing.Point(152, 70);
			this.EventHourNumericUpDown.Maximum = new System.Decimal(new int[] {
																				   23,
																				   0,
																				   0,
																				   0});
			this.EventHourNumericUpDown.Name = "EventHourNumericUpDown";
			this.EventHourNumericUpDown.Size = new System.Drawing.Size(80, 23);
			this.EventHourNumericUpDown.TabIndex = 10;
			this.EventHourNumericUpDown.Value = new System.Decimal(new int[] {
																				 1,
																				 0,
																				 0,
																				 0});
			this.EventHourNumericUpDown.ValueChanged += new System.EventHandler(this.EventHourNumericUpDown_ValueChanged);
			// 
			// EventHourLabel
			// 
			this.EventHourLabel.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.EventHourLabel.Location = new System.Drawing.Point(240, 72);
			this.EventHourLabel.Name = "EventHourLabel";
			this.EventHourLabel.Size = new System.Drawing.Size(90, 15);
			this.EventHourLabel.TabIndex = 9;
			this.EventHourLabel.Text = "שעת האירוע:";
			// 
			// StatusBar
			// 
			this.StatusBar.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.StatusBar.Location = new System.Drawing.Point(0, 130);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.StatusBar.Size = new System.Drawing.Size(338, 22);
			this.StatusBar.TabIndex = 11;
			this.StatusBar.Text = "באפשרותך לפתוח תפריט זה על ידי לחיצה ימנית על תא בטבלה";
			// 
			// UsersEventForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(338, 152);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.StatusBar,
																		  this.EventHourNumericUpDown,
																		  this.EventHourLabel,
																		  this.EventDayDomainUpDown,
																		  this.EventDayLabel,
																		  this.EventDurationNumericUpDown,
																		  this.CancelButton,
																		  this.OKButton,
																		  this.EventNameTextBox,
																		  this.EventDurationLabel,
																		  this.EventNameLabel});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UsersEventForm";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "אירוע משתמש";
			((System.ComponentModel.ISupportInitialize)(this.EventDurationNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.EventHourNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            OKButtonPushed = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

		private void EventHourNumericUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			
			Decimal newMax = new Decimal( 24 - (int)this.EventHourNumericUpDown.Value );
			if( this.EventDurationNumericUpDown.Value > newMax )
			{
				this.EventDurationNumericUpDown.Value = newMax;
			}
			this.EventDurationNumericUpDown.Maximum =  newMax;
		}
    
        public string EventName
        {
            get{ return EventNameTextBox.Text; }
        }
        public DayOfWeek EventDay
        {
            get{ return (DayOfWeek)EventDayDomainUpDown.SelectedItem;}
            set{ EventDayDomainUpDown.SelectedItem = value; }
        }
        public int EventHour
        {
            get{ return (int)EventHourNumericUpDown.Value; }
            set
			{ 
				this.EventDurationNumericUpDown.Maximum = new Decimal( 24 - value );
				EventHourNumericUpDown.Value = value; 
			}
        }
        public int EventDuration
        {
            get{ return (int)EventDurationNumericUpDown.Value; }
        }
        public bool OK
        {
            get{ return OKButtonPushed; }
        }
		#endregion
	}
}
