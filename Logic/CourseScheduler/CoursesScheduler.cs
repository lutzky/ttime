using System;
using System.Collections;

namespace UDonkey.Logic
{
    public delegate void SchedulingProgress( int progress );
	/// <summary>
	/// Summary description for CoursesScheduler.
	/// </summary>
	public class CoursesScheduler
	{
		#region Private Variables
		private int								    mnCurrentState;
        private CoursesList						    mCourses;
        private AbstractConstraintList			    mConstraints;
		private AbstractConstraintList      	    mPostConstraints;
		private SchedulerStatesCollection		    mSchedulerStates;
		private AbstractCourseIteratorsCollection   mIterators;
        private Schedule						    mSchedule;
        private AbstractScheduleStatisticList       mcStatistics;
        private System.Collections.IComparer        mComparer;
		private System.EventHandler                 mFixOccurrence;
		private System.EventHandler                 mUnFixOccurrence;
		private const string                        FIX   = "קבע את";
		private const string                        UNFIX = "בטל קיבוע של";
        private Hashtable                           mSchedulerErrors;
		private bool                                mbScheduling;
		#endregion Private Variables
        #region Events
        public event EventHandler ScheduleChanged;
        public event SchedulingProgress StartScheduling;
        public event SchedulingProgress ContinueScheduling;
        public event SchedulingProgress EndScheduling;
        #endregion Events
		public CoursesScheduler()
		{
            mCourses	     = new CoursesList();
            mConstraints     = new AbstractConstraintList();
            mSchedule        = new Schedule();
            mcStatistics     = new AbstractScheduleStatisticList();
			mPostConstraints = new AbstractConstraintList();
			mnCurrentState   = -1;
			mFixOccurrence   += new EventHandler( this.FixOccurrence );
			mUnFixOccurrence += new EventHandler( this.UnFixOccurrence );
			mbScheduling     = false;
		}
        /// <summary>
        /// Schedule schedules from the courses by the constraints given
        /// </summary>
		public void CreateSchedules()
		{
			mbScheduling = true;
			this.ResetStates();
			this.SetCoursesIterators();
            if( mIterators.Count == 0 )
            {//No coureses to schedule;
                if( EndScheduling != null )
                {
                    EndScheduling( 0 );
                } 
                return;
            }
            else
            {
                if( StartScheduling != null )
                {
                    int mult = 1;
                    foreach( ICourseIterator it in mIterators )
                    {
                        mult *= (it.Count != 0)? it.Count : 1;
                    }
                    StartScheduling( mult );
                }       
            }

			//Start the DFS
			WorkOnIterator( 0 );
            if( this.Comparer != null )
            {
                mSchedulerStates.Sort( this.Comparer );
            }
            if( EndScheduling != null )
            {
                EndScheduling( this.mSchedulerStates.Count );
            }  
			mbScheduling = false;
		}
        /// <summary>
        /// Add course to the courses list
        /// </summary>
        /// <param name="course"></param>
		public void AddCourse( Course course )
		{
			this.mCourses.Add( course.Number, course );
		}
        /// <summary>
        /// Checks if a course's exam ovelapse with other courses
        /// in the scheduler
        /// </summary>
        /// <param name="course">The course</param>
        /// <param name="moadA">True for moadA, False for moadB</param>
        /// <returns>Courses that overlaps</returns>
        public ICollection FindExamsOverlaps( Course course, bool moadA )
        {
            ArrayList list = new ArrayList();
            DateTime firstDate = (moadA)? course.MoadA : course.MoadB;

            if( Configuration.Get( Configuration.GENERAL, "TestAlert", true ) )
            {//If Overlap interval is defined
				int mnExamsInterval = Configuration.Get( "General","TestInterval", 2);
				if( mnExamsInterval <= 0 )
				{
					//Todo: Throw Exception
					return null;
				}
                foreach ( System.Collections.DictionaryEntry entry in mCourses )
                {
                    Course c = (Course)entry.Value;
                    DateTime secondDate = (moadA)? c.MoadA : c.MoadB;
                    if( secondDate == DateTime.MinValue || c.Number == course.Number )
                    {
                        continue;
                    } 
                    TimeSpan span = (firstDate > secondDate )?
                        firstDate - secondDate :
                        secondDate - firstDate ;
                    if( span < new TimeSpan( mnExamsInterval,0,0,0) )
                    {
                        list.Add( c );
                    }                                                               
                }
            }
            return list;

        }
		/// <summary>
		/// Add a constraint to the Scheduler
		/// </summary>
		/// <param name="contraint">The constraint</param>
		public void AddConstraint( ISchedulerConstraint constraint )
		{
			if( constraint.CheckType == ConstraintCheckType.Post )
			{
				this.mPostConstraints.Add( constraint.Key, constraint );
			}
			else
			{
				this.mConstraints.Add( constraint.Key, constraint );
			}
		}
		/// <summary>
		/// Reset the scheduler from its courser and user events
		/// </summary>
		public void ResetCoursesAndEvents() 
		{
			ResetStates();
			mCourses	     = new CoursesList();
			mSchedule        = new Schedule();
			mConstraints	 = new AbstractConstraintList();
		}
		public void AddUserEvent( string eventName,	
			DayOfWeek   day,
			int         hour,
			int         duration
			)
		{
			IScheduleEntry schedEntry = (IScheduleEntry)mSchedule.FullDataTable.Rows[hour][day.ToString()];
			if (schedEntry!=null && schedEntry.UserDefined)
			{
				System.Windows.Forms.MessageBox.Show("User event already exists in that time");
				return;
			}
			UsersEventScheduleObject obj = new UsersEventScheduleObject(
				eventName, day, hour, duration );
			obj.Events[string.Format( "הסר {0}", eventName)] = 
				new EventHandler( RemoveUserEvent );	
			this.Schedule.AddScheduleObject( obj );
			this.AddConstraint( new UsersEventConstraint( obj ) );
			this.Schedule.CallChanged();
		}
		#region Private Methods
		/// <summary>
		/// Reset all the scedules created.
		/// </summary>
		private void ResetStates()
		{
			ResetState();
			mSchedulerStates = new SchedulerStatesCollection();
			mSchedulerErrors = new Hashtable();
		}
		/// <summary>
		/// Generate Courses Iterators for the given courses
		/// </summary>
		/// <returns>AbstractCourseIteratorsCollection of the courses</returns>
		private void SetCoursesIterators()
		{
			mIterators = new AbstractCourseIteratorsCollection();
			bool mixed = Configuration.Get(Configuration.GENERAL,"AllowRegSplit",true);
			foreach( System.Collections.DictionaryEntry entry in mCourses )
			{
				AbstractCourseIterator iterator = null;
				if( mixed )
				{
					iterator = new MixedGroupIterator( entry.Value as Course );
				}
				else
				{
					iterator = new RegistrationGroupIterator( entry.Value as Course );
				}
				mIterators.Add( iterator );
			}
			mIterators.Sort( new CourseIteratorComparer() );
            int mult = 1;
            for( int i = mIterators.Count - 1 ; i > 0 ; --i )
            {//Calculting the depth of th branch the iterator sits on
                ICourseIterator it = (ICourseIterator)mIterators[ i ];
                it.Branch = mult;
                mult *= (it.Count == 0)?1:it.Count;
            }
		}
		private void WorkOnIterator( int index )
		{
			if( index >= mIterators.Count )
			{//Stop Condition
                if( mIterators.Count != 0 )
                {//Only if the state is not empty (At least one iterator);
					if( CheckSchedulePostConstraints( ) )
					{//If all the Post cons
						mSchedulerStates.Add( this.SaveIteratorsState( ) );
					}
                    if ( ContinueScheduling != null )
                    {
                        ContinueScheduling( 1 );
                    }
                }
				return;
			}

			AbstractCourseIterator iterator = (AbstractCourseIterator)mIterators[ index ];
			RemoveIteratorsEvents( iterator );
			while ( iterator.Next() )
			{
                try
                {
                    if( AddIteratorsEvents( iterator ) )
                    {
                        WorkOnIterator( index + 1 ); 
                    }
                    else
                    {//Progres of the branch
                        if ( ContinueScheduling != null )
                        {
                            ContinueScheduling( iterator.Branch );
                        }
                    }
                }
                catch( ScheduleException e )
                {
                    if( mSchedulerErrors.ContainsKey( e.ScheduleError ) )
                    {
                        mSchedulerErrors[ e.ScheduleError ] =
                            (int) mSchedulerErrors[ e.ScheduleError ] + 1;
                    }
                    else
                    {
                         mSchedulerErrors[ e.ScheduleError ] = 1;
                    }
                }
				RemoveIteratorsEvents( iterator );
			}
		}
		private void ResetState()
		{
			if( mnCurrentState == -1 )
			{
				return;
			}
			mnCurrentState = -1;
			for ( int i = 0 ; i <  mIterators.Count ; ++i )                
			{
                AbstractCourseIterator iterator  =
                    (AbstractCourseIterator)mIterators[ i ];
				RemoveIteratorsEvents( iterator );
			}
		}
		private void SetState( SchedulerState state )
		{           
			for ( int i = 0 ; i < state.Indexes.Length ; ++i )
			{

				AbstractCourseIterator iterator 
					= (AbstractCourseIterator)mIterators[ i ];
				iterator.GoTo( state.Indexes[i] );
				AddIteratorsEvents( iterator );
			}
		}
		private void RemoveIteratorsEvents( AbstractCourseIterator iterator )
		{
			CourseEventCollection events = iterator.CourseEvents;
			foreach( CourseEvent evnt in events )
			{
				foreach( CourseEventOccurrence occurrence in evnt.Occurrences )
				{
					mSchedule.RemoveScheduleObject( 
						new CourseEventOccurrencesScheduleObject( occurrence ) );			
				}
			}
		}
		private bool AddIteratorsEvents( AbstractCourseIterator iterator )
		{
			CourseEventCollection events = iterator.CourseEvents;
			foreach( CourseEvent evnt in events )
			{
				foreach( CourseEventOccurrence occurrence in evnt.Occurrences )
				{
					CourseEventOccurrencesScheduleObject occure = 
						new CourseEventOccurrencesScheduleObject( occurrence );				
					if( !mSchedule.AddScheduleObject( occure ) )
					{//Could not be added
						return false;
					}
					if( !mbScheduling )
					{
						bool fix = this.mPostConstraints.ContainsKey( occure.Key );
						string name = string.Format( "{0} {1} {2} {3}",
							fix?UNFIX : FIX,
							occurrence.CourseEvent.Course.NickName,
							occurrence.CourseEvent.Type,
							occurrence.CourseEvent.EventNum);
						this.AddFixItemMenu( occure, name, !fix );
					}
				}
			}
			return true;
		}
		private ISchedulerState SaveIteratorsState()
		{
			ISchedulerState state = new SchedulerState( mIterators.Count );
            foreach (  System.Collections.DictionaryEntry entry in mcStatistics )
            {
				AbstractScheduleStatistic stat = (AbstractScheduleStatistic)entry.Value;
                state.Statistics.Add( stat );
            }
			for( int i = 0 ; i < mIterators.Count ; ++i )
			{
				state.Indexes[i] = ( (AbstractCourseIterator)mIterators[ i ] ).Index;
			}
			state.CalculteStatistics( this.Schedule );
			return state;
		}

		private bool CheckSchedulePostConstraints()
		{
			foreach( System.Collections.DictionaryEntry entry in this.mPostConstraints )
			{
				ISchedulerConstraint contraint = (ISchedulerConstraint)entry.Value;
				if( contraint.Set && !contraint.Check( this.Schedule ) )
				{//The constraint check failed
					return false;
				}
			}
			return true;
		}
		private void FixOccurrence( object sender, EventArgs args )
		{
			IScheduleObject obj = (IScheduleObject)sender;
			ScheduleEventArgs sargs = (ScheduleEventArgs)args;
			obj.Events.Remove( sargs.Text );
			ISchedulerConstraint constraint = new ScheduleObjectConstraint( obj );
			constraint.Set = true;
			this.AddConstraint( constraint );
			this.AddFixItemMenu( obj,
				sargs.Text.Replace(FIX, UNFIX),
				false);
			ScheduleChanged( this, null );
			return;
		}
		private void UnFixOccurrence( object sender, EventArgs args )
		{			
			IScheduleObject obj = (IScheduleObject)sender;
			ScheduleEventArgs sargs = (ScheduleEventArgs)args;
			obj.Events.Remove( sargs.Text );
			this.mPostConstraints.Remove( obj.Key );
			this.AddFixItemMenu( obj, 
				sargs.Text.Replace(UNFIX, FIX),
				true );
			ScheduleChanged( this, null );
			return;
		}
		private void RemoveUserEvent( object sender, EventArgs args )
		{
			IScheduleObject obj = (IScheduleObject) sender;
			this.Schedule.RemoveScheduleObject( obj );
			this.mConstraints.Remove( obj.Key );
			this.Schedule.CallChanged();
		}
		private void AddFixItemMenu( IScheduleObject obj, string name, bool fix )
		{
			if( fix )
			{
				obj.Events.Add( 
					name, mFixOccurrence );
				//obj.ForeColor = System.Drawing.Color.Empty;
				//obj.BackColor = System.Drawing.Color.Empty;
			}
			else
			{
				obj.Events.Add( 
					name , mUnFixOccurrence );
				//obj.ForeColor = System.Drawing.Color.WhiteSmoke;
				//obj.BackColor = System.Drawing.Color.DarkSeaGreen;
			}
		}

		#endregion Private Methods
		#region Properties
		public CoursesList Courses
		{
			get
			{ 
				return mCourses;
			}
		}
        public AbstractScheduleStatisticList Statistics
        {
            get{ return mcStatistics; }
        }
        public System.Collections.IComparer Comparer
        {
            get{ return mComparer; }
            set{ mComparer = value; }
        }
		public Schedule Schedule
		{
			get{ return mSchedule; }
		}
		public int Index
		{
			get{ return mnCurrentState; }
			set{
				if( mSchedulerStates == null )
				{
					return; 
				}
				if ( value < 0 	|| value >= mSchedulerStates.Count )
				{//Circular indexes
					value = value % mSchedulerStates.Count;
					if( value < 0 )
					{
						value += mSchedulerStates.Count;
					}
				}
				ResetState();
				mnCurrentState = value;
				SetState( (SchedulerState)mSchedulerStates[ value ] );
			}
		}
		public int MinimalExamsInterval
		{
			get 
			{ 
				if( Configuration.Get(Configuration.GENERAL,"TestAlert",true) )
				{
					return Configuration.Get(Configuration.GENERAL,"TestInterval",2);
				}
				else
				{
					return 1;
				}
			}
			set 
			{ 
				Configuration.Set( Configuration.GENERAL,"TestAlert", value > 0 );
				Configuration.Set( Configuration.GENERAL,"TestInterval", ( value > 0 )?value:0 );
			}
		}
		public int Count
		{
			get{ return mSchedulerStates.Count; }
		}
        public SchedulerState CurrentState
        {
            get
            {
                return ( this.Index == -1 )?
                    null:
                    this.mSchedulerStates[ this.Index ] as SchedulerState;   
            }
        }
		public SchedulerStatesCollection States
		{
			get{ return mSchedulerStates; }
		}
        public Hashtable Errors
        {
            get{ return mSchedulerErrors; }
        }
		public ICollection UserEvents
		{
			get
			{
				ArrayList list = new ArrayList();
				foreach ( DictionaryEntry e in mConstraints)
				{
					ISchedulerConstraint c = (ISchedulerConstraint)e.Value;
					UsersEventConstraint h = c as UsersEventConstraint;
					if (h != null)
					{
						list.Add(h.UserEvent);
					}
				}
				return list;
			}
		}
		#endregion Properties
	}

	class ScheduleEventArgs : EventArgs 
	{
		public ScheduleEventArgs(string text)
		{
			Text = text;
		}

		public string Text;
	}
}
