using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using UDonkey.Logic;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for SearchControl.
	/// </summary>
	public class SearchControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label6;
		
		private System.Windows.Forms.TextBox courseName;
		private System.Windows.Forms.ComboBox coursePoints;
		private System.Windows.Forms.CheckBox monday;
		private System.Windows.Forms.CheckBox wednesday;
		private System.Windows.Forms.CheckBox friday;
		private System.Windows.Forms.CheckBox sunday;
		private System.Windows.Forms.CheckBox thursday;
		private System.Windows.Forms.CheckBox tuesday;
		private System.Windows.Forms.TextBox lecturer;
		private System.Windows.Forms.Button search;
		private System.Windows.Forms.TextBox courseNumber;
		private System.Windows.Forms.ComboBox cfaculty;
		private System.Windows.Forms.TextBox textBox1;
	
		public delegate void SearchEventHandler(object sender, System.EventArgs e);
		private event SearchEventHandler mSearch;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SearchControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitializeComponent call
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cfaculty = new System.Windows.Forms.ComboBox();
			this.coursePoints = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.courseName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.courseNumber = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.monday = new System.Windows.Forms.CheckBox();
			this.wednesday = new System.Windows.Forms.CheckBox();
			this.friday = new System.Windows.Forms.CheckBox();
			this.sunday = new System.Windows.Forms.CheckBox();
			this.thursday = new System.Windows.Forms.CheckBox();
			this.tuesday = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lecturer = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.search = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cfaculty);
			this.groupBox1.Controls.Add(this.coursePoints);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.courseName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.courseNumber);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 128);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "פרטי קורס";
			// 
			// cfaculty
			// 
			this.cfaculty.Location = new System.Drawing.Point(8, 96);
			this.cfaculty.Name = "cfaculty";
			this.cfaculty.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cfaculty.Size = new System.Drawing.Size(168, 21);
			this.cfaculty.TabIndex = 8;
			this.cfaculty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// coursePoints
			// 
			this.coursePoints.AllowDrop = true;
			this.coursePoints.Items.AddRange(new object[] {
															  "",
															  "0.5",
															  "1",
															  "1.5",
															  "2",
															  "2.5",
															  "3",
															  "3.5",
															  "4",
															  "4.5",
															  "5",
															  "5.5",
															  "6",
															  "6.5",
															  "7"});
			this.coursePoints.Location = new System.Drawing.Point(8, 72);
			this.coursePoints.Name = "coursePoints";
			this.coursePoints.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.coursePoints.Size = new System.Drawing.Size(168, 21);
			this.coursePoints.TabIndex = 7;
			this.coursePoints.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Location = new System.Drawing.Point(184, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "פקולטה";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(184, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "מספר נקודות";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Location = new System.Drawing.Point(184, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "מספר הקורס";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// courseName
			// 
			this.courseName.Location = new System.Drawing.Point(8, 24);
			this.courseName.Name = "courseName";
			this.courseName.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.courseName.Size = new System.Drawing.Size(168, 20);
			this.courseName.TabIndex = 1;
			this.courseName.Text = "";
			this.courseName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.courseName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(184, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "שם הקורס";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// courseNumber
			// 
			this.courseNumber.Location = new System.Drawing.Point(8, 48);
			this.courseNumber.Name = "courseNumber";
			this.courseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.courseNumber.Size = new System.Drawing.Size(168, 20);
			this.courseNumber.TabIndex = 5;
			this.courseNumber.Text = "";
			this.courseNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.courseNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.textBox1.Size = new System.Drawing.Size(168, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// monday
			// 
			this.monday.Location = new System.Drawing.Point(184, 16);
			this.monday.Name = "monday";
			this.monday.Size = new System.Drawing.Size(32, 16);
			this.monday.TabIndex = 4;
			this.monday.Text = "ב";
			this.monday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// wednesday
			// 
			this.wednesday.Location = new System.Drawing.Point(104, 16);
			this.wednesday.Name = "wednesday";
			this.wednesday.Size = new System.Drawing.Size(32, 16);
			this.wednesday.TabIndex = 5;
			this.wednesday.Text = "ד";
			this.wednesday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// friday
			// 
			this.friday.Location = new System.Drawing.Point(24, 16);
			this.friday.Name = "friday";
			this.friday.Size = new System.Drawing.Size(32, 16);
			this.friday.TabIndex = 7;
			this.friday.Text = "ו";
			this.friday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// sunday
			// 
			this.sunday.Location = new System.Drawing.Point(224, 16);
			this.sunday.Name = "sunday";
			this.sunday.Size = new System.Drawing.Size(32, 16);
			this.sunday.TabIndex = 6;
			this.sunday.Text = "א";
			this.sunday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// thursday
			// 
			this.thursday.Location = new System.Drawing.Point(64, 16);
			this.thursday.Name = "thursday";
			this.thursday.Size = new System.Drawing.Size(32, 16);
			this.thursday.TabIndex = 9;
			this.thursday.Text = "ה";
			this.thursday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// tuesday
			// 
			this.tuesday.Location = new System.Drawing.Point(144, 16);
			this.tuesday.Name = "tuesday";
			this.tuesday.Size = new System.Drawing.Size(32, 16);
			this.tuesday.TabIndex = 8;
			this.tuesday.Text = "ג";
			this.tuesday.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.sunday);
			this.groupBox2.Controls.Add(this.tuesday);
			this.groupBox2.Controls.Add(this.thursday);
			this.groupBox2.Controls.Add(this.friday);
			this.groupBox2.Controls.Add(this.monday);
			this.groupBox2.Controls.Add(this.wednesday);
			this.groupBox2.Location = new System.Drawing.Point(0, 176);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 40);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "ימים";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.lecturer);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Location = new System.Drawing.Point(0, 128);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(272, 48);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "מרצה";
			// 
			// lecturer
			// 
			this.lecturer.Location = new System.Drawing.Point(8, 16);
			this.lecturer.Name = "lecturer";
			this.lecturer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lecturer.Size = new System.Drawing.Size(168, 20);
			this.lecturer.TabIndex = 3;
			this.lecturer.Text = "";
			this.lecturer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressEvent);
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label6.Location = new System.Drawing.Point(184, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 23);
			this.label6.TabIndex = 2;
			this.label6.Text = "שם המרצה";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// search
			// 
			this.search.Location = new System.Drawing.Point(192, 224);
			this.search.Name = "search";
			this.search.TabIndex = 12;
			this.search.Text = "בצע חיפוש";
			this.search.Click += new System.EventHandler(this.Search_Click);
			// 
			// SearchControl
			// 
			this.Controls.Add(this.search);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "SearchControl";
			this.Size = new System.Drawing.Size(272, 248);
			this.Load += new System.EventHandler(this.SearchControl_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private void SearchControl_Load(object sender, System.EventArgs e)
		{

		}

		private void Search_Click(object sender, System.EventArgs e)
		{	
			SearchEventArgs args = new SearchEventArgs();
			
			if ( courseName.Text != "")
				args.Name = courseName.Text;

			if ( courseNumber.Text != "")
				args.Number = courseNumber.Text;

			if ( coursePoints.Text != "")
				args.Points = coursePoints.Text;

			if ( cfaculty.Text != "")
				args.Faculty = cfaculty.Text;

			if ( lecturer.Text !="")
				args.Lecturer = lecturer.Text;

			if( sunday.Checked )
				args.Days[0]="א";
			
			if( monday.Checked )
				args.Days[1]="ב";
			
			if( tuesday.Checked )
				args.Days[2]="ג";
			
			if( wednesday.Checked )
				args.Days[3]="ד";
			
			if( thursday.Checked )
				args.Days[4]="ה";
			
			if( friday.Checked )
				args.Days[5]="ו";

			this.mSearch( this, args );
		}

		private void KeyPressEvent(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
			{
				search.PerformClick();
			}
		}
	
		public class SearchEventArgs: EventArgs
		{
			public string Name   = null;
			public string Number = null;
			public string Points = null;
			public string Faculty      = null;
			public string Lecturer     = null;
			public string[] Days = new string[6];
		}

		public event SearchEventHandler Search
		{
			add { mSearch += value; }
			remove { mSearch -= value; }
		}
	
		public ComboBox FacultiesComboBox
		{
			get{ return cfaculty; }
		}
	}
	
}

