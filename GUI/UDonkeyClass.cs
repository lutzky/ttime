using System;
using System.Collections;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using UDonkey.DB;
using UDonkey.RepFile;
using UDonkey.IO;
using UDonkey.Logic;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for UDonkey.
	/// </summary>
	public class UDonkeyClass
	{
		// Globlized Classes
		private CourseDB          mCourseDB;
		private CoursesScheduler  mScheduler;
		// Logics
		private DBLogic           mDBLogic;
		private ScheduleGridLogic mScheduleGridLogic;
		private MainFormLogic     mMainFormLogic;
		private ConfigurationController mConfigurationController;

		//GUI
		 private MainForm          mMainForm;

		//Statistics
		private StartHourStatistic mStartHourStatistic;
		private EndHourStatistic   mEndHourStatistic;
		private HolesStatistic     mHolesStatistic;
		private FreeDaysStatistic  mFreeDaysStatistic;
		private SchedulerStateComparer mComparer;

		//Constraints
		private FreeDaysConstraint          mFreeDaysConstraint;
		private MinimalStudyHoursConstraint mMinimalStudyHoursConstraint;
		private MaximalStudyHoursConstraint mMaximalStudyHoursConstraint;

		public UDonkeyClass()
		{
			InitComponents();
		}

		private void InitComponents()
		{
			mScheduler = new CoursesScheduler();
            InitCoursesScheduler();
            try {
				mCourseDB = new CourseDB();
			}
			catch(System.IO.FileNotFoundException)
			{
				const string RESOURCES_GROUP = "CourseDB";

				MessageBox.Show( null, Resources.String( RESOURCES_GROUP, "xsdFailedMessage1" ), Resources.String( RESOURCES_GROUP, "xsdFailedMessage2" ), MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
				return;
			}
			
			if (this.CourseDB.isSchemaExists != true) return;
			InitDataBase();
			if (this.CourseDB.isInitialized != true) return;
			Configuration.RegisterConfigurationChangeHandler( Configuration.DISPLAY     , "", new ConfigurationChangeHandler( this.DisplayConfigurationChanged ) );
			Configuration.RegisterConfigurationChangeHandler( Configuration.SORT        , "", new ConfigurationChangeHandler( this.SortingConfigurationChanged ) );
			Configuration.RegisterConfigurationChangeHandler( Configuration.CONSTRAINTS , "", new ConfigurationChangeHandler( this.ConstraintsConfigurationChanged ) );
			mMainFormLogic           = new MainFormLogic( this, mScheduler ); 
			mMainForm                = mMainFormLogic.MainForm;
			mMainForm.WindowState    = FormWindowState.Maximized;
			mMainForm.SelectedTab    = 1;
			mDBLogic                 = new DBLogic( this.CourseDB , this.Scheduler, this.mMainFormLogic );
			mScheduleGridLogic       = new ScheduleGridLogic( this.Scheduler, mMainForm.Grid );
			mDBLogic.DBBrowser       = mMainForm.DBBrowserControl;
			mConfigurationController = new ConfigurationController( mMainForm.ConfigControl );
			mConfigurationController.Load();
		}

		private void InitDataBase()
		{		 
			try
			{
				//try default mainDB.xml
				mCourseDB.Load( CourseDB.DEFAULT_DB_FILE_NAME );				
			}
			catch(System.IO.FileNotFoundException)
			{
				// try default REPFILE.zip
				mCourseDB.OpenLocalZip(CourseDB.m_zipFileName);
				try
				{
					RepFileConvertForm form = new RepFileConvertForm();
					// try defualt REPY
					RepToXML.Convert("REPY", System.IO.Directory.GetCurrentDirectory() + "\\" + CourseDB.DEFAULT_DB_FILE_NAME);
				}
				catch
				{
					//no defualt DB found
					Application.Run( new LoadDBForm(mCourseDB,System.IO.Directory.GetCurrentDirectory()) );
				}	
			}
		}

		private void InitCoursesScheduler()
		{
			ISchedulerConstraint constraint;
			IScheduleStatistic stat;
			mComparer = new SchedulerStateComparer();
			this.Scheduler.Comparer = mComparer;
 
			mStartHourStatistic = new StartHourStatistic();
			
			stat = mStartHourStatistic;
			this.Scheduler.Statistics[ stat.Name ] =  stat;
			mComparer.StatisticsWeight[ stat.Name ] = 1;

			mEndHourStatistic = new EndHourStatistic();
			stat = mEndHourStatistic;
			this.Scheduler.Statistics[ stat.Name ] =  stat;
			mComparer.StatisticsWeight[ stat.Name ] = 1;

			mHolesStatistic = new HolesStatistic();
			stat = mHolesStatistic;
			this.Scheduler.Statistics[ stat.Name ] =  stat;
			mComparer.StatisticsWeight[ stat.Name ] = 1;

			mFreeDaysStatistic = new FreeDaysStatistic();
			stat = mFreeDaysStatistic;
			this.Scheduler.Statistics[ stat.Name ] =  stat;
			mComparer.StatisticsWeight[ stat.Name ] = 1;

			mFreeDaysConstraint = new FreeDaysConstraint();
			constraint = mFreeDaysConstraint;
			constraint.Set = false;
			this.Scheduler.AddConstraint( constraint );	
	
			mMinimalStudyHoursConstraint = new MinimalStudyHoursConstraint();
			constraint = mMinimalStudyHoursConstraint;
			constraint.Set = false;
			this.Scheduler.AddConstraint( constraint );	

			mMaximalStudyHoursConstraint = new MaximalStudyHoursConstraint();
			constraint = mMaximalStudyHoursConstraint;
			constraint.Set = false;
			this.Scheduler.AddConstraint( constraint );	

		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main() 
		{   
			UDonkeyClass udonkey = new UDonkeyClass();
			if ((udonkey.CourseDB.isInitialized != true) || (udonkey.CourseDB.isSchemaExists != true)) return;
			udonkey.Run();
			return;
		}

		public void Run()
		{
			mMainFormLogic.Run();
		}
		public CoursesScheduler Scheduler
		{
			get{ return mScheduler; }
		}
		public CourseDB CourseDB
		{
			get{ return mCourseDB; }
		}

		public DBLogic DBLogic
		{
			get { return mDBLogic; }
		}

		public void Reset()
		{
			mScheduler.ResetCoursesAndEvents();	
			mMainFormLogic.SetNavigationButton( false );
			mMainFormLogic.SetStatusBarLine( "UDonkey" );
			mDBLogic.RemoveAllCourses();
			mScheduleGridLogic.Refresh();
			mMainFormLogic.MainForm.SelectedTab = 0;
		}
		private void DisplayConfigurationChanged( string path, string name, object newVal, object oldVal )
		{
			switch( name.ToLower() )
			{
				case "sunday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.ראשון.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.ראשון.ToString() );
					}
					break;
				}
				case "monday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.שני.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.שני.ToString() );
					}
					break;
				}
				case "tuesday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.שלישי.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.שלישי.ToString() );
					}
					break;
				}
				case "wednesday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.רביעי.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.רביעי.ToString() );
					}
					break;
				}
				case "thursday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.חמישי.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.חמישי.ToString() );
					}
					break;
				}
				case "friday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.שישי.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.שישי.ToString() );
					}
					break;
				}
				case "saturday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.שבת.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.שבת.ToString() );
					}
					break;
				}
				case "starthour" : 
				case "endhour" :
				{
					this.mScheduleGridLogic.Refresh();
					break;
				}
			}
			return;
		}
		private void SortingConfigurationChanged( string path, string name, object newVal, object oldVal )
		{
			IScheduleStatistic stat = null;
			switch( name.ToLower() )
			{
				case "prefstarthour":
				{
					stat = mStartHourStatistic;
					break;
				}
				case "prefendhour":
				{
					stat = mEndHourStatistic;
					break;
				}
				case "preffreedays":
				{
					stat = mFreeDaysStatistic;
					break;
				}
				case "prefholes":
				{
					stat = mHolesStatistic;
					newVal = -(int)newVal;
					break;
				}
			}
			if ( stat != null )
			{
				int val = (int)newVal;
				if( val < 0 )
				{
					stat.Positive = false;
					val = -val;
				}
				mComparer.StatisticsWeight[stat.Name] = val;
			}
			return;
		}
		private void ConstraintsConfigurationChanged( string path, string name, object newVal, object oldVal )
		{
			ISchedulerConstraint constraint = null;
			switch( name.ToLower() )
			{
				case "minfreedays":
				{
					constraint = mFreeDaysConstraint;
					break;
				}
				case "mindailyhours":
				{
					constraint = mMinimalStudyHoursConstraint;
					break;
				}
				case "maxdailyhours":
				{
					constraint = mMaximalStudyHoursConstraint;
					break;
				}
			}

			if( constraint != null  )
			{
				if( (int)newVal > 0 )
				{
					constraint.Set = true;
				}
				else
				{
					constraint.Set = false;
				}
			}
			return;
		}

		public void RefreshSchedule()
		{
		    this.mScheduleGridLogic.Refresh();
		}

		public void ImportSystemState (string filename)
		{
			if (filename.Length==0)
				return;
			Stream strm;
			XmlElement node;
			try
			{
				strm = File.OpenRead(filename);
			}
			catch(System.IO.FileNotFoundException)
			{
				MessageBox.Show(filename + " does not exist");
				return;
			}
			XmlDataDocument dataFile = new XmlDataDocument();
			dataFile.Load(strm);
			XmlElement root = dataFile.DocumentElement;
			Reset();
			try
			{
				node = root["Config"];
				IOManager.ImportConfigFromXml(node);

				node = root["CourseList"];
				CoursesList courses = IOManager.ImportCourseListFromXmlNode(node);
			
				node = root["EVENT_LIST"];
				ICollection events = IOManager.ImportEventsFromXml(node);

				CoursesScheduler scheduler = Scheduler;
				foreach (Course c in courses.Values)
				{
					scheduler.AddCourse(c);
				}
				foreach (UsersEventScheduleObject u in events)
				{
					scheduler.AddUserEvent(u.Event, u.DayOfWeek, u.StartHour, u.Duration);
				}
			}
			catch  // No course found by that number
			{
				MessageBox.Show("Error with Xpath");
			}
			

		}

	}
}
