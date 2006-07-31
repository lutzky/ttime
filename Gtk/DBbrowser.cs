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
		TreeViewColumn chRegNum, chEvent;
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

			// tvCourses stuff
			numberColumn = new TreeViewColumn();
			nameColumn = new TreeViewColumn();
			numberColumn.Title = "מס' קורס";
			nameColumn.Title = "שם הקורס";
			storeCourses = new ListStore(typeof(string),typeof(string));
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
			storeOccurences = new ListStore(typeof(string),typeof(string),typeof(string),typeof(string));
			tvOccurences.AppendColumn(chDay);
			tvOccurences.AppendColumn(chFrom);
			tvOccurences.AppendColumn(chUntil);
			tvOccurences.AppendColumn(chLocation);
			tvOccurences.Model = storeOccurences;
			
			// tvCourseEvents stuff
			chEvent = new TreeViewColumn();
			chRegNum = new TreeViewColumn();
			chEvent.Title = "קבוצה";
			chRegNum.Title = "מספר";
			storeCourseEvents = new ListStore(typeof(string),typeof(string));
			tvCourseEvents.AppendColumn(chEvent);
			tvCourseEvents.AppendColumn(chRegNum);
			tvCourseEvents.Model = storeCourseEvents;

			// tvCourseBasket stuff
			chBasketNumber = new TreeViewColumn();
			chBasketName = new TreeViewColumn();
			chBasketNumber.Title = "מס' קורס";
			chBasketName.Title = "שם הקורס";
			storeCourseBasket = new ListStore(typeof(string), typeof(string));
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
			// set TODO
		}

#region Events
#endregion

#region Properties
#endregion
/*		
		public static void Main()
		{
			Application.Init();
			Window win = new Window("Hello World");
			win.Resize(800,600);
			Widget w = new DBbrowser();
			win.Add(w);
			win.ShowAll();
			Application.Run();
		}
		*/
	}
	
}
