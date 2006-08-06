//
using System;
using System.Collections;
using System.Collections.Specialized;
using Gtk;
using GtkSharp;
using Glade;
using Gdk;

using UDonkey.Logic;

namespace UDonkey.GUI
{
	public class DBbrowser 
	{
		private Widget mMainWidget;
		private UDonkey.GUI.SearchControl mSearchControl;

#region Glade Widgets
		[Widget] ComboBoxEntry cbFaculties;
		[Widget] Entry entrySearch;
		[Widget] Button btFind;
		[Widget] Button btAdvancedSearch;

		[Widget] Label lblSemester;
		[Widget] TreeView tvCourses;
		[Widget] Button btAddCourse;
		[Widget] Button btDone;

		[Widget] Label lblCourseBasket;
		[Widget] TreeView tvCourseBasket;
		[Widget] Button btRemoveCourse;
		[Widget] Button btRemoveAll;

		[Widget] Entry tbAcademicPoints;
		[Widget] Entry tbMoedA;
		[Widget] Entry tbMoedB;
		[Widget] Entry tbLecturer;
		[Widget] Entry tbLectureHours;
		[Widget] Entry tbTutorialHours;
		[Widget] Entry tbLabHours;
		[Widget] Entry tbProjectHours;

		[Widget] TreeView tvCourseEvents;
		[Widget] Button btChooseAll;
		[Widget] Button btChooseNone;
		[Widget] Button btReverseChecked;
		[Widget] TreeView tvOccurences;
#endregion
		// members of tvCourses
		TreeViewColumn nameColumn, numberColumn; 
		ListStore storeCourses;
		CourseIDCollection mCourses;

		// members of tvOccurences
		TreeViewColumn chLocation, chUntil, chFrom, chDay;
		ListStore storeOccurences;

		// members of tvCourseEvents
		TreeViewColumn chCourseEventCheckBox, 
			       chRegNum, chEvent, chGivenBy;
		ListStore storeCourseEvents;
		Pixbuf PixbufLab, PixbufProject, PixbufTutorial, PixbufLecture;

		// members of tvCourseBasket
		TreeViewColumn chBasketName, chBasketNumber;
		ListStore storeCourseBasket;

		// members of cbFaculties
		StringCollection mFaculties;
		ListStore storeFaculties;

		string strSelectedPoints, strSemester;

		Course mCurrentCourse;

		public DBbrowser() 
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "DBbrowser", null); 
			gxml.Autoconnect (this);
			mMainWidget = gxml.GetWidget("DBbrowser");

			CellRenderer cr;
			
			// tvCourses stuff
			numberColumn = new TreeViewColumn();
			numberColumn.Title = "מס' קורס";
			cr = new CellRendererText();
			numberColumn.PackStart(cr, true);
			numberColumn.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseIDNumber));
			nameColumn = new TreeViewColumn();
			nameColumn.Title = "שם הקורס";
			cr = new CellRendererText();
			nameColumn.PackStart(cr, true);
			nameColumn.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseIDName));
			
			storeCourses = new ListStore(typeof (UDonkey.Logic.CourseID));
			tvCourses.AppendColumn(numberColumn);
			tvCourses.AppendColumn(nameColumn); 
			tvCourses.Model = storeCourses;

			// tvOccurences stuff
			chLocation = new TreeViewColumn();
			chUntil = new TreeViewColumn();
			chFrom = new TreeViewColumn();
			chDay = new TreeViewColumn();
			chLocation.Title = "מיקום";
			chUntil.Title = "עד שעה";
			chFrom.Title = "משעה";
			chDay.Title = "יום";
			storeOccurences = new ListStore(typeof (UDonkey.Logic.Course));
			tvOccurences.AppendColumn(chDay);
			tvOccurences.AppendColumn(chFrom);
			tvOccurences.AppendColumn(chUntil);
			tvOccurences.AppendColumn(chLocation);
			tvOccurences.Model = storeOccurences;
			
			// tvCourseEvents stuff

			PixbufLecture = new Pixbuf(null, "lecture.bmp");
			PixbufLab = new Pixbuf(null, "lab.bmp");
			PixbufTutorial = new Pixbuf(null, "tutorial.bmp");
			PixbufProject = new Pixbuf(null, "project.bmp");
			
			chCourseEventCheckBox = new TreeViewColumn();
			cr = new CellRendererToggle();
			(cr as CellRendererToggle).Toggled += new ToggledHandler(OnCourseEventToggle);

			chCourseEventCheckBox.PackStart(cr, false);
			chCourseEventCheckBox.AddAttribute(cr, "active", 1);
			
			cr = new CellRendererPixbuf();
			chCourseEventCheckBox.PackStart(cr, false);
			chCourseEventCheckBox.SetCellDataFunc(cr, new TreeCellDataFunc (RenderCourseEventIcon));
			
			chEvent = new TreeViewColumn();
			chEvent.Title = "קבוצה";
			cr = new CellRendererText();
			chEvent.PackStart(cr, true);
			chEvent.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseEventType));
			
			chGivenBy = new TreeViewColumn();
			chGivenBy.Title = "ניתן ע\"י";
			cr = new CellRendererText();
			chGivenBy.PackStart(cr, true);
			chGivenBy.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseEventGivenBy));

			chRegNum = new TreeViewColumn();
			chRegNum.Title = "מספר";
			cr = new CellRendererText();
			chRegNum.PackStart(cr, true);
			chRegNum.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseEventNum));

			storeCourseEvents = new ListStore(typeof (UDonkey.Logic.CourseEvent), typeof(bool));
			
			tvCourseEvents.AppendColumn(chCourseEventCheckBox);
			tvCourseEvents.AppendColumn(chEvent);
			tvCourseEvents.AppendColumn(chGivenBy);
			tvCourseEvents.AppendColumn(chRegNum);
			tvCourseEvents.Model = storeCourseEvents;

			// tvCourseBasket stuff
			chBasketNumber = new TreeViewColumn();
			chBasketNumber.Title = "מס' קורס";
			cr = new CellRendererText();
			chBasketNumber.PackStart(cr, true);
			chBasketNumber.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseNumber));
			chBasketName = new TreeViewColumn();
			chBasketName.Title = "שם הקורס";
			cr = new CellRendererText();
			chBasketName.PackStart(cr, true);
			chBasketName.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseName));
			storeCourseBasket = new ListStore(typeof (UDonkey.Logic.Course));
			tvCourseBasket.AppendColumn(chBasketNumber);
			tvCourseBasket.AppendColumn(chBasketName);
			tvCourseBasket.Model = storeCourseBasket;

			// cbFaculties stuff
			storeFaculties = new ListStore(typeof (string));
			cbFaculties.Model = storeFaculties;
			cbFaculties.TextColumn = 0;

			// SearchControl
			mSearchControl = new UDonkey.GUI.SearchControl();
		}

		public static implicit operator Widget(DBbrowser dbb)
		{
			return dbb.mMainWidget; 
		}

		public void AddCourseToCourseBasket(Course course)
		{
			storeCourseBasket.AppendValues(course);
		}

		public void RemoveCourseFromCourseBasket( Course course )
		{
			TreeIter iter;
			storeCourseBasket.GetIterFirst(out iter);
			do			{
				Course c = (Course)storeCourseBasket.GetValue(iter, 0);
				if (c == course)
				{
					storeCourseBasket.Remove(ref iter);
					return;
				}
			}
			while (storeCourseBasket.IterNext(ref iter));

		}

/*		public void RemoveAllFromCourseBasket()
		{
			storeCourseBasket.Clear();
		}*/

#region Properties
		public string NickName
		{
			get { return null; } // TODO
		}

		public string CourseNumber
		{
			get { return null; } // TODO
		}

		public string SelectedPoints
		{
			get { return strSelectedPoints; }
			set {
				strSelectedPoints = value;
				lblCourseBasket.Text = "סל הקורסים שנבחרו (" + value + ")נקודות";
			}
		}

		public string Semester
		{
			get { return strSemester; }
			set {
				strSemester = value;
				lblSemester.Text = "רשימת הקורסים " + value;
			}
		}

		public bool FacultyClicked
		{
			get { return true; }
			set {}
		}

		public Course Course
		{
			get { return mCurrentCourse; }
			set {
				mCurrentCourse = value;
				if (mCurrentCourse != null)
				{
					tbAcademicPoints.Text = mCurrentCourse.AcademicPoints.ToString();
                                        tbLabHours.Text       = mCurrentCourse.LabHours.ToString();
                                        tbLectureHours.Text   = mCurrentCourse.LectureHours.ToString();
                                        tbLecturer.Text       = mCurrentCourse.Lecturer;
                                        if (mCurrentCourse.MoadA != DateTime.MinValue)
                                                tbMoedA.Text          = mCurrentCourse.MoadA.ToShortDateString();
                                        else
                                                tbMoedA.Text      = string.Empty;
                                        if (mCurrentCourse.MoadB != DateTime.MinValue)
                                                tbMoedB.Text          = mCurrentCourse.MoadB.ToShortDateString();
                                        else
                                                tbMoedB.Text      = string.Empty;
                                        //tbCourseNum.Text      = mCurrentCourse.Number;
                                        tbProjectHours.Text   = mCurrentCourse.ProjecHours.ToString();
                                        tbTutorialHours.Text  = mCurrentCourse.TutorialHours.ToString();
                                        cbFaculties.Entry.Text = mCurrentCourse.Faculty;
                                        //tbNickName.Text           = mCurrentCourse.NickName;
                                        DisplayCourseEvents();
                                        //tbNickName.Enabled=true;
                                }
                                else                                 {
                                        tbAcademicPoints.Text = string.Empty;
                                        tbLabHours.Text       = string.Empty;
					tbLectureHours.Text   = string.Empty;
                                        tbLecturer.Text       = string.Empty;
                                        tbMoedA.Text          = string.Empty;
                                        tbMoedB.Text          = string.Empty; 
					//tbCourseNum.Text      = string.Empty;
                                        tbProjectHours.Text   = string.Empty;
                                        tbTutorialHours.Text  = string.Empty;
                                        //tbNickName.Text           = string.Empty;
                                        //lvCourseEvents.Items.Clear();
                                        //lvOccurences.Items.Clear();
                                }
			}
		}

		public Course CurrentBasketCourse
		{
			get {
				TreePath path;
				TreeViewColumn col;
				TreeIter iter;
				tvCourseBasket.GetCursor(out path, out col);
				if (storeCourseBasket.GetIter(out iter, path))
					return (Course)storeCourseBasket.GetValue(iter, 0);
				else
					return null;
			}
		}

		public StringCollection Faculties {
			get { return mFaculties; }
			set {
				mFaculties = value;
				mSearchControl.Faculties = value;
				storeFaculties.Clear();
				foreach (string faculty in mFaculties)
				{
					storeFaculties.AppendValues(faculty);
				}
			}
		}

		public IList CheckedCourseEvents {
			get {
				ArrayList list = new ArrayList();
				foreach (object[] row in storeCourseEvents)
				{
					if ((bool)row[1])
						list.Add((CourseEvent)row[0]);
				}
				return list;
			}
		}

		public CourseIDCollection Courses {
			get { return mCourses; }
			set {
				mCourses = value;
				storeCourses.Clear();
				foreach (CourseID course in mCourses)
				{
					storeCourses.AppendValues(course);
				}
			}
		}

		public string SelectedFaculty {
			get { return cbFaculties.Entry.Text; }
			set { cbFaculties.Entry.Text = value; }
		}

		public CourseID[] SelectedCourseID {
			get { 
				TreeIter iter;
				if (tvCourses.Selection.GetSelected(out iter))
				{
					CourseID c = (CourseID) storeCourses.GetValue(iter, 0);
					return new CourseID[1]{c};
				}
				else
					return new CourseID[0];
			}
		}
		
		public CourseEvent SelectedCourseEvent {
			get { 
				TreeIter iter;
				if (tvCourseEvents.Selection.GetSelected(out iter))
				{
					return (CourseEvent) storeCourseEvents.GetValue(iter, 0);
				}
				else
					return null;
			}
		}

		public CoursesList CourseBasket
		{
			get {
				CoursesList list = new CoursesList();
				foreach (object[] row in storeCourseBasket)
				{
					Course c = (Course)row[0];
					list.Add(c.Number, c);
				}
				return list;
			}

			set {
				storeCourseBasket.Clear();
				if (value != null)
				{
					foreach (Course c in value)
					{
						storeCourseBasket.AppendValues(c);
					}
				}
			}
		}

		public UDonkey.GUI.SearchControl SearchControl 
		{
			get { return mSearchControl; }
		}
#endregion
		

#region Event handlers
		private void OnCourseEventToggle(object obj, ToggledArgs args)
		{

			TreeIter iter;
			storeCourseEvents.GetIter(out iter, new TreePath(args.Path));

			storeCourseEvents.SetValue(iter, 1,
				!(bool)storeCourseEvents.GetValue(iter, 1));
		}

		private void on_button_press_event(object obj, ButtonPressEventArgs args)
		{
			if (args.Event.Type == EventType.TwoButtonPress) {
				if (obj == tvCourses) 
					mAddCourseEvent(this, new EventArgs());
				else if (obj == tvCourseBasket) 
					mRemoveCourseEvent(this, new EventArgs());
			}
		}

		private void on_tvOccurences_focus_out_event(object obj, FocusOutEventArgs args)
		{
			mOccurrencesFocusOut(obj, args);
		}

		private void on_btAdvancedSearch_clicked(object obj, EventArgs args)
		{
			mSearchControl.Show();
		}
#endregion

#region Event 
		private EventHandler mRemoveCourseEvent;
		public event EventHandler RemoveCourseClick
		{
			add { 
				mRemoveCourseEvent += value; 
				btRemoveCourse.Clicked += value;
			} 
			remove { 
				mRemoveCourseEvent -= value;
				btRemoveCourse.Clicked -= value;
		       	}
		}

		public event EventHandler SelectedFacultyChanged
		{
			add { 
				cbFaculties.EditingDone += value;
				cbFaculties.Changed += value;
			}
			remove {
				cbFaculties.EditingDone -= value;
				cbFaculties.Changed -= value;
			}
		}
		
		public event EventHandler SelectedCourseChanged
		{
			add { 
				tvCourses.Selection.Changed += value;
			}
			remove {
				tvCourses.Selection.Changed -= value;
			}
		}

		public event EventHandler Load
		{
			add { mMainWidget.Realized += value; }
			remove { mMainWidget.Realized -= value; }
		}
		
		public event EventHandler VisibleChanged 
		{
			add { mMainWidget.Shown += value; }
			remove { mMainWidget.Shown -= value; }
		}
		
		private EventHandler mAddCourseEvent;
		public event EventHandler AddCourseClick
		{
			add { 
				mAddCourseEvent += value;
				btAddCourse.Clicked += value; 
			}
			remove {
			        mAddCourseEvent -= value;	
				btAddCourse.Clicked -= value; 
			}
		}

		private EventHandler mOccurrencesFocusOut;
		public event EventHandler OccurrencesFocusOut
		{
			add { mOccurrencesFocusOut += value; }
			remove { mOccurrencesFocusOut -= value; }
		}

		public event EventHandler DoneClick
		{
			add { btDone.Clicked += value; }
			remove { btDone.Clicked -= value; }
		}
		
		public event EventHandler RemoveAllClick
		{
			add { btRemoveAll.Clicked += value; }
			remove { btRemoveAll.Clicked -= value; }
		}

		public event EventHandler CourseNumberChanged;
#endregion

		// courseID renderers
		private void RenderCourseIDName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseID course = (CourseID) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = course.CourseName;
		}
		private void RenderCourseIDNumber(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseID course = (CourseID) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = course.CourseNumber;
		}
		
		// Course renderers
		private void RenderCourseName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			Course course = (Course) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = course.Name;
		}
		private void RenderCourseNumber(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			Course course = (Course) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = course.Number;
		}
		
		// CourseEvent renderers
		private void RenderCourseEventNum(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseEvent ev = (CourseEvent) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = ev.EventNum.ToString();
		}
		private void RenderCourseEventGivenBy(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseEvent ev = (CourseEvent) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = ev.Giver;
		}
		private void RenderCourseEventType(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseEvent ev = (CourseEvent) model.GetValue(iter, 0);
			(cell as CellRendererText).Text = ev.Type;
		}
		private void RenderCourseEventIcon(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CourseEvent ev = (CourseEvent) model.GetValue(iter, 0);
			CellRendererPixbuf cellp = (CellRendererPixbuf) cell;
			switch (ev.Type)
			{
				case "הרצאה":
					cellp.Pixbuf = PixbufLecture;
					break;
				case "מעבדה":
					cellp.Pixbuf = PixbufLab;
					break;
				case "קבוצה":
					cellp.Pixbuf = PixbufTutorial;
					break;
				default:
					cellp.Pixbuf = PixbufProject;
					break;
			}		
		}
		
		private void DisplayCourseEvents()
		{
			storeCourseEvents.Clear();
			storeOccurences.Clear();
		    
			CourseEventCollection courseEvents = mCurrentCourse.GetAllCourseEvents();
		    	foreach (CourseEvent aCourseEvent in courseEvents)
		    	{
				storeCourseEvents.AppendValues(aCourseEvent, true);
		    	}
/* TODO		    	if ( lvCourseEvents.Items.Count>0)
			lvCourseEvents.Items[0].Selected=true;*/

		}

		public void AddCourse(Course c)
		{
			AddCourseToCourseBasket(c);
			AddCourseToCourseBasket(c);
			AddCourseToCourseBasket(c);
			RemoveCourseFromCourseBasket(c);

			this.Course = c;
		}

/*		public static void Main()
		{
			Application.Init();
			Gtk.Window win = new Gtk.Window("Hello World");
			win.Resize(800,600);
			DBbrowser dbb = new DBbrowser();

			win.Add(dbb);
			win.ShowAll();

			UDonkey.DB.CourseDB cdb = new UDonkey.DB.CourseDB();
			cdb.Load("MainDB.xml");

			dbb.Faculties = cdb.GetFacultyList();
			dbb.Courses = cdb.GetCoursesByFacultyName("מדעי המחשב");
			dbb.AddCourse(cdb.GetCourseByNumber("046267"));
			Application.Run();
		}*/
		
	}
	
}
