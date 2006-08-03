using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using UDonkey.Logic;
using UDonkey.RepFile;
using UDonkey.DB;
using UDonkey.GUI;

namespace UDonkey.GUI
{
    /// <summary>
    /// Summary description for DBbrowser.
    /// </summary>
    public class DBbrowser : System.Windows.Forms.UserControl	
    {
		private const string RESOURCES_GROUP = "SearchTab";
		//private CourseDB mCourseDB;
        private Course mCurrentCourse;
        //private CoursesScheduler mCoursesScheduler;
        private bool mFacultyClicked;
		private bool mCourseBasketClicked;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader numberColumn;
        private System.Windows.Forms.Label LabelCourseName;
        private System.Windows.Forms.Label labelCourseNum;
        private System.Windows.Forms.TextBox tbCourseNum;
        private System.Windows.Forms.Label labelFaculty;
        private System.Windows.Forms.ListView lbCourses;
        private System.Windows.Forms.ComboBox cbFaculties;
        private System.Windows.Forms.TextBox tbAcademicPoints;
        private System.Windows.Forms.TextBox tbLecturer;
        private System.Windows.Forms.TextBox tbMoedA;
        private System.Windows.Forms.TextBox tbMoedB;
        private System.Windows.Forms.TextBox tbLectureHours;
        private System.Windows.Forms.TextBox tbTutorialHours;
        private System.Windows.Forms.TextBox tbLabHours;
        private System.Windows.Forms.TextBox tbProjectHours;
        private System.Windows.Forms.Label labelAcademicPoints;
        private System.Windows.Forms.Label labelLecturer;
        private System.Windows.Forms.Label labelMoedA;
        private System.Windows.Forms.Label labelMoedB;
        private System.Windows.Forms.Label labelLectureHours;
        private System.Windows.Forms.Label labelTutorialHours;
        private System.Windows.Forms.Label labelLabHours;
        private System.Windows.Forms.Label labelProjectHours;
        private System.Windows.Forms.Label labelHours;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btAddCourse;
        private System.Windows.Forms.Label lblAdvancedSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbCourseBasket;
        private System.Windows.Forms.Label lblCourseBasket;
        private System.Windows.Forms.Button btRemoveCourse;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lvOccurences;
		private System.Windows.Forms.ColumnHeader chLocation;
		private System.Windows.Forms.ColumnHeader chUntil;
		private System.Windows.Forms.ColumnHeader chFrom;
		private System.Windows.Forms.ColumnHeader chDay;
		private System.Windows.Forms.ListView lvCourseEvents;
		private System.Windows.Forms.ColumnHeader chRegNum;
		private System.Windows.Forms.ColumnHeader chEvent;
		private SearchControl searchControl1;
		private System.Windows.Forms.Button btChooseAll;
		private System.Windows.Forms.Button btChooseNone;
		private System.Windows.Forms.Button btReverseChecked;
		private System.Windows.Forms.TextBox tbNickName;
		private System.Windows.Forms.Label lblNickName;
		private ImageList ig;
		private System.Windows.Forms.Button btRemoveAll;
		private System.Windows.Forms.Button btDone;
		private System.Windows.Forms.Label lblSelectedPoints;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblSemester;
		private System.Windows.Forms.Button btUGLink;
		private System.ComponentModel.IContainer components;

        public DBbrowser( )
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
			mFacultyClicked=false;
			mCourseBasketClicked=false;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DBbrowser));
			this.LabelCourseName = new System.Windows.Forms.Label();
			this.labelCourseNum = new System.Windows.Forms.Label();
			this.tbCourseNum = new System.Windows.Forms.TextBox();
			this.labelFaculty = new System.Windows.Forms.Label();
			this.tbAcademicPoints = new System.Windows.Forms.TextBox();
			this.tbLecturer = new System.Windows.Forms.TextBox();
			this.labelAcademicPoints = new System.Windows.Forms.Label();
			this.labelLecturer = new System.Windows.Forms.Label();
			this.tbMoedA = new System.Windows.Forms.TextBox();
			this.labelMoedA = new System.Windows.Forms.Label();
			this.labelMoedB = new System.Windows.Forms.Label();
			this.tbMoedB = new System.Windows.Forms.TextBox();
			this.labelLectureHours = new System.Windows.Forms.Label();
			this.tbLectureHours = new System.Windows.Forms.TextBox();
			this.tbTutorialHours = new System.Windows.Forms.TextBox();
			this.labelTutorialHours = new System.Windows.Forms.Label();
			this.tbLabHours = new System.Windows.Forms.TextBox();
			this.labelLabHours = new System.Windows.Forms.Label();
			this.tbProjectHours = new System.Windows.Forms.TextBox();
			this.labelProjectHours = new System.Windows.Forms.Label();
			this.labelHours = new System.Windows.Forms.Label();
			this.lbCourses = new System.Windows.Forms.ListView();
			this.numberColumn = new System.Windows.Forms.ColumnHeader();
			this.nameColumn = new System.Windows.Forms.ColumnHeader();
			this.cbFaculties = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btAddCourse = new System.Windows.Forms.Button();
			this.lblAdvancedSearch = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.searchControl1 = new UDonkey.GUI.SearchControl();
			this.lbCourseBasket = new System.Windows.Forms.ListBox();
			this.lblCourseBasket = new System.Windows.Forms.Label();
			this.btRemoveCourse = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btReverseChecked = new System.Windows.Forms.Button();
			this.lvCourseEvents = new System.Windows.Forms.ListView();
			this.chRegNum = new System.Windows.Forms.ColumnHeader();
			this.chEvent = new System.Windows.Forms.ColumnHeader();
			this.ig = new System.Windows.Forms.ImageList(this.components);
			this.lvOccurences = new System.Windows.Forms.ListView();
			this.chLocation = new System.Windows.Forms.ColumnHeader();
			this.chUntil = new System.Windows.Forms.ColumnHeader();
			this.chFrom = new System.Windows.Forms.ColumnHeader();
			this.chDay = new System.Windows.Forms.ColumnHeader();
			this.btChooseAll = new System.Windows.Forms.Button();
			this.btChooseNone = new System.Windows.Forms.Button();
			this.tbNickName = new System.Windows.Forms.TextBox();
			this.lblNickName = new System.Windows.Forms.Label();
			this.btRemoveAll = new System.Windows.Forms.Button();
			this.btDone = new System.Windows.Forms.Button();
			this.lblSelectedPoints = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblSemester = new System.Windows.Forms.Label();
			this.btUGLink = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// LabelCourseName
			// 
			this.LabelCourseName.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.LabelCourseName.Location = new System.Drawing.Point(592, 8);
			this.LabelCourseName.Name = "LabelCourseName";
			this.LabelCourseName.Size = new System.Drawing.Size(112, 16);
			this.LabelCourseName.TabIndex = 1;
			this.LabelCourseName.Text = "רשימת הקורסים";
			this.LabelCourseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCourseNum
			// 
			this.labelCourseNum.Location = new System.Drawing.Point(832, 56);
			this.labelCourseNum.Name = "labelCourseNum";
			this.labelCourseNum.Size = new System.Drawing.Size(64, 23);
			this.labelCourseNum.TabIndex = 2;
			this.labelCourseNum.Text = "מספר הקורס";
			// 
			// tbCourseNum
			// 
			this.tbCourseNum.Location = new System.Drawing.Point(752, 56);
			this.tbCourseNum.Name = "tbCourseNum";
			this.tbCourseNum.Size = new System.Drawing.Size(72, 20);
			this.tbCourseNum.TabIndex = 3;
			this.tbCourseNum.Text = "";
			this.tbCourseNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelFaculty
			// 
			this.labelFaculty.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.labelFaculty.Location = new System.Drawing.Point(720, 8);
			this.labelFaculty.Name = "labelFaculty";
			this.labelFaculty.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.labelFaculty.Size = new System.Drawing.Size(176, 16);
			this.labelFaculty.TabIndex = 5;
			this.labelFaculty.Text = "נווט על פי:";
			this.labelFaculty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbAcademicPoints
			// 
			this.tbAcademicPoints.Location = new System.Drawing.Point(728, 136);
			this.tbAcademicPoints.Name = "tbAcademicPoints";
			this.tbAcademicPoints.ReadOnly = true;
			this.tbAcademicPoints.Size = new System.Drawing.Size(88, 20);
			this.tbAcademicPoints.TabIndex = 6;
			this.tbAcademicPoints.Text = "";
			this.tbAcademicPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbLecturer
			// 
			this.tbLecturer.Location = new System.Drawing.Point(728, 208);
			this.tbLecturer.Name = "tbLecturer";
			this.tbLecturer.ReadOnly = true;
			this.tbLecturer.Size = new System.Drawing.Size(104, 20);
			this.tbLecturer.TabIndex = 7;
			this.tbLecturer.Text = "";
			this.tbLecturer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelAcademicPoints
			// 
			this.labelAcademicPoints.Location = new System.Drawing.Point(816, 136);
			this.labelAcademicPoints.Name = "labelAcademicPoints";
			this.labelAcademicPoints.Size = new System.Drawing.Size(80, 23);
			this.labelAcademicPoints.TabIndex = 8;
			this.labelAcademicPoints.Text = "נקודות אקדמיות";
			// 
			// labelLecturer
			// 
			this.labelLecturer.Location = new System.Drawing.Point(832, 208);
			this.labelLecturer.Name = "labelLecturer";
			this.labelLecturer.Size = new System.Drawing.Size(64, 23);
			this.labelLecturer.TabIndex = 9;
			this.labelLecturer.Text = "מרצה אחראי";
			// 
			// tbMoedA
			// 
			this.tbMoedA.Location = new System.Drawing.Point(728, 160);
			this.tbMoedA.Name = "tbMoedA";
			this.tbMoedA.ReadOnly = true;
			this.tbMoedA.Size = new System.Drawing.Size(120, 20);
			this.tbMoedA.TabIndex = 10;
			this.tbMoedA.Text = "";
			this.tbMoedA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelMoedA
			// 
			this.labelMoedA.Location = new System.Drawing.Point(856, 160);
			this.labelMoedA.Name = "labelMoedA";
			this.labelMoedA.Size = new System.Drawing.Size(40, 23);
			this.labelMoedA.TabIndex = 11;
			this.labelMoedA.Text = "מועד א\'";
			// 
			// labelMoedB
			// 
			this.labelMoedB.Location = new System.Drawing.Point(856, 184);
			this.labelMoedB.Name = "labelMoedB";
			this.labelMoedB.Size = new System.Drawing.Size(40, 23);
			this.labelMoedB.TabIndex = 13;
			this.labelMoedB.Text = "מועד ב\'";
			// 
			// tbMoedB
			// 
			this.tbMoedB.Location = new System.Drawing.Point(728, 184);
			this.tbMoedB.Name = "tbMoedB";
			this.tbMoedB.ReadOnly = true;
			this.tbMoedB.Size = new System.Drawing.Size(120, 20);
			this.tbMoedB.TabIndex = 12;
			this.tbMoedB.Text = "";
			this.tbMoedB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelLectureHours
			// 
			this.labelLectureHours.Location = new System.Drawing.Point(816, 232);
			this.labelLectureHours.Name = "labelLectureHours";
			this.labelLectureHours.Size = new System.Drawing.Size(24, 16);
			this.labelLectureHours.TabIndex = 14;
			this.labelLectureHours.Text = "הר";
			// 
			// tbLectureHours
			// 
			this.tbLectureHours.Location = new System.Drawing.Point(816, 248);
			this.tbLectureHours.Name = "tbLectureHours";
			this.tbLectureHours.ReadOnly = true;
			this.tbLectureHours.Size = new System.Drawing.Size(24, 20);
			this.tbLectureHours.TabIndex = 15;
			this.tbLectureHours.Text = "";
			this.tbLectureHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbTutorialHours
			// 
			this.tbTutorialHours.Location = new System.Drawing.Point(792, 248);
			this.tbTutorialHours.Name = "tbTutorialHours";
			this.tbTutorialHours.ReadOnly = true;
			this.tbTutorialHours.Size = new System.Drawing.Size(24, 20);
			this.tbTutorialHours.TabIndex = 17;
			this.tbTutorialHours.Text = "";
			this.tbTutorialHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelTutorialHours
			// 
			this.labelTutorialHours.Location = new System.Drawing.Point(792, 232);
			this.labelTutorialHours.Name = "labelTutorialHours";
			this.labelTutorialHours.Size = new System.Drawing.Size(24, 16);
			this.labelTutorialHours.TabIndex = 16;
			this.labelTutorialHours.Text = "תר";
			// 
			// tbLabHours
			// 
			this.tbLabHours.Location = new System.Drawing.Point(768, 248);
			this.tbLabHours.Name = "tbLabHours";
			this.tbLabHours.ReadOnly = true;
			this.tbLabHours.Size = new System.Drawing.Size(24, 20);
			this.tbLabHours.TabIndex = 19;
			this.tbLabHours.Text = "";
			this.tbLabHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelLabHours
			// 
			this.labelLabHours.Location = new System.Drawing.Point(768, 232);
			this.labelLabHours.Name = "labelLabHours";
			this.labelLabHours.Size = new System.Drawing.Size(24, 16);
			this.labelLabHours.TabIndex = 18;
			this.labelLabHours.Text = "מע";
			// 
			// tbProjectHours
			// 
			this.tbProjectHours.Location = new System.Drawing.Point(744, 248);
			this.tbProjectHours.Name = "tbProjectHours";
			this.tbProjectHours.ReadOnly = true;
			this.tbProjectHours.Size = new System.Drawing.Size(24, 20);
			this.tbProjectHours.TabIndex = 21;
			this.tbProjectHours.Text = "";
			this.tbProjectHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelProjectHours
			// 
			this.labelProjectHours.Location = new System.Drawing.Point(744, 232);
			this.labelProjectHours.Name = "labelProjectHours";
			this.labelProjectHours.Size = new System.Drawing.Size(24, 16);
			this.labelProjectHours.TabIndex = 20;
			this.labelProjectHours.Text = "פר";
			// 
			// labelHours
			// 
			this.labelHours.Location = new System.Drawing.Point(848, 232);
			this.labelHours.Name = "labelHours";
			this.labelHours.Size = new System.Drawing.Size(48, 32);
			this.labelHours.TabIndex = 22;
			this.labelHours.Text = "שעות שבועיות";
			// 
			// lbCourses
			// 
			this.lbCourses.AllowColumnReorder = true;
			this.lbCourses.AutoArrange = false;
			this.lbCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.numberColumn,
																						this.nameColumn});
			this.lbCourses.FullRowSelect = true;
			this.lbCourses.HideSelection = false;
			this.lbCourses.Location = new System.Drawing.Point(296, 24);
			this.lbCourses.MultiSelect = false;
			this.lbCourses.Name = "lbCourses";
			this.lbCourses.Size = new System.Drawing.Size(416, 216);
			this.lbCourses.TabIndex = 24;
			this.lbCourses.View = System.Windows.Forms.View.Details;
			this.lbCourses.ColumnClick += new ColumnClickEventHandler(this.lbCourses_ColumnClick);
			
			// 
			// numberColumn
			// 
			this.numberColumn.Text = "מס קורס";
			this.numberColumn.Width = 57;
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "שם הקורס";
			this.nameColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nameColumn.Width = 338;
			// 
			// cbFaculties
			// 
			this.cbFaculties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFaculties.Location = new System.Drawing.Point(720, 24);
			this.cbFaculties.Name = "cbFaculties";
			this.cbFaculties.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.cbFaculties.Size = new System.Drawing.Size(176, 21);
			this.cbFaculties.Sorted = true;
			this.cbFaculties.TabIndex = 25;
			this.cbFaculties.Click += new System.EventHandler(this.cbFaculties_OnClick);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label1.Location = new System.Drawing.Point(728, 304);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 28;
			this.label1.Text = "ארועים";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("David", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label2.Location = new System.Drawing.Point(312, 304);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(400, 16);
			this.label2.TabIndex = 29;
			this.label2.Text = "זמנים ומיקומים";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btAddCourse
			// 
			this.btAddCourse.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.btAddCourse.Font = new System.Drawing.Font("Miriam", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btAddCourse.Location = new System.Drawing.Point(480, 242);
			this.btAddCourse.Name = "btAddCourse";
			this.btAddCourse.Size = new System.Drawing.Size(232, 38);
			this.btAddCourse.TabIndex = 30;
			this.btAddCourse.Text = "הוסף קורס לסל";
			// 
			// lblAdvancedSearch
			// 
			this.lblAdvancedSearch.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblAdvancedSearch.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.lblAdvancedSearch.Location = new System.Drawing.Point(16, 208);
			this.lblAdvancedSearch.Name = "lblAdvancedSearch";
			this.lblAdvancedSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblAdvancedSearch.Size = new System.Drawing.Size(272, 16);
			this.lblAdvancedSearch.TabIndex = 33;
			this.lblAdvancedSearch.Text = "חיפוש מתקדם";
			this.lblAdvancedSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox1.Controls.Add(this.searchControl1);
			this.groupBox1.Location = new System.Drawing.Point(8, 192);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 288);
			this.groupBox1.TabIndex = 34;
			this.groupBox1.TabStop = false;
			// 
			// searchControl1
			// 
			this.searchControl1.Location = new System.Drawing.Point(8, 32);
			this.searchControl1.Name = "searchControl1";
			this.searchControl1.Size = new System.Drawing.Size(272, 248);
			this.searchControl1.TabIndex = 0;
			// 
			// lbCourseBasket
			// 
			this.lbCourseBasket.BackColor = System.Drawing.Color.FloralWhite;
			this.lbCourseBasket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lbCourseBasket.ItemHeight = 16;
			this.lbCourseBasket.Location = new System.Drawing.Point(8, 24);
			this.lbCourseBasket.Name = "lbCourseBasket";
			this.lbCourseBasket.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lbCourseBasket.Size = new System.Drawing.Size(280, 116);
			this.lbCourseBasket.TabIndex = 35;
			this.lbCourseBasket.Click += new System.EventHandler(this.lbCourseBasket_OnClick);
			this.lbCourseBasket.SelectedIndexChanged += new System.EventHandler(this.lbCourseBasket_SelectedIndexChanged);
			// 
			// lblCourseBasket
			// 
			this.lblCourseBasket.BackColor = System.Drawing.SystemColors.Control;
			this.lblCourseBasket.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblCourseBasket.ForeColor = System.Drawing.Color.Black;
			this.lblCourseBasket.Location = new System.Drawing.Point(128, 8);
			this.lblCourseBasket.Name = "lblCourseBasket";
			this.lblCourseBasket.Size = new System.Drawing.Size(160, 16);
			this.lblCourseBasket.TabIndex = 36;
			this.lblCourseBasket.Text = "סל הקורסים שנבחרו    (";
			this.lblCourseBasket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btRemoveCourse
			// 
			this.btRemoveCourse.BackColor = System.Drawing.Color.Red;
			this.btRemoveCourse.Font = new System.Drawing.Font("Miriam", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btRemoveCourse.Location = new System.Drawing.Point(112, 144);
			this.btRemoveCourse.Name = "btRemoveCourse";
			this.btRemoveCourse.Size = new System.Drawing.Size(176, 40);
			this.btRemoveCourse.TabIndex = 37;
			this.btRemoveCourse.Text = "הסר קורס מהסל";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(720, 120);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(184, 160);
			this.groupBox2.TabIndex = 38;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "פרטי קורס";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btReverseChecked);
			this.groupBox3.Controls.Add(this.lvCourseEvents);
			this.groupBox3.Controls.Add(this.lvOccurences);
			this.groupBox3.Location = new System.Drawing.Point(304, 288);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(600, 195);
			this.groupBox3.TabIndex = 39;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "במידה וחלק מהקבוצות אינם רלוונטים עבורך, בטל את בחירתם פה";
			// 
			// btReverseChecked
			// 
			this.btReverseChecked.Location = new System.Drawing.Point(336, 168);
			this.btReverseChecked.Name = "btReverseChecked";
			this.btReverseChecked.Size = new System.Drawing.Size(80, 24);
			this.btReverseChecked.TabIndex = 41;
			this.btReverseChecked.Text = "הפוך בחירה";
			this.btReverseChecked.Click += new System.EventHandler(this.btReverseChecked_Click);
			// 
			// lvCourseEvents
			// 
			this.lvCourseEvents.CheckBoxes = true;
			this.lvCourseEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.chRegNum,
																							 this.chEvent});
			this.lvCourseEvents.FullRowSelect = true;
			this.lvCourseEvents.HideSelection = false;
			this.lvCourseEvents.LargeImageList = this.ig;
			this.lvCourseEvents.Location = new System.Drawing.Point(336, 32);
			this.lvCourseEvents.MultiSelect = false;
			this.lvCourseEvents.Name = "lvCourseEvents";
			this.lvCourseEvents.Size = new System.Drawing.Size(256, 136);
			this.lvCourseEvents.SmallImageList = this.ig;
			this.lvCourseEvents.TabIndex = 1;
			this.lvCourseEvents.View = System.Windows.Forms.View.Details;
			this.lvCourseEvents.SelectedIndexChanged += new System.EventHandler(this.lvCourseEvents_SelectedIndexChanged);
			// 
			// chRegNum
			// 
			this.chRegNum.Text = "קבוצה";
			this.chRegNum.Width = 65;
			// 
			// chEvent
			// 
			this.chEvent.Text = "קבוצה";
			this.chEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.chEvent.Width = 166;
			// 
			// ig
			// 
			this.ig.ImageSize = new System.Drawing.Size(16, 16);
			this.ig.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ig.ImageStream")));
			this.ig.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// lvOccurences
			// 
			this.lvOccurences.CheckBoxes = true;
			this.lvOccurences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.chLocation,
																						   this.chUntil,
																						   this.chFrom,
																						   this.chDay});
			this.lvOccurences.FullRowSelect = true;
			this.lvOccurences.HideSelection = false;
			this.lvOccurences.Location = new System.Drawing.Point(3, 32);
			this.lvOccurences.MultiSelect = false;
			this.lvOccurences.Name = "lvOccurences";
			this.lvOccurences.Size = new System.Drawing.Size(325, 136);
			this.lvOccurences.TabIndex = 0;
			this.lvOccurences.View = System.Windows.Forms.View.Details;
			// 
			// chLocation
			// 
			this.chLocation.Text = "מיקום";
			this.chLocation.Width = 140;
			// 
			// chUntil
			// 
			this.chUntil.Text = "עד שעה";
			this.chUntil.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.chUntil.Width = 66;
			// 
			// chFrom
			// 
			this.chFrom.Text = "משעה";
			this.chFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.chFrom.Width = 67;
			// 
			// chDay
			// 
			this.chDay.Text = "יום";
			this.chDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.chDay.Width = 48;
			// 
			// btChooseAll
			// 
			this.btChooseAll.Location = new System.Drawing.Point(816, 456);
			this.btChooseAll.Name = "btChooseAll";
			this.btChooseAll.Size = new System.Drawing.Size(80, 24);
			this.btChooseAll.TabIndex = 40;
			this.btChooseAll.Text = "בחר הכל";
			this.btChooseAll.Click += new System.EventHandler(this.btChooseAll_Click);
			// 
			// btChooseNone
			// 
			this.btChooseNone.Location = new System.Drawing.Point(720, 456);
			this.btChooseNone.Name = "btChooseNone";
			this.btChooseNone.Size = new System.Drawing.Size(96, 24);
			this.btChooseNone.TabIndex = 41;
			this.btChooseNone.Text = "הסר בחירה מהכל";
			this.btChooseNone.Click += new System.EventHandler(this.btChooseNone_Click);
			// 
			// tbNickName
			// 
			this.tbNickName.Enabled = false;
			this.tbNickName.Location = new System.Drawing.Point(720, 88);
			this.tbNickName.Name = "tbNickName";
			this.tbNickName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.tbNickName.Size = new System.Drawing.Size(144, 20);
			this.tbNickName.TabIndex = 42;
			this.tbNickName.Text = "";
			// 
			// lblNickName
			// 
			this.lblNickName.Location = new System.Drawing.Point(864, 88);
			this.lblNickName.Name = "lblNickName";
			this.lblNickName.Size = new System.Drawing.Size(32, 16);
			this.lblNickName.TabIndex = 43;
			this.lblNickName.Text = "כינוי";
			// 
			// btRemoveAll
			// 
			this.btRemoveAll.BackColor = System.Drawing.Color.Red;
			this.btRemoveAll.Font = new System.Drawing.Font("Miriam", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btRemoveAll.Location = new System.Drawing.Point(8, 144);
			this.btRemoveAll.Name = "btRemoveAll";
			this.btRemoveAll.Size = new System.Drawing.Size(80, 40);
			this.btRemoveAll.TabIndex = 44;
			this.btRemoveAll.Text = "הסר הכל מהסל";
			// 
			// btDone
			// 
			this.btDone.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(255)));
			this.btDone.Font = new System.Drawing.Font("Miriam", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btDone.Image = ((System.Drawing.Image)(resources.GetObject("btDone.Image")));
			this.btDone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btDone.Location = new System.Drawing.Point(296, 242);
			this.btDone.Name = "btDone";
			this.btDone.Size = new System.Drawing.Size(176, 38);
			this.btDone.TabIndex = 45;
			this.btDone.Text = "בנה מערכות!";
			// 
			// lblSelectedPoints
			// 
			this.lblSelectedPoints.Font = new System.Drawing.Font("David", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.lblSelectedPoints.Location = new System.Drawing.Point(96, 8);
			this.lblSelectedPoints.Name = "lblSelectedPoints";
			this.lblSelectedPoints.Size = new System.Drawing.Size(32, 14);
			this.lblSelectedPoints.TabIndex = 48;
			this.lblSelectedPoints.Text = "0";
			this.lblSelectedPoints.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(24, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 47;
			this.label3.Text = " נקודות   )";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSemester
			// 
			this.lblSemester.Font = new System.Drawing.Font("David", 12F, System.Drawing.FontStyle.Bold);
			this.lblSemester.Location = new System.Drawing.Point(296, 7);
			this.lblSemester.Name = "lblSemester";
			this.lblSemester.Size = new System.Drawing.Size(296, 16);
			this.lblSemester.TabIndex = 49;
			this.lblSemester.Text = "נכון לסמסטר";
			// 
			// btUGLink
			// 
			this.btUGLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btUGLink.Image = ((System.Drawing.Image)(resources.GetObject("btUGLink.Image")));
			this.btUGLink.Location = new System.Drawing.Point(717, 52);
			this.btUGLink.Name = "btUGLink";
			this.btUGLink.Size = new System.Drawing.Size(33, 33);
			this.btUGLink.TabIndex = 50;
			this.btUGLink.Click += new System.EventHandler(this.btUGLink_Click);
			// 
			// DBbrowser
			// 
			this.Controls.Add(this.btUGLink);
			this.Controls.Add(this.lblSemester);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblSelectedPoints);
			this.Controls.Add(this.btDone);
			this.Controls.Add(this.btRemoveAll);
			this.Controls.Add(this.lblNickName);
			this.Controls.Add(this.tbNickName);
			this.Controls.Add(this.btChooseNone);
			this.Controls.Add(this.btChooseAll);
			this.Controls.Add(this.btRemoveCourse);
			this.Controls.Add(this.lblCourseBasket);
			this.Controls.Add(this.lbCourseBasket);
			this.Controls.Add(this.lblAdvancedSearch);
			this.Controls.Add(this.btAddCourse);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbFaculties);
			this.Controls.Add(this.lbCourses);
			this.Controls.Add(this.labelHours);
			this.Controls.Add(this.tbProjectHours);
			this.Controls.Add(this.labelProjectHours);
			this.Controls.Add(this.tbLabHours);
			this.Controls.Add(this.labelLabHours);
			this.Controls.Add(this.tbTutorialHours);
			this.Controls.Add(this.labelTutorialHours);
			this.Controls.Add(this.tbLectureHours);
			this.Controls.Add(this.labelLectureHours);
			this.Controls.Add(this.labelMoedB);
			this.Controls.Add(this.tbMoedB);
			this.Controls.Add(this.labelMoedA);
			this.Controls.Add(this.tbMoedA);
			this.Controls.Add(this.labelLecturer);
			this.Controls.Add(this.labelAcademicPoints);
			this.Controls.Add(this.tbLecturer);
			this.Controls.Add(this.tbAcademicPoints);
			this.Controls.Add(this.labelFaculty);
			this.Controls.Add(this.tbCourseNum);
			this.Controls.Add(this.labelCourseNum);
			this.Controls.Add(this.LabelCourseName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Name = "DBbrowser";
			this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Size = new System.Drawing.Size(912, 485);
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void lbCourses_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			ListView courses = (ListView) sender;
			
			if (courses.Sorting == SortOrder.Descending)
				courses.Sorting = SortOrder.Ascending;
			else
				courses.Sorting = SortOrder.Descending;
			if (courses.Sorting == SortOrder.Ascending)
			{
				courses.ListViewItemSorter = new ListViewItemComparer(e.Column);
			}
			else
			{
				courses.ListViewItemSorter = new InvListViewItemComparer(e.Column);
			}
			courses.Sort();
		}

        private void cbFaculties_OnClick(object sender, System.EventArgs e)
        {
            mFacultyClicked=true;
        }
		private void lbCourseBasket_OnClick(object sender, System.EventArgs e)
		{
			mCourseBasketClicked=true;
		}
        private void DisplayCourseEvents()
        {
            lvCourseEvents.Items.Clear();
			lvOccurences.Items.Clear();
            CourseEventCollection courseEvents = mCurrentCourse.GetAllCourseEvents();
            foreach (CourseEvent aCourseEvent in courseEvents)
            {
				string[] lv = new String[2];
				lv[0]=aCourseEvent.EventNum.ToString();
				lv[1]=aCourseEvent.Type + " " + aCourseEvent.Giver;
				ListViewItem lvItem; 
				switch (aCourseEvent.Type)
				{
					case "הרצאה": { lvItem = new ListViewItem(lv,0); break; }
					case "מעבדה": { lvItem = new ListViewItem(lv,2); break; }
					case "קבוצה": { lvItem = new ListViewItem(lv,3); break; }
					default: { lvItem = new ListViewItem(lv,1); break; }
				}	 
				lvItem.Tag = aCourseEvent;
				lvItem.Checked=true;
				lvCourseEvents.Items.Add(lvItem);
            }
            if ( lvCourseEvents.Items.Count>0)
                lvCourseEvents.Items[0].Selected=true;
        }


        private void lvCourseEvents_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ListView courseEvents = (ListView)sender;
			if (courseEvents.SelectedIndices.Count>0)
			{
				lvOccurences.Items.Clear();
				CourseEvent selectedEvent = (CourseEvent)(courseEvents.SelectedItems[0].Tag);
				foreach(CourseEventOccurrence ceo in selectedEvent.Occurrences)
				{
					string[] lv = new String[4];
					lv[0]=ceo.Location;
					lv[1]=(ceo.Hour + ceo.Duration).ToString() + ":30";
					lv[2]=ceo.Hour.ToString() + ":30";
					lv[3]=ceo.Day.ToString();
					ListViewItem lvItem = new ListViewItem(lv);
					lvItem.Tag = ceo;
					if (ceo.Selected==true)
					{
						lvItem.Checked=true;
					}
					else
					{
						lvItem.Checked=false;
					}
					lvOccurences.Items.Add(lvItem);
						
				}
			}
        }

		
        public void AddCourseToCourseBasket( Course course )
        {
            lbCourseBasket.Items.Add( course );
            if ( lbCourseBasket.Items.Count > 0)
            {
                lbCourseBasket.SelectedIndex = 0;
            }
        }

        public void RemoveCourseFromeCourseBasket( Course course )
        {
            lbCourseBasket.Items.Remove( course );
            if (lbCourseBasket.Items.Count>0)
            {
                lbCourseBasket.SelectedIndex = 0;
            }
        }

/*	public void RemoveAllFromCourseBasket()
	{
		lbCourseBasket.Items.Clear();
	}*/

		private void btChooseAll_Click(object sender, System.EventArgs e)
		{
			if (lvCourseEvents.Items.Count==0)
				return;
			foreach (ListViewItem lvi in lvCourseEvents.Items)
			{
				lvi.Checked = true;
			}
		}

		private void btChooseNone_Click(object sender, System.EventArgs e)
		{
			if (lvCourseEvents.Items.Count==0)
				return;
			foreach (ListViewItem lvi in lvCourseEvents.Items)
			{
				lvi.Checked = false;
			}
		}

		private void btReverseChecked_Click(object sender, System.EventArgs e)
		{
			if (lvCourseEvents.Items.Count==0)
				return;
			foreach (ListViewItem lvi in lvCourseEvents.Items)
			{
				if (lvi.Checked==true)
					lvi.Checked = false;
				else
					lvi.Checked = true;
			}
		}

		private void lbCourseBasket_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (mCourseBasketClicked == true)
			{
				ListBox cb = (ListBox)sender;
				Course selectedCourse = (Course)cb.SelectedItem;
				if (selectedCourse != null)
					this.Course = selectedCourse;
			}
		}

		private void btUGLink_Click(object sender, System.EventArgs e)
		{
			if (mCurrentCourse==null)
				return;
			string link = "http://ug.technion.ac.il/rishum/mikdet.php?MK=" + mCurrentCourse.Number;
			System.Diagnostics.Process.Start( link );
		}

		public string NickName
		{
			get{ return tbNickName.Text;}
		}
		
	
		private StringCollection mFaculties;
		public StringCollection Faculties
		{
			get{ return mFaculties; }
			set {
				mFaculties = value;
				searchControl1.Faculties = value;
				cbFaculties.Items.Clear();
				foreach (string faculty in mFaculties)
				{
					cbFaculties.Items.Add(faculty);
				}
			}
		}

		public IList CheckedCourseEvents {
			get {
				ArrayList list = new ArrayList();
				foreach (ListViewItem item in lvCourseEvents.CheckedItems)
				{
					list.Add(item.Tag);
				}
				return list;
			}
		}

		private CourseIDCollection mCourses;
		public CourseIDCollection Courses {
			get { return mCourses; }
			set {
				mCourses = value;
				lbCourses.Items.Clear();
				foreach (CourseID aCourse in mCourses)
				{
					string[] lv = new String[2];
					lv[1]=aCourse.CourseName;
					lv[0]=aCourse.CourseNumber;
					ListViewItem lvItem = new ListViewItem(lv);
					lvItem.Tag = aCourse;
					lbCourses.Items.Add(lvItem);
				}
			}
		}

		// FIXME
		/*public ListView Occurrences
		{
			get{ return lvOccurences; }
		}

		public ListBox CourseBasket
		{
			get{ return lbCourseBasket; }
		}
		public ListView CourseEvents
		{
			get{ return lvCourseEvents; }
		}
		public Button   AddCourse
		{
			get { return btAddCourse; }
		}
		public Button   RemoveAll
		{
			get { return btRemoveAll; }
		}
		public Button   Done
		{
			get { return btDone; }
		}
		public Button   Link
		{
			get { return btUGLink; }
		}
		public TextBox  CourseNumber
		{
			get { return tbCourseNum; }
		}*/
		public string  SelectedPoints
		{
			get { return lblSelectedPoints.Text; }
			set { lblSelectedPoints.Text = value; }
		}
		public string  Semester
		{
			get { return lblSemester.Text; }
			set { lblSemester.Text = value; }
		}
		public bool FacultyClicked
		{
			get {return mFacultyClicked; }
            set { mFacultyClicked = value; }
		}
        public Course Course
        {
            get{ return mCurrentCourse; }
			set
			{ 
				mCurrentCourse = value; 
				if ( mCurrentCourse != null )
				{
					tbAcademicPoints.Text = mCurrentCourse.AcademicPoints.ToString();
					tbLabHours.Text       = mCurrentCourse.LabHours.ToString();
					tbLectureHours.Text   = mCurrentCourse.LectureHours.ToString();
					tbLecturer.Text       = mCurrentCourse.Lecturer;
					if (mCurrentCourse.MoadA != DateTime.MinValue)
						tbMoedA.Text          = mCurrentCourse.MoadA.ToShortDateString();
					else
						tbMoedA.Text	  = string.Empty;
					if (mCurrentCourse.MoadB != DateTime.MinValue)
						tbMoedB.Text          = mCurrentCourse.MoadB.ToShortDateString();
					else
						tbMoedB.Text	  = string.Empty;
					tbCourseNum.Text      = mCurrentCourse.Number;
					tbProjectHours.Text   = mCurrentCourse.ProjecHours.ToString();
					tbTutorialHours.Text  = mCurrentCourse.TutorialHours.ToString();
					cbFaculties.SelectedItem= mCurrentCourse.Faculty;
					tbNickName.Text		  = mCurrentCourse.NickName;
					DisplayCourseEvents();
					tbNickName.Enabled=true;
				}
				else
				{
					tbAcademicPoints.Text = string.Empty;
					tbLabHours.Text       = string.Empty;
					tbLectureHours.Text   = string.Empty;
					tbLecturer.Text       = string.Empty;
					tbMoedA.Text          = string.Empty;
					tbMoedB.Text          = string.Empty;
					tbCourseNum.Text      = string.Empty;
					tbProjectHours.Text   = string.Empty;
					tbTutorialHours.Text  = string.Empty;
					tbNickName.Text		  = string.Empty;
					lvCourseEvents.Items.Clear();
					lvOccurences.Items.Clear();
				}
            }
        }

	public Course CurrentBasketCourse
	{
		get { return lbCourseBasket.SelectedItem as Course; }
	}

	public string SelectedFaculty {
		get { return cbFaculties.SelectedItem; }
	       	set { cbFaculties.SelectedItem = value; }
	}	

	public CourseID[] SelectedCourseID {
		get {
			if (lvCourses.SelectedIndices.Count() > 0) {
				ListViewItem lvItem = lvCourses.Items[ lvCourses.SelectedIndices[0]];
				CourseID theCourseID = (CourseID)(lvItem.Tag);
				return new CourseID[1]{theCourseID};
			}
			else
				return new CourseID[0];
		}
	}

	public CoursesList CourseBasket {
		get {
			CoursesList list = new CoursesList();
			foreach (Course c in lbCourseBasket.Items)
			{
				list.Add(c);
			}
			return list;
		}
		set {
			lbCourseBasket.Items.Clear();
			foreach (Course c in value)
			{
				AddCourseToCourseBasket(c);
			}
		}
	}

        public SearchControl SearchControl
        {
            get{ return searchControl1; }
        }

    	public event EventHandler RemoveCourseClick
	{
		add { 
			btRemoveCourse.Click += value;
			lbCourseBasket.DoubleClick += value;
	       	}
		remove { 
			btRemoveCourse.Click -= value;
			lbCourseBasket.DoubleClick -= value;
		}
	}

    	public event EventHandler AddCourseClick
	{
		add { 
			btAddCourse.Click += value;
			lbCourses.DoubleClick += value;
	       	}
		remove { 
			btAddCourse.Click -= value;
			lbCourses.DoubleClick -= value;
		}
	}

	public event EventHandler SelectedFacultyChanged
	{
		add { 
			cbFaculties.SelectedIndexChanged += value;
			cbFaculties.TextChanged += value;
		}
		remove {
			cbFaculties.SelectedIndexChanged -= value;
			cbFaculties.TextChanged -= value;
		}
	}
	
	public event EventHandler SelectedCourseChanged
	{
		add { 
			lbCourses.SelectedIndexChanged += value;
		}
		remove {
			lbCourses.SelectedIndexChanged -= value;
		}
	}

	public event EventHandler OccurrencesFocusOut
	{
		add { lvOccurences.Validated += value; }
	       	remove { lvOccurences.Validated -= value; }
	}

	public event EventHandler DoneClick
	{
		add { btDone.Click += value; }
		remove { btDone.Click -= value; }
	}
	
	public event EventHandler RemoveAllClick 
	{
		add { btRemoveAll.Click += value; }
		remove { btRemoveAll.Click -= value; }
	}
    }
}
