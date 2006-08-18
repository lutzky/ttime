using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for ConfigControl.
	/// </summary>
	public class ConfigControl : System.Windows.Forms.UserControl
	{
		private const string RESOURCES_GROUP = "Configuration"; 
		private System.Windows.Forms.Label lblMainTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbMinTestDays;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TrackBar trackBar5;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TrackBar trackBar6;
		private System.Windows.Forms.TrackBar trackBar7;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TrackBar trackBar8;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TrackBar tbHoles;
		private System.Windows.Forms.TrackBar tbFreeDays;
		private System.Windows.Forms.TrackBar tbStartDay;
		private System.Windows.Forms.TrackBar tbEndDay;
		private System.Windows.Forms.Button btAnt;
		private System.Windows.Forms.Button btPadlaa;
		private System.Windows.Forms.Button btDontCare;
		private System.Windows.Forms.Button btSaveChanges;
		private System.Windows.Forms.CheckBox chbCollisions;
		private System.Windows.Forms.TextBox tbMaxCollisions;
		private System.Windows.Forms.CheckBox chbRegGroups;
		private System.Windows.Forms.CheckBox chbTestAlert;
		private System.Windows.Forms.CheckBox chbSunday;
		private System.Windows.Forms.CheckBox chbMonday;
		private System.Windows.Forms.CheckBox chbWednesday;
		private System.Windows.Forms.CheckBox chbTuesday;
		private System.Windows.Forms.CheckBox chbSaturday;
		private System.Windows.Forms.CheckBox chbFriday;
		private System.Windows.Forms.CheckBox chbThursday;
		private System.Windows.Forms.ComboBox cbStartHour;
		private System.Windows.Forms.ComboBox cbEndHour;
		private System.Windows.Forms.GroupBox grpDisplayOptions;
		private System.Windows.Forms.GroupBox grpGeneral;
		private System.Windows.Forms.Label lblMaxCollisions;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chbExportTeacher;
		private System.Windows.Forms.CheckBox chbExportLocation;
		private System.Windows.Forms.CheckBox chbExportRegNum;
		private System.Windows.Forms.CheckBox chbExportType;
		private System.Windows.Forms.CheckBox chbExportName;
		private System.Windows.Forms.CheckBox chbExportNumber;
		private System.Windows.Forms.GroupBox grpConstraints;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label lblSaveResult;
		private System.Windows.Forms.TextBox tbMinFreeDays;
		private System.Windows.Forms.ComboBox cbSundayEnd;
		private System.Windows.Forms.ComboBox cbSundayStart;
		private System.Windows.Forms.CheckBox chbFreeSunday;
		private System.Windows.Forms.CheckBox chbFreeMonday;
		private System.Windows.Forms.ComboBox cbMondayEnd;
		private System.Windows.Forms.ComboBox cbMondayStart;
		private System.Windows.Forms.CheckBox chbFreeTuesday;
		private System.Windows.Forms.ComboBox cbTuesdayEnd;
		private System.Windows.Forms.ComboBox cbTuesdayStart;
		private System.Windows.Forms.CheckBox chbFreeWednesday;
		private System.Windows.Forms.ComboBox cbWednesdayEnd;
		private System.Windows.Forms.ComboBox cbWednesdayStart;
		private System.Windows.Forms.CheckBox chbFreeThursday;
		private System.Windows.Forms.ComboBox cbThursdayEnd;
		private System.Windows.Forms.ComboBox cbThursdayStart;
		private System.Windows.Forms.TextBox tbMinDailyHours;
		private System.Windows.Forms.TextBox tbMaxDailyHours;
		private System.Windows.Forms.GroupBox grpPref;
		private System.Windows.Forms.Label lblThursdayEnd;
		private System.Windows.Forms.Label lblThursdayStart;
		private System.Windows.Forms.Label lblWednesdayEnd;
		private System.Windows.Forms.Label lblWednesdayStart;
		private System.Windows.Forms.Label lblTuesdayEnd;
		private System.Windows.Forms.Label lblTuesdayStart;
		private System.Windows.Forms.Label lblMondayEnd;
		private System.Windows.Forms.Label lblMondayStart;
		private System.Windows.Forms.Label lblSundayEnd;
		private System.Windows.Forms.Label lblSundayStart;
		private System.Windows.Forms.Button btDefaults;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConfigControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.btDefaults_Click(this, new System.EventArgs());
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
			this.lblMainTitle = new System.Windows.Forms.Label();
			this.chbCollisions = new System.Windows.Forms.CheckBox();
			this.tbMaxCollisions = new System.Windows.Forms.TextBox();
			this.lblMaxCollisions = new System.Windows.Forms.Label();
			this.chbRegGroups = new System.Windows.Forms.CheckBox();
			this.chbTestAlert = new System.Windows.Forms.CheckBox();
			this.tbMinTestDays = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chbSunday = new System.Windows.Forms.CheckBox();
			this.chbMonday = new System.Windows.Forms.CheckBox();
			this.chbWednesday = new System.Windows.Forms.CheckBox();
			this.chbTuesday = new System.Windows.Forms.CheckBox();
			this.chbSaturday = new System.Windows.Forms.CheckBox();
			this.chbFriday = new System.Windows.Forms.CheckBox();
			this.chbThursday = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbStartHour = new System.Windows.Forms.ComboBox();
			this.cbEndHour = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbHoles = new System.Windows.Forms.TrackBar();
			this.tbFreeDays = new System.Windows.Forms.TrackBar();
			this.tbStartDay = new System.Windows.Forms.TrackBar();
			this.tbEndDay = new System.Windows.Forms.TrackBar();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.grpPref = new System.Windows.Forms.GroupBox();
			this.btAnt = new System.Windows.Forms.Button();
			this.btPadlaa = new System.Windows.Forms.Button();
			this.btDontCare = new System.Windows.Forms.Button();
			this.label17 = new System.Windows.Forms.Label();
			this.trackBar5 = new System.Windows.Forms.TrackBar();
			this.label18 = new System.Windows.Forms.Label();
			this.trackBar6 = new System.Windows.Forms.TrackBar();
			this.trackBar7 = new System.Windows.Forms.TrackBar();
			this.label19 = new System.Windows.Forms.Label();
			this.trackBar8 = new System.Windows.Forms.TrackBar();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.grpDisplayOptions = new System.Windows.Forms.GroupBox();
			this.grpGeneral = new System.Windows.Forms.GroupBox();
			this.btSaveChanges = new System.Windows.Forms.Button();
			this.label28 = new System.Windows.Forms.Label();
			this.chbExportTeacher = new System.Windows.Forms.CheckBox();
			this.chbExportLocation = new System.Windows.Forms.CheckBox();
			this.chbExportRegNum = new System.Windows.Forms.CheckBox();
			this.chbExportType = new System.Windows.Forms.CheckBox();
			this.chbExportNumber = new System.Windows.Forms.CheckBox();
			this.chbExportName = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grpConstraints = new System.Windows.Forms.GroupBox();
			this.label39 = new System.Windows.Forms.Label();
			this.tbMaxDailyHours = new System.Windows.Forms.TextBox();
			this.label38 = new System.Windows.Forms.Label();
			this.tbMinDailyHours = new System.Windows.Forms.TextBox();
			this.chbFreeThursday = new System.Windows.Forms.CheckBox();
			this.cbThursdayEnd = new System.Windows.Forms.ComboBox();
			this.lblThursdayEnd = new System.Windows.Forms.Label();
			this.cbThursdayStart = new System.Windows.Forms.ComboBox();
			this.lblThursdayStart = new System.Windows.Forms.Label();
			this.chbFreeWednesday = new System.Windows.Forms.CheckBox();
			this.cbWednesdayEnd = new System.Windows.Forms.ComboBox();
			this.lblWednesdayEnd = new System.Windows.Forms.Label();
			this.cbWednesdayStart = new System.Windows.Forms.ComboBox();
			this.lblWednesdayStart = new System.Windows.Forms.Label();
			this.chbFreeTuesday = new System.Windows.Forms.CheckBox();
			this.cbTuesdayEnd = new System.Windows.Forms.ComboBox();
			this.lblTuesdayEnd = new System.Windows.Forms.Label();
			this.cbTuesdayStart = new System.Windows.Forms.ComboBox();
			this.lblTuesdayStart = new System.Windows.Forms.Label();
			this.chbFreeMonday = new System.Windows.Forms.CheckBox();
			this.cbMondayEnd = new System.Windows.Forms.ComboBox();
			this.lblMondayEnd = new System.Windows.Forms.Label();
			this.cbMondayStart = new System.Windows.Forms.ComboBox();
			this.lblMondayStart = new System.Windows.Forms.Label();
			this.chbFreeSunday = new System.Windows.Forms.CheckBox();
			this.cbSundayEnd = new System.Windows.Forms.ComboBox();
			this.lblSundayEnd = new System.Windows.Forms.Label();
			this.cbSundayStart = new System.Windows.Forms.ComboBox();
			this.lblSundayStart = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbMinFreeDays = new System.Windows.Forms.TextBox();
			this.lblSaveResult = new System.Windows.Forms.Label();
			this.btDefaults = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.tbHoles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbFreeDays)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbStartDay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbEndDay)).BeginInit();
			this.grpPref.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar8)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.grpConstraints.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMainTitle
			// 
			this.lblMainTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblMainTitle.Font = new System.Drawing.Font("David", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblMainTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblMainTitle.Location = new System.Drawing.Point(0, 0);
			this.lblMainTitle.Name = "lblMainTitle";
			this.lblMainTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblMainTitle.Size = new System.Drawing.Size(904, 32);
			this.lblMainTitle.TabIndex = 0;
			this.lblMainTitle.Text = "הגדרות מערכת";
			this.lblMainTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// chbCollisions
			// 
			this.chbCollisions.Location = new System.Drawing.Point(704, 48);
			this.chbCollisions.Name = "chbCollisions";
			this.chbCollisions.Size = new System.Drawing.Size(184, 24);
			this.chbCollisions.TabIndex = 1;
			this.chbCollisions.Text = "אפשר התנגשויות במערכת";
			this.chbCollisions.CheckedChanged += new System.EventHandler(this.chbCollisions_CheckedChanged);
			// 
			// tbMaxCollisions
			// 
			this.tbMaxCollisions.Enabled = false;
			this.tbMaxCollisions.Location = new System.Drawing.Point(632, 48);
			this.tbMaxCollisions.Name = "tbMaxCollisions";
			this.tbMaxCollisions.Size = new System.Drawing.Size(32, 20);
			this.tbMaxCollisions.TabIndex = 2;
			this.tbMaxCollisions.Text = "1";
			this.tbMaxCollisions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblMaxCollisions
			// 
			this.lblMaxCollisions.Enabled = false;
			this.lblMaxCollisions.Location = new System.Drawing.Point(664, 48);
			this.lblMaxCollisions.Name = "lblMaxCollisions";
			this.lblMaxCollisions.Size = new System.Drawing.Size(64, 32);
			this.lblMaxCollisions.TabIndex = 3;
			this.lblMaxCollisions.Text = "מקסימום התנגשויות:";
			// 
			// chbRegGroups
			// 
			this.chbRegGroups.Checked = true;
			this.chbRegGroups.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbRegGroups.Location = new System.Drawing.Point(648, 72);
			this.chbRegGroups.Name = "chbRegGroups";
			this.chbRegGroups.Size = new System.Drawing.Size(240, 24);
			this.chbRegGroups.TabIndex = 4;
			this.chbRegGroups.Text = "אפשר שיבוץ ארועים מקבוצות רישום שונות";
			// 
			// chbTestAlert
			// 
			this.chbTestAlert.Checked = true;
			this.chbTestAlert.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbTestAlert.Location = new System.Drawing.Point(672, 96);
			this.chbTestAlert.Name = "chbTestAlert";
			this.chbTestAlert.Size = new System.Drawing.Size(216, 16);
			this.chbTestAlert.TabIndex = 5;
			this.chbTestAlert.Text = "התרע אם מועדי א\' ברווח של פחות מ";
			this.chbTestAlert.CheckedChanged += new System.EventHandler(this.chbTestAlert_CheckedChanged);
			// 
			// tbMinTestDays
			// 
			this.tbMinTestDays.Location = new System.Drawing.Point(656, 96);
			this.tbMinTestDays.Name = "tbMinTestDays";
			this.tbMinTestDays.Size = new System.Drawing.Size(24, 20);
			this.tbMinTestDays.TabIndex = 6;
			this.tbMinTestDays.Text = "1";
			this.tbMinTestDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(616, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "ימים";
			// 
			// chbSunday
			// 
			this.chbSunday.Checked = true;
			this.chbSunday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbSunday.Location = new System.Drawing.Point(848, 160);
			this.chbSunday.Name = "chbSunday";
			this.chbSunday.Size = new System.Drawing.Size(40, 24);
			this.chbSunday.TabIndex = 8;
			this.chbSunday.Text = "א\'";
			// 
			// chbMonday
			// 
			this.chbMonday.Checked = true;
			this.chbMonday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbMonday.Location = new System.Drawing.Point(848, 176);
			this.chbMonday.Name = "chbMonday";
			this.chbMonday.Size = new System.Drawing.Size(40, 24);
			this.chbMonday.TabIndex = 9;
			this.chbMonday.Text = "ב\'";
			// 
			// chbWednesday
			// 
			this.chbWednesday.Checked = true;
			this.chbWednesday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbWednesday.Location = new System.Drawing.Point(800, 160);
			this.chbWednesday.Name = "chbWednesday";
			this.chbWednesday.Size = new System.Drawing.Size(40, 24);
			this.chbWednesday.TabIndex = 11;
			this.chbWednesday.Text = "ד\'";
			// 
			// chbTuesday
			// 
			this.chbTuesday.Checked = true;
			this.chbTuesday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbTuesday.Location = new System.Drawing.Point(848, 192);
			this.chbTuesday.Name = "chbTuesday";
			this.chbTuesday.Size = new System.Drawing.Size(40, 24);
			this.chbTuesday.TabIndex = 10;
			this.chbTuesday.Text = "ג\'";
			// 
			// chbSaturday
			// 
			this.chbSaturday.Location = new System.Drawing.Point(752, 192);
			this.chbSaturday.Name = "chbSaturday";
			this.chbSaturday.Size = new System.Drawing.Size(48, 24);
			this.chbSaturday.TabIndex = 14;
			this.chbSaturday.Text = "שבת";
			// 
			// chbFriday
			// 
			this.chbFriday.Location = new System.Drawing.Point(800, 192);
			this.chbFriday.Name = "chbFriday";
			this.chbFriday.Size = new System.Drawing.Size(40, 24);
			this.chbFriday.TabIndex = 13;
			this.chbFriday.Text = "ו\'";
			// 
			// chbThursday
			// 
			this.chbThursday.Checked = true;
			this.chbThursday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbThursday.Location = new System.Drawing.Point(800, 176);
			this.chbThursday.Name = "chbThursday";
			this.chbThursday.Size = new System.Drawing.Size(40, 24);
			this.chbThursday.TabIndex = 12;
			this.chbThursday.Text = "ה\'";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(760, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 15;
			this.label3.Text = "ימים להכללה במערכת:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(696, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 17;
			this.label4.Text = "שעת התחלה";
			// 
			// cbStartHour
			// 
			this.cbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStartHour.Items.AddRange(new object[] {
															 "0:30",
															 "1:30",
															 "2:30",
															 "3:30",
															 "4:30",
															 "5:30",
															 "6:30",
															 "7:30",
															 "8:30",
															 "9:30",
															 "10:30",
															 "11:30",
															 "12:30",
															 "13:30",
															 "14:30",
															 "15:30",
															 "16:30",
															 "17:30",
															 "18:30",
															 "19:30",
															 "20:30",
															 "21:30",
															 "22:30",
															 "23:30"});
			this.cbStartHour.Location = new System.Drawing.Point(608, 144);
			this.cbStartHour.Name = "cbStartHour";
			this.cbStartHour.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbStartHour.Size = new System.Drawing.Size(88, 21);
			this.cbStartHour.TabIndex = 18;
			// 
			// cbEndHour
			// 
			this.cbEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEndHour.Items.AddRange(new object[] {
														   "0:30",
														   "1:30",
														   "2:30",
														   "3:30",
														   "4:30",
														   "5:30",
														   "6:30",
														   "7:30",
														   "8:30",
														   "9:30",
														   "10:30",
														   "11:30",
														   "12:30",
														   "13:30",
														   "14:30",
														   "15:30",
														   "16:30",
														   "17:30",
														   "18:30",
														   "19:30",
														   "20:30",
														   "21:30",
														   "22:30",
														   "23:30"});
			this.cbEndHour.Location = new System.Drawing.Point(608, 168);
			this.cbEndHour.Name = "cbEndHour";
			this.cbEndHour.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbEndHour.Size = new System.Drawing.Size(88, 21);
			this.cbEndHour.TabIndex = 20;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(696, 168);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 19;
			this.label5.Text = "שעת סיום";
			// 
			// tbHoles
			// 
			this.tbHoles.Cursor = System.Windows.Forms.Cursors.Hand;
			this.tbHoles.LargeChange = 1;
			this.tbHoles.Location = new System.Drawing.Point(304, 88);
			this.tbHoles.Maximum = 5;
			this.tbHoles.Minimum = -5;
			this.tbHoles.Name = "tbHoles";
			this.tbHoles.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbHoles.Size = new System.Drawing.Size(42, 184);
			this.tbHoles.TabIndex = 21;
			this.tbHoles.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// tbFreeDays
			// 
			this.tbFreeDays.Cursor = System.Windows.Forms.Cursors.Hand;
			this.tbFreeDays.LargeChange = 1;
			this.tbFreeDays.Location = new System.Drawing.Point(376, 88);
			this.tbFreeDays.Maximum = 5;
			this.tbFreeDays.Minimum = -5;
			this.tbFreeDays.Name = "tbFreeDays";
			this.tbFreeDays.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbFreeDays.Size = new System.Drawing.Size(42, 184);
			this.tbFreeDays.TabIndex = 22;
			this.tbFreeDays.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// tbStartDay
			// 
			this.tbStartDay.Cursor = System.Windows.Forms.Cursors.Hand;
			this.tbStartDay.LargeChange = 1;
			this.tbStartDay.Location = new System.Drawing.Point(544, 88);
			this.tbStartDay.Maximum = 5;
			this.tbStartDay.Minimum = -5;
			this.tbStartDay.Name = "tbStartDay";
			this.tbStartDay.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbStartDay.Size = new System.Drawing.Size(42, 184);
			this.tbStartDay.TabIndex = 24;
			this.tbStartDay.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// tbEndDay
			// 
			this.tbEndDay.Cursor = System.Windows.Forms.Cursors.Hand;
			this.tbEndDay.LargeChange = 1;
			this.tbEndDay.Location = new System.Drawing.Point(472, 88);
			this.tbEndDay.Maximum = 5;
			this.tbEndDay.Minimum = -5;
			this.tbEndDay.Name = "tbEndDay";
			this.tbEndDay.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbEndDay.Size = new System.Drawing.Size(42, 184);
			this.tbEndDay.TabIndex = 23;
			this.tbEndDay.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(304, 272);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 32);
			this.label6.TabIndex = 25;
			this.label6.Text = "מינימום חורים";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(304, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 32);
			this.label7.TabIndex = 26;
			this.label7.Text = "מקסימום חורים";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label8.Location = new System.Drawing.Point(416, 176);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 27;
			this.label8.Text = "לא חשוב";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label9.Location = new System.Drawing.Point(424, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(32, 24);
			this.label9.TabIndex = 28;
			this.label9.Text = "חשוב מאוד";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label10.Location = new System.Drawing.Point(424, 248);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(32, 24);
			this.label10.TabIndex = 29;
			this.label10.Text = "חשוב מאוד";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(368, 272);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 40);
			this.label11.TabIndex = 30;
			this.label11.Text = "מינימום ימים חופשיים";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(368, 48);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(56, 40);
			this.label12.TabIndex = 31;
			this.label12.Text = "מקסימום ימים חופשיים";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label13.Location = new System.Drawing.Point(168, 240);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 24);
			this.label13.TabIndex = 33;
			this.label13.Text = "סיום מאוחר";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label14.Location = new System.Drawing.Point(176, 24);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(48, 32);
			this.label14.TabIndex = 32;
			this.label14.Text = "סיום מוקדם";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(536, 48);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(48, 40);
			this.label15.TabIndex = 35;
			this.label15.Text = "שעת התחלה מאוחרת";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(536, 272);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(48, 32);
			this.label16.TabIndex = 34;
			this.label16.Text = "התחלה מוקדמת";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpPref
			// 
			this.grpPref.Controls.Add(this.label13);
			this.grpPref.Controls.Add(this.btAnt);
			this.grpPref.Controls.Add(this.btPadlaa);
			this.grpPref.Controls.Add(this.btDontCare);
			this.grpPref.Controls.Add(this.label14);
			this.grpPref.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.grpPref.Location = new System.Drawing.Point(296, 32);
			this.grpPref.Name = "grpPref";
			this.grpPref.Size = new System.Drawing.Size(304, 312);
			this.grpPref.TabIndex = 36;
			this.grpPref.TabStop = false;
			this.grpPref.Text = "העדפות סידור מערכת";
			// 
			// btAnt
			// 
			this.btAnt.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btAnt.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btAnt.Location = new System.Drawing.Point(208, 280);
			this.btAnt.Name = "btAnt";
			this.btAnt.TabIndex = 0;
			this.btAnt.Text = "מצב נמלה";
			this.btAnt.Click += new System.EventHandler(this.btAnt_Click);
			// 
			// btPadlaa
			// 
			this.btPadlaa.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btPadlaa.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btPadlaa.Location = new System.Drawing.Point(16, 280);
			this.btPadlaa.Name = "btPadlaa";
			this.btPadlaa.TabIndex = 39;
			this.btPadlaa.Text = "מצב פדלאה";
			this.btPadlaa.Click += new System.EventHandler(this.btPadlaa_Click);
			// 
			// btDontCare
			// 
			this.btDontCare.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btDontCare.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btDontCare.Location = new System.Drawing.Point(112, 280);
			this.btDontCare.Name = "btDontCare";
			this.btDontCare.TabIndex = 40;
			this.btDontCare.Text = "מצב אדיש";
			this.btDontCare.Click += new System.EventHandler(this.btDontCare_Click);
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label17.Location = new System.Drawing.Point(432, 232);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(32, 24);
			this.label17.TabIndex = 29;
			this.label17.Text = "חשוב מאוד";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar5
			// 
			this.trackBar5.Location = new System.Drawing.Point(544, 72);
			this.trackBar5.Maximum = 5;
			this.trackBar5.Minimum = -5;
			this.trackBar5.Name = "trackBar5";
			this.trackBar5.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBar5.Size = new System.Drawing.Size(42, 184);
			this.trackBar5.TabIndex = 24;
			this.trackBar5.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(536, 256);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(48, 32);
			this.label18.TabIndex = 34;
			this.label18.Text = "התחלה מוקדם";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar6
			// 
			this.trackBar6.Location = new System.Drawing.Point(472, 72);
			this.trackBar6.Maximum = 5;
			this.trackBar6.Minimum = -5;
			this.trackBar6.Name = "trackBar6";
			this.trackBar6.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBar6.Size = new System.Drawing.Size(42, 184);
			this.trackBar6.TabIndex = 23;
			this.trackBar6.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// trackBar7
			// 
			this.trackBar7.Location = new System.Drawing.Point(376, 72);
			this.trackBar7.Maximum = 5;
			this.trackBar7.Minimum = -5;
			this.trackBar7.Name = "trackBar7";
			this.trackBar7.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBar7.Size = new System.Drawing.Size(42, 184);
			this.trackBar7.TabIndex = 22;
			this.trackBar7.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(368, 32);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(56, 40);
			this.label19.TabIndex = 31;
			this.label19.Text = "מקסימום ימים חופשיים";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trackBar8
			// 
			this.trackBar8.Location = new System.Drawing.Point(304, 72);
			this.trackBar8.Maximum = 5;
			this.trackBar8.Minimum = -5;
			this.trackBar8.Name = "trackBar8";
			this.trackBar8.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBar8.Size = new System.Drawing.Size(42, 184);
			this.trackBar8.TabIndex = 21;
			this.trackBar8.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(464, 256);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(48, 32);
			this.label20.TabIndex = 32;
			this.label20.Text = "סיום מוקדם";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label21.Location = new System.Drawing.Point(416, 160);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 16);
			this.label21.TabIndex = 27;
			this.label21.Text = "לא משנה";
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(536, 40);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(56, 32);
			this.label22.TabIndex = 35;
			this.label22.Text = "התחלה מאוחר";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(296, 40);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(56, 32);
			this.label23.TabIndex = 26;
			this.label23.Text = "מקסימום חורים";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(368, 256);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(48, 40);
			this.label24.TabIndex = 30;
			this.label24.Text = "מינימום ימים חופשיים";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(464, 40);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(56, 32);
			this.label25.TabIndex = 33;
			this.label25.Text = "סיום מאוחר";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("David", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label26.Location = new System.Drawing.Point(424, 80);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(32, 24);
			this.label26.TabIndex = 28;
			this.label26.Text = "חשוב מאוד";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpDisplayOptions
			// 
			this.grpDisplayOptions.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.grpDisplayOptions.Location = new System.Drawing.Point(600, 120);
			this.grpDisplayOptions.Name = "grpDisplayOptions";
			this.grpDisplayOptions.Size = new System.Drawing.Size(296, 104);
			this.grpDisplayOptions.TabIndex = 37;
			this.grpDisplayOptions.TabStop = false;
			this.grpDisplayOptions.Text = "אפשרויות תצוגה";
			// 
			// grpGeneral
			// 
			this.grpGeneral.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.grpGeneral.Location = new System.Drawing.Point(600, 32);
			this.grpGeneral.Name = "grpGeneral";
			this.grpGeneral.Size = new System.Drawing.Size(296, 88);
			this.grpGeneral.TabIndex = 38;
			this.grpGeneral.TabStop = false;
			this.grpGeneral.Text = "אפשרויות כלליות";
			// 
			// btSaveChanges
			// 
			this.btSaveChanges.BackColor = System.Drawing.Color.Goldenrod;
			this.btSaveChanges.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btSaveChanges.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btSaveChanges.ForeColor = System.Drawing.Color.White;
			this.btSaveChanges.Location = new System.Drawing.Point(496, 352);
			this.btSaveChanges.Name = "btSaveChanges";
			this.btSaveChanges.Size = new System.Drawing.Size(96, 23);
			this.btSaveChanges.TabIndex = 39;
			this.btSaveChanges.Text = "שמור שינויים";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("David", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label28.Location = new System.Drawing.Point(712, 256);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(176, 16);
			this.label28.TabIndex = 47;
			this.label28.Text = "שדות להכללה במערכת המודפסת:";
			// 
			// chbExportTeacher
			// 
			this.chbExportTeacher.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbExportTeacher.Location = new System.Drawing.Point(120, 120);
			this.chbExportTeacher.Name = "chbExportTeacher";
			this.chbExportTeacher.Size = new System.Drawing.Size(168, 24);
			this.chbExportTeacher.TabIndex = 45;
			this.chbExportTeacher.Text = "שם המרצה/מתרגל";
			// 
			// chbExportLocation
			// 
			this.chbExportLocation.Checked = true;
			this.chbExportLocation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbExportLocation.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbExportLocation.Location = new System.Drawing.Point(128, 104);
			this.chbExportLocation.Name = "chbExportLocation";
			this.chbExportLocation.Size = new System.Drawing.Size(160, 24);
			this.chbExportLocation.TabIndex = 44;
			this.chbExportLocation.Text = "מיקום (בנין וחדר)";
			// 
			// chbExportRegNum
			// 
			this.chbExportRegNum.Checked = true;
			this.chbExportRegNum.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbExportRegNum.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbExportRegNum.Location = new System.Drawing.Point(200, 88);
			this.chbExportRegNum.Name = "chbExportRegNum";
			this.chbExportRegNum.Size = new System.Drawing.Size(88, 24);
			this.chbExportRegNum.TabIndex = 43;
			this.chbExportRegNum.Text = "מספר קבוצה";
			// 
			// chbExportType
			// 
			this.chbExportType.Checked = true;
			this.chbExportType.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbExportType.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbExportType.Location = new System.Drawing.Point(112, 72);
			this.chbExportType.Name = "chbExportType";
			this.chbExportType.Size = new System.Drawing.Size(176, 24);
			this.chbExportType.TabIndex = 42;
			this.chbExportType.Text = "סוג הארוע (הרצאה, תרגול...)";
			// 
			// chbExportNumber
			// 
			this.chbExportNumber.Checked = true;
			this.chbExportNumber.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbExportNumber.Location = new System.Drawing.Point(776, 288);
			this.chbExportNumber.Name = "chbExportNumber";
			this.chbExportNumber.Size = new System.Drawing.Size(112, 24);
			this.chbExportNumber.TabIndex = 41;
			this.chbExportNumber.Text = "מספר הקורס";
			// 
			// chbExportName
			// 
			this.chbExportName.Checked = true;
			this.chbExportName.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbExportName.Location = new System.Drawing.Point(776, 272);
			this.chbExportName.Name = "chbExportName";
			this.chbExportName.Size = new System.Drawing.Size(112, 24);
			this.chbExportName.TabIndex = 40;
			this.chbExportName.Text = "שם הקורס";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chbExportTeacher);
			this.groupBox1.Controls.Add(this.chbExportLocation);
			this.groupBox1.Controls.Add(this.chbExportRegNum);
			this.groupBox1.Controls.Add(this.chbExportType);
			this.groupBox1.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.groupBox1.Location = new System.Drawing.Point(600, 232);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(296, 152);
			this.groupBox1.TabIndex = 52;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "אפשרויות יצוא";
			// 
			// grpConstraints
			// 
			this.grpConstraints.Controls.Add(this.label39);
			this.grpConstraints.Controls.Add(this.tbMaxDailyHours);
			this.grpConstraints.Controls.Add(this.label38);
			this.grpConstraints.Controls.Add(this.tbMinDailyHours);
			this.grpConstraints.Controls.Add(this.chbFreeThursday);
			this.grpConstraints.Controls.Add(this.cbThursdayEnd);
			this.grpConstraints.Controls.Add(this.lblThursdayEnd);
			this.grpConstraints.Controls.Add(this.cbThursdayStart);
			this.grpConstraints.Controls.Add(this.lblThursdayStart);
			this.grpConstraints.Controls.Add(this.chbFreeWednesday);
			this.grpConstraints.Controls.Add(this.cbWednesdayEnd);
			this.grpConstraints.Controls.Add(this.lblWednesdayEnd);
			this.grpConstraints.Controls.Add(this.cbWednesdayStart);
			this.grpConstraints.Controls.Add(this.lblWednesdayStart);
			this.grpConstraints.Controls.Add(this.chbFreeTuesday);
			this.grpConstraints.Controls.Add(this.cbTuesdayEnd);
			this.grpConstraints.Controls.Add(this.lblTuesdayEnd);
			this.grpConstraints.Controls.Add(this.cbTuesdayStart);
			this.grpConstraints.Controls.Add(this.lblTuesdayStart);
			this.grpConstraints.Controls.Add(this.chbFreeMonday);
			this.grpConstraints.Controls.Add(this.cbMondayEnd);
			this.grpConstraints.Controls.Add(this.lblMondayEnd);
			this.grpConstraints.Controls.Add(this.cbMondayStart);
			this.grpConstraints.Controls.Add(this.lblMondayStart);
			this.grpConstraints.Controls.Add(this.chbFreeSunday);
			this.grpConstraints.Controls.Add(this.cbSundayEnd);
			this.grpConstraints.Controls.Add(this.lblSundayEnd);
			this.grpConstraints.Controls.Add(this.cbSundayStart);
			this.grpConstraints.Controls.Add(this.lblSundayStart);
			this.grpConstraints.Controls.Add(this.label1);
			this.grpConstraints.Controls.Add(this.tbMinFreeDays);
			this.grpConstraints.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.grpConstraints.Location = new System.Drawing.Point(8, 32);
			this.grpConstraints.Name = "grpConstraints";
			this.grpConstraints.Size = new System.Drawing.Size(288, 352);
			this.grpConstraints.TabIndex = 53;
			this.grpConstraints.TabStop = false;
			this.grpConstraints.Text = "אילוצים בבנית מערכות";
			// 
			// label39
			// 
			this.label39.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label39.Location = new System.Drawing.Point(72, 80);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(208, 16);
			this.label39.TabIndex = 49;
			this.label39.Text = "מקסימום שעות לימוד ביום שאינו חופשי:";
			// 
			// tbMaxDailyHours
			// 
			this.tbMaxDailyHours.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.tbMaxDailyHours.Location = new System.Drawing.Point(48, 80);
			this.tbMaxDailyHours.Name = "tbMaxDailyHours";
			this.tbMaxDailyHours.Size = new System.Drawing.Size(24, 20);
			this.tbMaxDailyHours.TabIndex = 48;
			this.tbMaxDailyHours.Text = "0";
			this.tbMaxDailyHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label38
			// 
			this.label38.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label38.Location = new System.Drawing.Point(72, 56);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(208, 16);
			this.label38.TabIndex = 47;
			this.label38.Text = "מינימום שעות לימוד ביום שאינו חופשי:";
			// 
			// tbMinDailyHours
			// 
			this.tbMinDailyHours.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.tbMinDailyHours.Location = new System.Drawing.Point(48, 56);
			this.tbMinDailyHours.Name = "tbMinDailyHours";
			this.tbMinDailyHours.Size = new System.Drawing.Size(24, 20);
			this.tbMinDailyHours.TabIndex = 46;
			this.tbMinDailyHours.Text = "0";
			this.tbMinDailyHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// chbFreeThursday
			// 
			this.chbFreeThursday.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbFreeThursday.Location = new System.Drawing.Point(176, 304);
			this.chbFreeThursday.Name = "chbFreeThursday";
			this.chbFreeThursday.TabIndex = 45;
			this.chbFreeThursday.Text = "יום ה\' חופשי";
			this.chbFreeThursday.CheckedChanged += new System.EventHandler(this.chbFreeThursday_CheckedChanged);
			// 
			// cbThursdayEnd
			// 
			this.cbThursdayEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbThursdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbThursdayEnd.Items.AddRange(new object[] {
															   "0:30",
															   "1:30",
															   "2:30",
															   "3:30",
															   "4:30",
															   "5:30",
															   "6:30",
															   "7:30",
															   "8:30",
															   "9:30",
															   "10:30",
															   "11:30",
															   "12:30",
															   "13:30",
															   "14:30",
															   "15:30",
															   "16:30",
															   "17:30",
															   "18:30",
															   "19:30",
															   "20:30",
															   "21:30",
															   "22:30",
															   "23:30"});
			this.cbThursdayEnd.Location = new System.Drawing.Point(8, 320);
			this.cbThursdayEnd.Name = "cbThursdayEnd";
			this.cbThursdayEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbThursdayEnd.Size = new System.Drawing.Size(88, 20);
			this.cbThursdayEnd.TabIndex = 44;
			// 
			// lblThursdayEnd
			// 
			this.lblThursdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblThursdayEnd.Location = new System.Drawing.Point(96, 320);
			this.lblThursdayEnd.Name = "lblThursdayEnd";
			this.lblThursdayEnd.Size = new System.Drawing.Size(72, 16);
			this.lblThursdayEnd.TabIndex = 43;
			this.lblThursdayEnd.Text = "שעת סיום";
			// 
			// cbThursdayStart
			// 
			this.cbThursdayStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbThursdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbThursdayStart.Items.AddRange(new object[] {
																 "0:30",
																 "1:30",
																 "2:30",
																 "3:30",
																 "4:30",
																 "5:30",
																 "6:30",
																 "7:30",
																 "8:30",
																 "9:30",
																 "10:30",
																 "11:30",
																 "12:30",
																 "13:30",
																 "14:30",
																 "15:30",
																 "16:30",
																 "17:30",
																 "18:30",
																 "19:30",
																 "20:30",
																 "21:30",
																 "22:30",
																 "23:30"});
			this.cbThursdayStart.Location = new System.Drawing.Point(8, 296);
			this.cbThursdayStart.Name = "cbThursdayStart";
			this.cbThursdayStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbThursdayStart.Size = new System.Drawing.Size(88, 20);
			this.cbThursdayStart.TabIndex = 42;
			// 
			// lblThursdayStart
			// 
			this.lblThursdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblThursdayStart.Location = new System.Drawing.Point(96, 296);
			this.lblThursdayStart.Name = "lblThursdayStart";
			this.lblThursdayStart.Size = new System.Drawing.Size(72, 16);
			this.lblThursdayStart.TabIndex = 41;
			this.lblThursdayStart.Text = "שעת התחלה";
			// 
			// chbFreeWednesday
			// 
			this.chbFreeWednesday.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbFreeWednesday.Location = new System.Drawing.Point(176, 256);
			this.chbFreeWednesday.Name = "chbFreeWednesday";
			this.chbFreeWednesday.TabIndex = 40;
			this.chbFreeWednesday.Text = "יום ד\' חופשי";
			this.chbFreeWednesday.CheckedChanged += new System.EventHandler(this.chbFreeWednesday_CheckedChanged);
			// 
			// cbWednesdayEnd
			// 
			this.cbWednesdayEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbWednesdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbWednesdayEnd.Items.AddRange(new object[] {
																"0:30",
																"1:30",
																"2:30",
																"3:30",
																"4:30",
																"5:30",
																"6:30",
																"7:30",
																"8:30",
																"9:30",
																"10:30",
																"11:30",
																"12:30",
																"13:30",
																"14:30",
																"15:30",
																"16:30",
																"17:30",
																"18:30",
																"19:30",
																"20:30",
																"21:30",
																"22:30",
																"23:30"});
			this.cbWednesdayEnd.Location = new System.Drawing.Point(8, 272);
			this.cbWednesdayEnd.Name = "cbWednesdayEnd";
			this.cbWednesdayEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbWednesdayEnd.Size = new System.Drawing.Size(88, 20);
			this.cbWednesdayEnd.TabIndex = 39;
			// 
			// lblWednesdayEnd
			// 
			this.lblWednesdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblWednesdayEnd.Location = new System.Drawing.Point(96, 272);
			this.lblWednesdayEnd.Name = "lblWednesdayEnd";
			this.lblWednesdayEnd.Size = new System.Drawing.Size(72, 16);
			this.lblWednesdayEnd.TabIndex = 38;
			this.lblWednesdayEnd.Text = "שעת סיום";
			// 
			// cbWednesdayStart
			// 
			this.cbWednesdayStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbWednesdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbWednesdayStart.Items.AddRange(new object[] {
																  "0:30",
																  "1:30",
																  "2:30",
																  "3:30",
																  "4:30",
																  "5:30",
																  "6:30",
																  "7:30",
																  "8:30",
																  "9:30",
																  "10:30",
																  "11:30",
																  "12:30",
																  "13:30",
																  "14:30",
																  "15:30",
																  "16:30",
																  "17:30",
																  "18:30",
																  "19:30",
																  "20:30",
																  "21:30",
																  "22:30",
																  "23:30"});
			this.cbWednesdayStart.Location = new System.Drawing.Point(8, 248);
			this.cbWednesdayStart.Name = "cbWednesdayStart";
			this.cbWednesdayStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbWednesdayStart.Size = new System.Drawing.Size(88, 20);
			this.cbWednesdayStart.TabIndex = 37;
			// 
			// lblWednesdayStart
			// 
			this.lblWednesdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblWednesdayStart.Location = new System.Drawing.Point(96, 248);
			this.lblWednesdayStart.Name = "lblWednesdayStart";
			this.lblWednesdayStart.Size = new System.Drawing.Size(72, 16);
			this.lblWednesdayStart.TabIndex = 36;
			this.lblWednesdayStart.Text = "שעת התחלה";
			// 
			// chbFreeTuesday
			// 
			this.chbFreeTuesday.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbFreeTuesday.Location = new System.Drawing.Point(176, 208);
			this.chbFreeTuesday.Name = "chbFreeTuesday";
			this.chbFreeTuesday.TabIndex = 35;
			this.chbFreeTuesday.Text = "יום ג\' חופשי";
			this.chbFreeTuesday.CheckedChanged += new System.EventHandler(this.chbFreeTuesday_CheckedChanged);
			// 
			// cbTuesdayEnd
			// 
			this.cbTuesdayEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTuesdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbTuesdayEnd.Items.AddRange(new object[] {
															  "0:30",
															  "1:30",
															  "2:30",
															  "3:30",
															  "4:30",
															  "5:30",
															  "6:30",
															  "7:30",
															  "8:30",
															  "9:30",
															  "10:30",
															  "11:30",
															  "12:30",
															  "13:30",
															  "14:30",
															  "15:30",
															  "16:30",
															  "17:30",
															  "18:30",
															  "19:30",
															  "20:30",
															  "21:30",
															  "22:30",
															  "23:30"});
			this.cbTuesdayEnd.Location = new System.Drawing.Point(8, 224);
			this.cbTuesdayEnd.Name = "cbTuesdayEnd";
			this.cbTuesdayEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbTuesdayEnd.Size = new System.Drawing.Size(88, 20);
			this.cbTuesdayEnd.TabIndex = 34;
			// 
			// lblTuesdayEnd
			// 
			this.lblTuesdayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblTuesdayEnd.Location = new System.Drawing.Point(96, 224);
			this.lblTuesdayEnd.Name = "lblTuesdayEnd";
			this.lblTuesdayEnd.Size = new System.Drawing.Size(72, 16);
			this.lblTuesdayEnd.TabIndex = 33;
			this.lblTuesdayEnd.Text = "שעת סיום";
			// 
			// cbTuesdayStart
			// 
			this.cbTuesdayStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTuesdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbTuesdayStart.Items.AddRange(new object[] {
																"0:30",
																"1:30",
																"2:30",
																"3:30",
																"4:30",
																"5:30",
																"6:30",
																"7:30",
																"8:30",
																"9:30",
																"10:30",
																"11:30",
																"12:30",
																"13:30",
																"14:30",
																"15:30",
																"16:30",
																"17:30",
																"18:30",
																"19:30",
																"20:30",
																"21:30",
																"22:30",
																"23:30"});
			this.cbTuesdayStart.Location = new System.Drawing.Point(8, 200);
			this.cbTuesdayStart.Name = "cbTuesdayStart";
			this.cbTuesdayStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbTuesdayStart.Size = new System.Drawing.Size(88, 20);
			this.cbTuesdayStart.TabIndex = 32;
			// 
			// lblTuesdayStart
			// 
			this.lblTuesdayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblTuesdayStart.Location = new System.Drawing.Point(96, 200);
			this.lblTuesdayStart.Name = "lblTuesdayStart";
			this.lblTuesdayStart.Size = new System.Drawing.Size(72, 16);
			this.lblTuesdayStart.TabIndex = 31;
			this.lblTuesdayStart.Text = "שעת התחלה";
			// 
			// chbFreeMonday
			// 
			this.chbFreeMonday.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbFreeMonday.Location = new System.Drawing.Point(176, 160);
			this.chbFreeMonday.Name = "chbFreeMonday";
			this.chbFreeMonday.TabIndex = 30;
			this.chbFreeMonday.Text = "יום ב\' חופשי";
			this.chbFreeMonday.CheckedChanged += new System.EventHandler(this.chbFreeMonday_CheckedChanged);
			// 
			// cbMondayEnd
			// 
			this.cbMondayEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMondayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbMondayEnd.Items.AddRange(new object[] {
															 "0:30",
															 "1:30",
															 "2:30",
															 "3:30",
															 "4:30",
															 "5:30",
															 "6:30",
															 "7:30",
															 "8:30",
															 "9:30",
															 "10:30",
															 "11:30",
															 "12:30",
															 "13:30",
															 "14:30",
															 "15:30",
															 "16:30",
															 "17:30",
															 "18:30",
															 "19:30",
															 "20:30",
															 "21:30",
															 "22:30",
															 "23:30"});
			this.cbMondayEnd.Location = new System.Drawing.Point(8, 176);
			this.cbMondayEnd.Name = "cbMondayEnd";
			this.cbMondayEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbMondayEnd.Size = new System.Drawing.Size(88, 20);
			this.cbMondayEnd.TabIndex = 29;
			// 
			// lblMondayEnd
			// 
			this.lblMondayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblMondayEnd.Location = new System.Drawing.Point(96, 176);
			this.lblMondayEnd.Name = "lblMondayEnd";
			this.lblMondayEnd.Size = new System.Drawing.Size(72, 16);
			this.lblMondayEnd.TabIndex = 28;
			this.lblMondayEnd.Text = "שעת סיום";
			// 
			// cbMondayStart
			// 
			this.cbMondayStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMondayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbMondayStart.Items.AddRange(new object[] {
															   "0:30",
															   "1:30",
															   "2:30",
															   "3:30",
															   "4:30",
															   "5:30",
															   "6:30",
															   "7:30",
															   "8:30",
															   "9:30",
															   "10:30",
															   "11:30",
															   "12:30",
															   "13:30",
															   "14:30",
															   "15:30",
															   "16:30",
															   "17:30",
															   "18:30",
															   "19:30",
															   "20:30",
															   "21:30",
															   "22:30",
															   "23:30"});
			this.cbMondayStart.Location = new System.Drawing.Point(8, 152);
			this.cbMondayStart.Name = "cbMondayStart";
			this.cbMondayStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbMondayStart.Size = new System.Drawing.Size(88, 20);
			this.cbMondayStart.TabIndex = 27;
			// 
			// lblMondayStart
			// 
			this.lblMondayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblMondayStart.Location = new System.Drawing.Point(96, 152);
			this.lblMondayStart.Name = "lblMondayStart";
			this.lblMondayStart.Size = new System.Drawing.Size(72, 16);
			this.lblMondayStart.TabIndex = 26;
			this.lblMondayStart.Text = "שעת התחלה";
			// 
			// chbFreeSunday
			// 
			this.chbFreeSunday.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.chbFreeSunday.Location = new System.Drawing.Point(176, 112);
			this.chbFreeSunday.Name = "chbFreeSunday";
			this.chbFreeSunday.TabIndex = 25;
			this.chbFreeSunday.Text = "יום א\' חופשי";
			this.chbFreeSunday.CheckedChanged += new System.EventHandler(this.chbFreeSunday_CheckedChanged);
			// 
			// cbSundayEnd
			// 
			this.cbSundayEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSundayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbSundayEnd.Items.AddRange(new object[] {
															 "0:30",
															 "1:30",
															 "2:30",
															 "3:30",
															 "4:30",
															 "5:30",
															 "6:30",
															 "7:30",
															 "8:30",
															 "9:30",
															 "10:30",
															 "11:30",
															 "12:30",
															 "13:30",
															 "14:30",
															 "15:30",
															 "16:30",
															 "17:30",
															 "18:30",
															 "19:30",
															 "20:30",
															 "21:30",
															 "22:30",
															 "23:30"});
			this.cbSundayEnd.Location = new System.Drawing.Point(8, 128);
			this.cbSundayEnd.Name = "cbSundayEnd";
			this.cbSundayEnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbSundayEnd.Size = new System.Drawing.Size(88, 20);
			this.cbSundayEnd.TabIndex = 24;
			// 
			// lblSundayEnd
			// 
			this.lblSundayEnd.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblSundayEnd.Location = new System.Drawing.Point(96, 128);
			this.lblSundayEnd.Name = "lblSundayEnd";
			this.lblSundayEnd.Size = new System.Drawing.Size(72, 16);
			this.lblSundayEnd.TabIndex = 23;
			this.lblSundayEnd.Text = "שעת סיום";
			// 
			// cbSundayStart
			// 
			this.cbSundayStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSundayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.cbSundayStart.Items.AddRange(new object[] {
															   "0:30",
															   "1:30",
															   "2:30",
															   "3:30",
															   "4:30",
															   "5:30",
															   "6:30",
															   "7:30",
															   "8:30",
															   "9:30",
															   "10:30",
															   "11:30",
															   "12:30",
															   "13:30",
															   "14:30",
															   "15:30",
															   "16:30",
															   "17:30",
															   "18:30",
															   "19:30",
															   "20:30",
															   "21:30",
															   "22:30",
															   "23:30"});
			this.cbSundayStart.Location = new System.Drawing.Point(8, 104);
			this.cbSundayStart.Name = "cbSundayStart";
			this.cbSundayStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbSundayStart.Size = new System.Drawing.Size(88, 20);
			this.cbSundayStart.TabIndex = 22;
			// 
			// lblSundayStart
			// 
			this.lblSundayStart.Font = new System.Drawing.Font("David", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblSundayStart.Location = new System.Drawing.Point(96, 104);
			this.lblSundayStart.Name = "lblSundayStart";
			this.lblSundayStart.Size = new System.Drawing.Size(72, 16);
			this.lblSundayStart.TabIndex = 21;
			this.lblSundayStart.Text = "שעת התחלה";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label1.Location = new System.Drawing.Point(112, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 9;
			this.label1.Text = "מינימום ימים חופשיים במערכת:";
			// 
			// tbMinFreeDays
			// 
			this.tbMinFreeDays.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.tbMinFreeDays.Location = new System.Drawing.Point(88, 30);
			this.tbMinFreeDays.Name = "tbMinFreeDays";
			this.tbMinFreeDays.Size = new System.Drawing.Size(24, 20);
			this.tbMinFreeDays.TabIndex = 8;
			this.tbMinFreeDays.Text = "0";
			this.tbMinFreeDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblSaveResult
			// 
			this.lblSaveResult.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblSaveResult.ForeColor = System.Drawing.Color.Blue;
			this.lblSaveResult.Location = new System.Drawing.Point(360, 376);
			this.lblSaveResult.Name = "lblSaveResult";
			this.lblSaveResult.Size = new System.Drawing.Size(184, 16);
			this.lblSaveResult.TabIndex = 54;
			this.lblSaveResult.Text = "השינויים נשמרו בהצלחה";
			this.lblSaveResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblSaveResult.Visible = false;
			// 
			// btDefaults
			// 
			this.btDefaults.BackColor = System.Drawing.Color.Goldenrod;
			this.btDefaults.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btDefaults.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btDefaults.ForeColor = System.Drawing.Color.White;
			this.btDefaults.Location = new System.Drawing.Point(312, 352);
			this.btDefaults.Name = "btDefaults";
			this.btDefaults.Size = new System.Drawing.Size(112, 23);
			this.btDefaults.TabIndex = 55;
			this.btDefaults.Text = "אפס לברירות מחדל";
			this.btDefaults.Click += new System.EventHandler(this.btDefaults_Click);
			// 
			// ConfigControl
			// 
			this.Controls.Add(this.btDefaults);
			this.Controls.Add(this.lblSaveResult);
			this.Controls.Add(this.grpConstraints);
			this.Controls.Add(this.label28);
			this.Controls.Add(this.chbExportNumber);
			this.Controls.Add(this.chbExportName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btSaveChanges);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbStartDay);
			this.Controls.Add(this.tbEndDay);
			this.Controls.Add(this.tbFreeDays);
			this.Controls.Add(this.tbHoles);
			this.Controls.Add(this.cbEndHour);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbStartHour);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chbSaturday);
			this.Controls.Add(this.chbFriday);
			this.Controls.Add(this.chbThursday);
			this.Controls.Add(this.chbWednesday);
			this.Controls.Add(this.chbTuesday);
			this.Controls.Add(this.chbMonday);
			this.Controls.Add(this.chbSunday);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbMinTestDays);
			this.Controls.Add(this.chbTestAlert);
			this.Controls.Add(this.chbRegGroups);
			this.Controls.Add(this.lblMaxCollisions);
			this.Controls.Add(this.tbMaxCollisions);
			this.Controls.Add(this.chbCollisions);
			this.Controls.Add(this.lblMainTitle);
			this.Controls.Add(this.grpPref);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.trackBar5);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.trackBar6);
			this.Controls.Add(this.trackBar7);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.trackBar8);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.label24);
			this.Controls.Add(this.label25);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.grpDisplayOptions);
			this.Controls.Add(this.grpGeneral);
			this.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.Name = "ConfigControl";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Size = new System.Drawing.Size(904, 400);
			this.Click += new System.EventHandler(this.ConfigControl_OnClick);
			((System.ComponentModel.ISupportInitialize)(this.tbHoles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbFreeDays)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbStartDay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbEndDay)).EndInit();
			this.grpPref.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBar5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar8)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.grpConstraints.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events
		private void btAnt_Click(object sender, System.EventArgs e)
		{
			PrefStartHour = -5;
			PrefEndHour = -5;
			PrefFreeDays = -5;
			PrefHoles = -5;
		}

		private void btDontCare_Click(object sender, System.EventArgs e)
		{
			PrefStartHour = 0;
			PrefEndHour = 0;
			PrefFreeDays = 0;
			PrefHoles = 0;
		}

		private void btPadlaa_Click(object sender, System.EventArgs e)
		{
			PrefStartHour = 5;
			PrefEndHour = 5;
			PrefFreeDays = 5;
			PrefHoles = 5;
		}

		private void chbCollisions_CheckedChanged(object sender, System.EventArgs e)
		{
			AllowCollisions = chbCollisions.Checked;
		}
		private void chbTestAlert_CheckedChanged(object sender, System.EventArgs e)
		{
			TestAlert = chbTestAlert.Checked;
		}
		private void chbFreeSunday_CheckedChanged(object sender, System.EventArgs e)
		{
			FreeSunday = chbFreeSunday.Checked;
		}
		private void chbFreeMonday_CheckedChanged(object sender, System.EventArgs e)
		{
			FreeMonday = chbFreeMonday.Checked;
		}

		private void chbFreeTuesday_CheckedChanged(object sender, System.EventArgs e)
		{
			FreeTuesday = chbFreeTuesday.Checked;
		}

		private void chbFreeWednesday_CheckedChanged(object sender, System.EventArgs e)
		{
			FreeWednesday = chbFreeWednesday.Checked;
		}

		private void chbFreeThursday_CheckedChanged(object sender, System.EventArgs e)
		{
			FreeThursday = chbFreeThursday.Checked;
		}

		#endregion

		private void ConfigControl_OnClick(object sender, System.EventArgs e)
		{
			lblSaveResult.Visible = false;
		}

		private void btDefaults_Click(object sender, System.EventArgs e)
		{
			AllowCollisions = false;
			MaxCollisions = 1;
			AllowRegSplit = true;
			TestAlert = true;
			TestInterval = 1;
            Sunday = true;
			Monday = true;
			Tuesday = true;
			Wednesday = true;
			Thursday = true;
			Friday = false;
			Saturday = false;
			StartHour = "8:30";
			EndHour = "18:30";
			PrefEndHour = 0;
			PrefFreeDays = 0;
			PrefHoles = 0;
			PrefStartHour = 0;
			ExportName = true;
			ExportLocation = true;
			ExportNumber = true;
			ExportRegNum = true;
			ExportType = true;
			ExportTeacher = false;
			FreeSunday = false;
			FreeMonday = false;
			FreeTuesday = false;
			FreeWednesday = false;
			FreeThursday = false;
			MaxDailyHours = 0;
			MinDailyHours = 0;
			MinFreeDays = 0;
			SundayStartHour = "8:30";
			SundayEndHour = "18:30";
			MondayStartHour = "8:30";
			MondayEndHour = "18:30";
			TuesdayStartHour = "8:30";
			TuesdayEndHour = "18:30";
			WednesdayStartHour = "8:30";
			WednesdayEndHour = "18:30";
			ThursdayStartHour = "8:30";
			ThursdayEndHour = "18:30";
		}
		
		#region Properties
		public bool AllowCollisions
		{
			get {return chbCollisions.Checked;}
			set 
			{
				chbCollisions.Checked = value;
				if (value==true)
				{
					tbMaxCollisions.Enabled=true;
					lblMaxCollisions.Enabled=true;
				}
				else
				{
					tbMaxCollisions.Enabled=false;
					lblMaxCollisions.Enabled=false;
				}
			}
		}
		public int MaxCollisions
		{
			get 
			{
				try
				{
					return int.Parse(tbMaxCollisions.Text);
				}
				catch
				{
					tbMaxCollisions.Text="0";
					return 0;
				}
			}
			set { tbMaxCollisions.Text = value.ToString();}
		}

		public bool AllowRegSplit
		{
			get {return chbRegGroups.Checked;}
			set {chbRegGroups.Checked = value;}
		}
		public bool TestAlert
		{
			get {return chbTestAlert.Checked;}
			set 
			{
				chbTestAlert.Checked = value;
				if (value==true)
				{
					tbMinTestDays.Enabled=true;
				}
				else
				{
					tbMinTestDays.Enabled=false;
				}
			}
		}
		public int TestInterval
		{
			get 
			{
				try
				{
					return int.Parse(tbMinTestDays.Text);
				}
				catch
				{
					tbMinTestDays.Text="1";
					return 1;
				}
		 	}
			set { tbMinTestDays.Text = value.ToString();}
		}


		public bool Sunday
		{
			get {return chbSunday.Checked;}
			set 
			{
				chbSunday.Checked = value;
			}
		}
		public bool Monday
		{
			get {return chbMonday.Checked;}
			set {chbMonday.Checked = value;}
		}
		public bool Tuesday
		{
			get {return chbTuesday.Checked;}
			set {chbTuesday.Checked = value;}
		}
		public bool Wednesday
		{
			get {return chbWednesday.Checked;}
			set {chbWednesday.Checked = value;}
		}
		public bool Thursday
		{
			get {return chbThursday.Checked;}
			set {chbThursday.Checked = value;}
		}
		public bool Friday
		{
			get {return chbFriday.Checked;}
			set {chbFriday.Checked = value;}
		}
		public bool Saturday
		{
			get {return chbSaturday.Checked;}
			set {chbSaturday.Checked = value;}
		}
		public string StartHour
		{
			get 
			{
				string hour = cbStartHour.SelectedItem as string;;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbStartHour.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbStartHour.SelectedItem="8:30";
					return;
				}

				if (cbEndHour.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbEndHour.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbEndHour.SelectedItem=value + ":30";
					}
				}
				cbStartHour.SelectedItem = value + ":30";
			}
		}
		public string EndHour
		{
			get 
			{
				string hour = cbEndHour.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbEndHour.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbEndHour.SelectedItem="20:30";
					return;
				}

				if (cbStartHour.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbStartHour.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbStartHour.SelectedItem=value + ":30";
					}
				}
				cbEndHour.SelectedItem = value + ":30";
			}
		}


		public int PrefStartHour
		{
			get {return tbStartDay.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbStartDay.Value=value;
			}
		}
		public int PrefEndHour
		{
			get {return tbEndDay.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbEndDay.Value=value;
			}
		}
		public int PrefFreeDays
		{
			get {return tbFreeDays.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbFreeDays.Value=value;
			}
		}
		public int PrefHoles
		{
			get {return tbHoles.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbHoles.Value=value;
			}
		}


		public bool ExportName
		{
			get {return chbExportName.Checked;}
			set {chbExportName.Checked = value;}
		}
		public bool ExportNumber
		{
			get {return chbExportNumber.Checked;}
			set {chbExportNumber.Checked = value;}
		}
		public bool ExportType
		{
			get {return chbExportType.Checked;}
			set {chbExportType.Checked = value;}
		}
		public bool ExportRegNum
		{
			get {return chbExportRegNum.Checked;}
			set {chbExportRegNum.Checked = value;}
		}
		public bool ExportLocation
		{
			get {return chbExportLocation.Checked;}
			set {chbExportLocation.Checked = value;}
		}
		public bool ExportTeacher
		{
			get {return chbExportTeacher.Checked;}
			set {chbExportTeacher.Checked = value;}
		}


		public bool FreeSunday
		{
			get {return chbFreeSunday.Checked;}
			set 
			{
				chbFreeSunday.Checked = value;
				if (value==true)
				{
					cbSundayStart.Enabled=false;
					cbSundayEnd.Enabled=false;
					lblSundayStart.Enabled=false;
					lblSundayEnd.Enabled=false;
				}
				else
				{
					cbSundayStart.Enabled=true;
					cbSundayEnd.Enabled=true;
					lblSundayStart.Enabled=true;
					lblSundayEnd.Enabled=true;
				}
			}
		}
		public bool FreeMonday
		{
			get {return chbFreeMonday.Checked;}
			set 
			{
				chbFreeMonday.Checked = value;
				if (value==true)
				{
					cbMondayStart.Enabled=false;
					cbMondayEnd.Enabled=false;
					lblMondayStart.Enabled=false;
					lblMondayEnd.Enabled=false;
				}
				else
				{
					cbMondayStart.Enabled=true;
					cbMondayEnd.Enabled=true;
					lblMondayStart.Enabled=true;
					lblMondayEnd.Enabled=true;
					
				}
			}
		}
		public bool FreeTuesday
		{
			get {return chbFreeTuesday.Checked;}
			set 
			{
				chbFreeTuesday.Checked = value;
				if (value==true)
				{
					cbTuesdayStart.Enabled=false;
					cbTuesdayEnd.Enabled=false;
					lblTuesdayStart.Enabled=false;
					lblTuesdayEnd.Enabled=false;
				}
				else
				{
					cbTuesdayStart.Enabled=true;
					cbTuesdayEnd.Enabled=true;
					lblTuesdayStart.Enabled=true;
					lblTuesdayEnd.Enabled=true;
				}
			}
		}
		public bool FreeWednesday
		{
			get {return chbFreeWednesday.Checked;}
			set 
			{
				chbFreeWednesday.Checked = value;
				if (value==true)
				{
					cbWednesdayStart.Enabled=false;
					cbWednesdayEnd.Enabled=false;
					lblWednesdayStart.Enabled=false;
					lblWednesdayEnd.Enabled=false;
				}
				else
				{
					cbWednesdayStart.Enabled=true;
					cbWednesdayEnd.Enabled=true;
					lblWednesdayStart.Enabled=true;
					lblWednesdayEnd.Enabled=true;
				}
			}
		}
		public bool FreeThursday
		{
			get {return chbFreeThursday.Checked;}
			set 
			{
				chbFreeThursday.Checked = value;
				if (value==true)
				{
					cbThursdayStart.Enabled=false;
					cbThursdayEnd.Enabled=false;
					lblThursdayStart.Enabled=false;
					lblThursdayEnd.Enabled=false;
				}
				else
				{
					cbThursdayStart.Enabled=true;
					cbThursdayEnd.Enabled=true;
					lblThursdayStart.Enabled=true;
					lblThursdayEnd.Enabled=true;
				}
			}
		}

		
		public string SundayStartHour
		{
			get 
			{
				string hour = cbSundayStart.SelectedItem as string;;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbSundayStart.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbSundayStart.SelectedItem="8:30";
					return;
				}

				if (cbSundayEnd.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbSundayEnd.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbSundayEnd.SelectedItem=value + ":30";
					}
				}
				cbSundayStart.SelectedItem = value + ":30";
			}
		}
		public string SundayEndHour
		{
			get 
			{
				string hour = cbSundayEnd.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbSundayEnd.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbSundayEnd.SelectedItem="20:30";
					return;
				}

				if (cbSundayStart.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbSundayStart.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbSundayStart.SelectedItem=value + ":30";
					}
				}
				cbSundayEnd.SelectedItem = value + ":30";
			}
		}


		public string MondayStartHour
		{
			get 
			{
				string hour = cbMondayStart.SelectedItem as string;;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbMondayStart.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbMondayStart.SelectedItem="8:30";
					return;
				}

				if (cbMondayEnd.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbMondayEnd.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbMondayEnd.SelectedItem=value + ":30";
					}
				}
				cbMondayStart.SelectedItem = value + ":30";
			}
		}
		public string MondayEndHour
		{
			get 
			{
				string hour = cbMondayEnd.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbMondayEnd.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbMondayEnd.SelectedItem="20:30";
					return;
				}

				if (cbMondayStart.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbMondayStart.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbMondayStart.SelectedItem=value + ":30";
					}
				}
				cbMondayEnd.SelectedItem = value + ":30";
			}
		}


		public string TuesdayStartHour
		{
			get 
			{
				string hour = cbTuesdayStart.SelectedItem as string;;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbTuesdayStart.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbTuesdayStart.SelectedItem="8:30";
					return;
				}

				if (cbTuesdayEnd.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbTuesdayEnd.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbTuesdayEnd.SelectedItem=value + ":30";
					}
				}
				cbTuesdayStart.SelectedItem = value + ":30";
			}
		}
		public string TuesdayEndHour
		{
			get 
			{
				string hour = cbTuesdayEnd.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbTuesdayEnd.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbTuesdayEnd.SelectedItem="20:30";
					return;
				}

				if (cbTuesdayStart.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbTuesdayStart.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbTuesdayStart.SelectedItem=value + ":30";
					}
				}
				cbTuesdayEnd.SelectedItem = value + ":30";
			}
		}


		public string WednesdayStartHour
		{
			get 
			{
				string hour = cbWednesdayStart.SelectedItem as string;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbWednesdayStart.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbWednesdayStart.SelectedItem="8:30";
					return;
				}

				if (cbWednesdayEnd.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbWednesdayEnd.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbWednesdayEnd.SelectedItem=value + ":30";
					}
				}
				cbWednesdayStart.SelectedItem = value + ":30";
			}
		}
		public string WednesdayEndHour
		{
			get 
			{
				string hour = cbWednesdayEnd.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbWednesdayEnd.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbWednesdayEnd.SelectedItem="20:30";
					return;
				}

				if (cbWednesdayStart.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbWednesdayStart.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbWednesdayStart.SelectedItem=value + ":30";
					}
				}
				cbWednesdayEnd.SelectedItem = value + ":30";
			}
		}


		public string ThursdayStartHour
		{
			get 
			{
				string hour = cbThursdayStart.SelectedItem as string;;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbThursdayStart.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbThursdayStart.SelectedItem="8:30";
					return;
				}

				if (cbThursdayEnd.SelectedItem!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbThursdayEnd.SelectedItem)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbThursdayEnd.SelectedItem=value + ":30";
					}
				}
				cbThursdayStart.SelectedItem = value + ":30";
			}
		}
		public string ThursdayEndHour
		{
			get 
			{
				string hour = cbThursdayEnd.SelectedItem as string ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				//string hour=(string)cbThursdayEnd.SelectedItem;
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbThursdayEnd.SelectedItem="20:30";
					return;
				}

				if (cbThursdayStart.SelectedItem!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbThursdayStart.SelectedItem).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbThursdayStart.SelectedItem=value + ":30";
					}
				}
				cbThursdayEnd.SelectedItem = value + ":30";
			}
		}

		
		public int MaxDailyHours
		{
			get 
			{
				try
				{
					return int.Parse(tbMaxDailyHours.Text);
				}
				catch
				{
					tbMaxDailyHours.Text="0";
					return 0;
				}
			}
			set { tbMaxDailyHours.Text = value.ToString();}
		}

		public int MinDailyHours
		{
			get 
			{
				try
				{
					return int.Parse(tbMinDailyHours.Text);
				}
				catch
				{
					tbMinDailyHours.Text="0";
					return 0;
				}
			}
			set { tbMinDailyHours.Text = value.ToString();}
		}

		public int MinFreeDays
		{
			get 
			{
				try
				{
					return int.Parse(tbMinFreeDays.Text);
				}
				catch
				{
					tbMinFreeDays.Text="0";
					return 0;
				}
			}
			set { tbMinFreeDays.Text = value.ToString();}
		}


		/*public Label SavedLabel
		{
			get { return lblSaveResult;}
		}

		public Button Save
		{
			get { return btSaveChanges; }
		}*/

		public bool SavedLabelVisible
		{
			get { return lblSaveResult.Visible; }
			set { lblSaveResult.Visible = value; }
		}
		
		#endregion

		public event System.EventHandler Save
		{
			add { btSaveChanges.Click += value; }
			remove { btSaveChanges.Click -= value; }
		}

		public void InitiateSave()
		{
			btSaveChanges.PerformClick();
		}
	}
}
