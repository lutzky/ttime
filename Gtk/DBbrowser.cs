//
using System;
using Gtk;
using GtkSharp;
using Glade;

using UDonkey.Logic;

namespace UDonkey.GUI
{
	public class DBbrowser 
	{
		private Widget mMainWidget;
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

		// members of tvOccurences
		TreeViewColumn chLocation, chUntil, chFrom, chDay;
		ListStore storeOccurences;

		// members of tvCourseEvents
		TreeViewColumn chCourseEventCheckBox, chRegNum, chEvent, chGivenBy;
		ListStore storeCourseEvents;

		// members of tvCourseBasket
		TreeViewColumn chBasketName, chBasketNumber;
		ListStore storeCourseBasket;

		string strSelectedPoints, strSemester;

		Course mCurrentCourse;

		public DBbrowser() 
		{
			Glade.XML gxml = new Glade.XML("udonkey.glade", "DBbrowser", null); 
			//Glade.XML gxml = Glade.XML.FromAssembly("udonkey.glade", "ConfigControl", null);
			gxml.Autoconnect (this);
			mMainWidget = gxml.GetWidget("DBbrowser");

			CellRenderer cr;
			
			// tvCourses stuff
			numberColumn = new TreeViewColumn();
			numberColumn.Title = "מס' קורס";
			cr = new CellRendererText();
			numberColumn.PackStart(cr, true);
			numberColumn.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseNumber));
			nameColumn = new TreeViewColumn();
			nameColumn.Title = "שם הקורס";
			cr = new CellRendererText();
			nameColumn.PackStart(cr, true);
			nameColumn.SetCellDataFunc( cr, new TreeCellDataFunc (RenderCourseName));
			
			storeCourses = new ListStore(typeof (UDonkey.Logic.Course));
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
			chCourseEventCheckBox = new TreeViewColumn();
			cr = new CellRendererToggle();
			(cr as CellRendererToggle).Toggled += new ToggledHandler(OnCourseEventToggle);

			chCourseEventCheckBox.PackStart(cr, false);
			chCourseEventCheckBox.AddAttribute(cr, "active", 1);
			
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
		}

		public static implicit operator Widget(DBbrowser dbb)
		{
			return dbb.mMainWidget; 
		}

		public void AddCourseToCourseBasket(Course course)
		{
			// TODO
		}

		public void RemoveCourseFromeCourseBasket( Course course )
		{
			// TODO
		}

		public string NickName
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
				lblSemester.Text = "רשימת הקורסים נכון לסמסטר " + value;
			}
		}

		public bool FacultyClicked
		{
			//FIXME is this really necessary?
			get { return false; }
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

#region Events
		private void OnCourseEventToggle(object obj, ToggledArgs args)
		{

			TreeIter iter;
			storeCourseEvents.GetIter(out iter, new TreePath(args.Path));

			storeCourseEvents.SetValue(iter, 1,
				!(bool)storeCourseEvents.GetValue(iter, 1));
		}
#endregion

#region Properties
#endregion

		// course renderers
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
		
		private void DisplayCourseEvents()
		{
			storeCourseEvents.Clear();
			storeOccurences.Clear();
		    
			CourseEventCollection courseEvents = mCurrentCourse.GetAllCourseEvents();
		    	foreach (CourseEvent aCourseEvent in courseEvents)
		    	{
				storeCourseEvents.AppendValues(aCourseEvent, true);
				/*string[] lv = new String[2];
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
					*/
		    	}
/* TODO		    	if ( lvCourseEvents.Items.Count>0)
			lvCourseEvents.Items[0].Selected=true;*/

		}

		public void AddCourse(Course c)
		{
			storeCourses.AppendValues(c);
			storeCourseBasket.AppendValues(c);

			this.Course = c;
		}

		public static void Main()
		{
			Application.Init();
			Window win = new Window("Hello World");
			win.Resize(800,600);
			DBbrowser dbb = new DBbrowser();

			win.Add(dbb);
			win.ShowAll();

			UDonkey.DB.CourseDB cdb = new UDonkey.DB.CourseDB();
			cdb.Load("MainDB.xml");

			dbb.AddCourse(cdb.GetCourseByNumber("046267"));
			Application.Run();
		}
		
	}
	
}
