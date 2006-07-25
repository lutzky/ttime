using System;
using UDonkey.DB;
using UDonkey.GUI;
using UDonkey.RepFile;
using UDonkey.IO;

namespace UDonkey.Logic
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
			mCourseDB = new CourseDB();
			if (this.CourseDB.isSchemaExists != true) return;
			InitDataBase();
			if (this.CourseDB.isInitialized != true) return;
			Configuration.RegisterConfigurationChangeHandler( Configuration.DISPLAY     , "", new ConfigurationChangeHandler( this.DisplayConfigurationChanged ) );
			Configuration.RegisterConfigurationChangeHandler( Configuration.SORT        , "", new ConfigurationChangeHandler( this.SortingConfigurationChanged ) );
			Configuration.RegisterConfigurationChangeHandler( Configuration.CONSTRAINTS , "", new ConfigurationChangeHandler( this.ConstraintsConfigurationChanged ) );
			mMainFormLogic           = new MainFormLogic( this, mScheduler ); 
			mMainForm                = mMainFormLogic.MainForm;
			mMainForm.WindowState    = System.Windows.Forms.FormWindowState.Maximized;
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
					System.Windows.Forms.Application.Run( new LoadDBForm(mCourseDB,System.IO.Directory.GetCurrentDirectory()) );
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
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Sunday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Sunday.ToString() );
					}
					break;
				}
				case "monday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Monday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Monday.ToString() );
					}
					break;
				}
				case "tuesday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Tuesday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Tuesday.ToString() );
					}
					break;
				}
				case "wednesday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Wednesday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Wednesday.ToString() );
					}
					break;
				}
				case "thursday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Thursday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Thursday.ToString() );
					}
					break;
				}
				case "friday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Friday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Friday.ToString() );
					}
					break;
				}
				case "saturday" : 
				{
					if( (bool)newVal )
					{
						this.mMainForm.Grid.ShowColumn ( DayOfWeek.Saturday.ToString() );
					}
					else
					{
						this.mMainForm.Grid.HideColumn( DayOfWeek.Saturday.ToString() );
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
	}
}
