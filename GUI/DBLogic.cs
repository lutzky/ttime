using System;
using System.Collections;
using System.Windows.Forms;
using UDonkey.Logic;
using UDonkey.DB;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for DBBrowserLogic.
	/// </summary>
	public class DBLogic
	{
		// Varibles
		private CourseDB mCourseDB;
		private CoursesScheduler mCoursesScheduler;
		private MainFormLogic mMainFormLogic;
		private const string RESOURCES_GROUP = "SearchTab";
		//GUI
		private SearchControl mSearchControl;
		private DBbrowser mDBBrowser;
	    #region Constructor
		public DBLogic(CourseDB courseDB, CoursesScheduler coursesScheduler, MainFormLogic mainFormLogic)
		{
			mCourseDB		               = courseDB;
			mCoursesScheduler              = coursesScheduler;
			mMainFormLogic				   = mainFormLogic;
		}
		
		#endregion Constructor
		#region Public Method
		public void AddCourse( Course course )
		{
	            	ICollection col = mCoursesScheduler.FindExamsOverlaps( course, true );
			if ( col.Count != 0 )
			{//If there are Exams overlaps
				string err = string.Format(Resources.String( RESOURCES_GROUP, "ExamOverlapMessage1" ),
					course.NickName	);
				
				foreach( Course c in col )
				{
					TimeSpan span = ( course.MoadA > c.MoadA )?
						course.MoadA - c.MoadA :
						c.MoadA - course.MoadA ;
					err += string.Format(Resources.String(RESOURCES_GROUP, "ExamOverlapMessage2" ),
						c.NickName,
						span.Days);
				}
				err += string.Format(Resources.String(RESOURCES_GROUP, "ExamOverlapMessage3" ),
						course.NickName);

				DialogResult result = 
					MessageBox.Show( null, err, Resources.String(RESOURCES_GROUP, "ExamOverlapMessage4" ), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
				if ( result == DialogResult.Cancel )
				{
					return;
				}
			}

			if ( AddCourseToScheduler( course ) )
			{
				mDBBrowser.AddCourseToCourseBasket( course );
				// Add course's points to point counter
				float totalPoints = float.Parse(mDBBrowser.SelectedPoints);
				totalPoints += course.AcademicPoints;
				mDBBrowser.SelectedPoints = totalPoints.ToString();
			}
		}
		public void RemoveCourse( Course course )
		{
			mCoursesScheduler.Courses.Remove(course.Number);
			mDBBrowser.RemoveCourseFromCourseBasket( course );
			// Remove course's points from point counter
			float totalPoints = float.Parse(mDBBrowser.SelectedPoints);
			totalPoints -= course.AcademicPoints;
			mDBBrowser.SelectedPoints = totalPoints.ToString();
			mMainFormLogic.SetStatusBarLine(course.NickName + Resources.String(RESOURCES_GROUP, "CourseRemoveMessage" ));
		}
		public void RemoveAllCourses()
		{
			mCoursesScheduler.Courses.Clear(); // Clear course basket
			mDBBrowser.CourseBasket = null; // Clear course basket display
			mDBBrowser.Course = null; // Clear current course
			mDBBrowser.SelectedPoints="0"; // Clear points counter
		}

		public void Load()
		{
			SearchControl_Load(this, new System.EventArgs());
			DBBrowser_Load(this, new System.EventArgs());
		}
		#endregion Public Method
		#region Events
		private void Search(object sender, System.EventArgs e)
		{
			if ( mDBBrowser != null )
			{
				SearchEventArgs args = e as SearchEventArgs;
				CourseIDCollection courses =  mCourseDB.SearchDBByParam(
					args.Name,
					args.Number,
					args.Points,
					args.Faculty,
					args.Lecturer,
					args.Days);
				mDBBrowser.Courses = courses;
				mMainFormLogic.SetStatusBarLine(Resources.String(RESOURCES_GROUP, "CourseFoundMessage1" ) + courses.Count.ToString() + Resources.String(RESOURCES_GROUP, "CourseFoundMessage2" ));
			}
		}

		private void SearchControl_Load(object sender, EventArgs e)
		{				
			mSearchControl.Faculties = mCourseDB.GetFacultyList();
		}

		private void DBBrowser_Load(object sender, EventArgs e)
		{
			System.Collections.Specialized.StringCollection faculties=mCourseDB.GetFacultyList();
			mDBBrowser.Faculties = faculties;

			foreach (Course aCourse in mCoursesScheduler.Courses.Values)
			{
				this.mDBBrowser.AddCourseToCourseBasket( aCourse );
			}
			this.mDBBrowser.Semester = "נכון לסמסטר " + this.mCourseDB.GetSemesterInfo();
		}

		private void AddCourse_Click(object sender, System.EventArgs e)
		{
			Course curr = mDBBrowser.Course;
			if( curr == null )
			{
				return; 
			}
			Course temp = new Course();
			temp.Name = curr.Name;
			temp.NickName = mDBBrowser.NickName;
			temp.Number = curr.Number;
			temp.Lecturer = curr.Lecturer;
			temp.Faculty = curr.Faculty;
			temp.AcademicPoints = curr.AcademicPoints;
			temp.LectureHours = curr.LectureHours;
			temp.TutorialHours = curr.TutorialHours;
			temp.LabHours = curr.LabHours;
			temp.ProjecHours = curr.ProjecHours;
			temp.MoadA = curr.MoadA;
			temp.MoadB = curr.MoadB;
			temp.RegistrationGroups = curr.RegistrationGroups;

			foreach (object item in DBBrowser.CheckedCourseEvents)
			{
				CourseEvent courseEvent = (CourseEvent)(item);
				courseEvent.Course = temp;
				CourseEvent ce = temp.FindEvent(courseEvent); // Check if the same event already exists
				if (ce==null) // If the new event does not exist already in the course. We add the event.
				{
					switch (courseEvent.Type)
					{
						case "הרצאה": { temp.Lectures.Add(courseEvent.EventNum, courseEvent); break; }
						case "מעבדה": { temp.Labs.Add(courseEvent.EventNum, courseEvent); break; }
						case "קבוצה": { temp.Projects.Add(courseEvent.EventNum, courseEvent); break; }
						default: { temp.Tutorials.Add(courseEvent.EventNum, courseEvent); break; }
					}	 
				}
				else // An identical event exists.  We only concat the locations.
				{
					int i;
					for (i=0 ; i<ce.Occurrences.Count ; i++)
					{
						CourseEventOccurrence event1 = (CourseEventOccurrence)(ce.Occurrences[i]);
						CourseEventOccurrence event2 = (CourseEventOccurrence)(courseEvent.Occurrences[i]);
						if (event2.Location.Length>0) // Only concat location if it exists
						{
							if (event1.Location.Length>0)
								event1.Location += ("/" + event2.Location);
							else
								event1.Location = event2.Location;
						}
					}
				}
			}
			
			AddCourse( temp );
		}

		private void RemoveCourse_Click(object sender, System.EventArgs e)
		{			
			Course course = mDBBrowser.CurrentBasketCourse;
			if ( course != null )
			{
				this.RemoveCourse( course );
			}
		}

		private void CourseNumerTextChanged(object sender, System.EventArgs e)
		{
			if ((mDBBrowser.CourseNumber.Length==6)
				&&((DBBrowser.Course==null)
				||(mDBBrowser.CourseNumber != DBBrowser.Course.Number)))
			{
				Course course = mCourseDB.GetCourseByNumber( mDBBrowser.CourseNumber );

				if ( course != null)
				{
					if (course != DBBrowser.Course)
					{
						DBBrowser.Course = course;
					}
				
					if (!(CoursesListViewContains(course.Number)))
					{
						CourseIDCollection courses = new CourseIDCollection();
						CourseID aCourseID = new CourseID();
						aCourseID.CourseName = course.Name;
						aCourseID.CourseNumber = course.Number;
						courses.Add(aCourseID);
						mDBBrowser.Courses = courses;
						/* FIXME mDBBrowser.Courses.Items[0].Selected = true;
						if (mDBBrowser.CourseEvents.Items.Count>0)
							mDBBrowser.CourseEvents.Items[0].Selected = true;  */
					}
				
				
				} 
			}
		}
		private void Faculties_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Console.WriteLine("DBLogic.SelectedFacultyChanged()");
			if (mDBBrowser.FacultyClicked)
			{
				CourseIDCollection courses=mCourseDB.GetCoursesByFacultyName((string)mDBBrowser.SelectedFaculty);
				Console.WriteLine("courses="+courses);
				mDBBrowser.Courses = courses;
			}
		}

		
		private void Courses_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( mDBBrowser.SelectedCourseID.Length == 0 )
			{
				return;
			}
			mDBBrowser.FacultyClicked = false;
			CourseID theCourseID = mDBBrowser.SelectedCourseID[0]; 
	            	mDBBrowser.Course    = mCourseDB.GetCourseByNumber(theCourseID.CourseNumber);
		}
		
		private void btRemoveAll_Click(object sender, System.EventArgs e)
		{
			RemoveAllCourses();
			mMainFormLogic.SetStatusBarLine(Resources.String(RESOURCES_GROUP, "AllRemovedMessage" ));
		}
		private void lvOccurences_Validated(object sender, System.EventArgs e)
		{
			ListView occurrences = (ListView) sender;
			if (occurrences.Items.Count==0)
				return;
			CourseEvent theEvent = DBBrowser.SelectedCourseEvent ;
			CourseEventOccurrenceCollection ceoCollection = theEvent.Occurrences;
			// Reset Selected status
			foreach (CourseEventOccurrence ceo in ceoCollection)
			{
				ceo.Selected = false;
			}
			// Select only checked items
			foreach (ListViewItem lvi in occurrences.CheckedItems)
			{
				((CourseEventOccurrence)(lvi.Tag)).Selected=true;
			}
		}
		private void btDone_Click(object sender, System.EventArgs e)
		{
			if (mDBBrowser.CourseBasket.Count>0)
				mMainFormLogic.ScheduleSchedules();
			else
				System.Windows.Forms.MessageBox.Show("נא לבחור קורס אחד לפחות על מנת לסדר מערכות");
		}
		private void mDBBrowser_VisibleChanged(object sender, EventArgs e)
		{
			mDBBrowser.CourseBasket = mCoursesScheduler.Courses;
		}
		#endregion Events
		#region Private Methods
        private bool AddCourseToScheduler( Course course )
        {
		if ( course != null )
		{
			if ( mCoursesScheduler.Courses.Contains( course.Number ) )  
			{ // If course already exists in the basket, we must remove it so it can be updated
				Console.WriteLine("DBLogic.AddCourseToScheduler - Modify course");
				RemoveCourse(course);
				/*foreach (Course c in mDBBrowser.CourseBasket) // search for the course in the basket
				{
					if (c.Number == course.Number) // We found the course
					{
						RemoveCourse(c);
						break;
					}
				}*/
			}
			mCoursesScheduler.AddCourse(course);
			return true;
		}
            	return false;
        }
		
		/// <summary>
		/// Finds out if the Courses ListView contains a certain course.  If it does, it selects it
		/// and returns true. Otherwise, it returns false
		/// </summary>
		/// <param name="courseNumber">The Course Number to look for</param>
		/// <returns></returns>
		private bool CoursesListViewContains ( string courseNumber)
		{
			foreach (CourseID course in DBBrowser.Courses)
			{
				if (course.CourseNumber == courseNumber)
				{
					// TODO DBBrowser.SelectedCourseID = course;
					return true;
				}
			}
			return false;
		}

		#endregion Private Methods
		#region Properties
		public  DBbrowser     DBBrowser
		{
			get{ return mDBBrowser; }
			set
			{
				if ( mDBBrowser != null )
				{
					mDBBrowser.Load -= new EventHandler(DBBrowser_Load);
					mDBBrowser.RemoveCourseClick -= new EventHandler(this.RemoveCourse_Click);
					mDBBrowser.AddCourseClick -= new EventHandler(this.AddCourse_Click);
					mDBBrowser.CourseNumberChanged -= new EventHandler(this.CourseNumerTextChanged);
					mDBBrowser.SelectedFacultyChanged -= new EventHandler( this.Faculties_SelectedIndexChanged );
					mDBBrowser.SelectedCourseChanged -= new EventHandler(this.Courses_SelectedIndexChanged);
					mDBBrowser.OccurrencesFocusOut -= new EventHandler(this.lvOccurences_Validated);
					mDBBrowser.DoneClick -= new System.EventHandler(this.btDone_Click);
					mDBBrowser.RemoveAllClick -= new System.EventHandler(this.btRemoveAll_Click);
					mDBBrowser.VisibleChanged -=new EventHandler(mDBBrowser_VisibleChanged);
				}
				mDBBrowser = value;
				if ( mDBBrowser != null )
				{				
					DBBrowser_Load(this, new EventArgs());
					mDBBrowser.Load += new EventHandler(DBBrowser_Load);
					mDBBrowser.RemoveCourseClick += new EventHandler(this.RemoveCourse_Click);
					mDBBrowser.AddCourseClick += new EventHandler(this.AddCourse_Click);
					mDBBrowser.CourseNumberChanged += new EventHandler(this.CourseNumerTextChanged);
					mDBBrowser.SelectedFacultyChanged += new EventHandler( this.Faculties_SelectedIndexChanged );
					mDBBrowser.SelectedCourseChanged += new EventHandler(this.Courses_SelectedIndexChanged);
					mDBBrowser.OccurrencesFocusOut += new EventHandler(this.lvOccurences_Validated);
					mDBBrowser.DoneClick += new System.EventHandler(this.btDone_Click);
					mDBBrowser.RemoveAllClick += new System.EventHandler(this.btRemoveAll_Click);
					mDBBrowser.VisibleChanged +=new EventHandler(mDBBrowser_VisibleChanged);
					this.SearchControl = mDBBrowser.SearchControl;
					
				}
			}
		}
		private SearchControl SearchControl
		{
			get{ return mSearchControl; }
			set
			{
				if ( mSearchControl != null )
				{
					mSearchControl.Search -= new SearchEventHandler( Search );
					mSearchControl.Load   -= new EventHandler( SearchControl_Load );
				}
				
				mSearchControl = value;
				if ( value != null )
				{
					mSearchControl.Search += new SearchEventHandler( Search );
					mSearchControl.Load   += new EventHandler( SearchControl_Load );
				}
			}
		}
		
		#endregion Properties
		#region Comparers
		// Implements the manual sorting of items by columns.
		public class ListViewItemComparer : System.Collections.IComparer
		{
			private int col;
			public ListViewItemComparer()
			{
				col = 0;
			}
			public ListViewItemComparer(int column)
			{
				col = column;
			}
			public int Compare(object x, object y)
			{
				return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
			}
		}

		public class InvListViewItemComparer : System.Collections.IComparer
		{
			private int col;
			public InvListViewItemComparer()
			{
				col = 0;
			}
			public InvListViewItemComparer(int column)
			{
				col = column;
			}
			public int Compare(object x, object y)
			{
				return -String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
			}
		}

		#endregion


		}
	}
