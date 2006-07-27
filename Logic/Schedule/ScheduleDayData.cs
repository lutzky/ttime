using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ScheduleDayData.
	/// </summary>
	public class ScheduleDayData
	{
		#region Private Variables
        private Schedule  mSchedule;
        private DayOfWeek meDay;
		private string	  dayString;
        private int       mnStartHour;
        private int       mnEndHour;
		private int       mnEndHourConstraint;
		private int       mnStartHourConstraint;
		private bool      mbFreeDayConstraint;
		private string    msConfigurationPath;
		#endregion Private Variables
		#region Constructor
		public ScheduleDayData( Schedule schedule, DayOfWeek day )
		{
            mSchedule   = schedule;
            meDay       = day;
			dayString   = day.ToString();
            mnStartHour			  = 25;
            mnEndHour			  = -1;
			mnEndHourConstraint   = 23;
			mnStartHourConstraint = 0;
			mbFreeDayConstraint   = false;
			msConfigurationPath   = ConfKey();
			Configuration.RegisterConfigurationChangeHandler( msConfigurationPath, "",
				new ConfigurationChangeHandler( this.ConfigurationChange ) );
		}
		#endregion Constructor
		#region Methods
		public IScheduleEntry GetScheduleObject( int row )
		{
			return mSchedule.FullDataTable.Rows[ row ][ dayString ] as IScheduleEntry;
		}
		/// <summary>
		/// Add ScheduleObject to this Schedule
		/// </summary>
		/// <param name="obj">ScheduleObject to add</param>
		public bool AddScheduleObject( IScheduleObject obj )
		{
			if ( CanBeAdded( obj ) )
			{
				obj.DayOfWeek = this.Day;
				int endTime = obj.StartHour + obj.Duration;

				if ( !obj.UserDefined )
				{//!User defined objects should update DayData's properties
					if ( obj.StartHour < this.StartHour )
					{//Update this.StartHour
						mnStartHour = obj.StartHour;
					}
					if ( endTime > this.EndHour )
					{//Update this.EndHour
						mnEndHour = endTime;
					}                    
				}

				for( int i = obj.StartHour ; i < endTime ; ++i )
				{
					IScheduleEntry entry = (IScheduleEntry)mSchedule.FullDataTable.Rows[ i ][dayString];
					entry.Add( obj.Key, obj );
				}
				return true;
			}
			else return false;
		}
		/// <summary>
		/// Remove the ScheduleObject from the Schedule
		/// </summary>
		/// <param name="obj">ScheduleObject to remove</param>
		public void RemoveScheduleObject( IScheduleObject obj )
		{           
			int endTime = obj.StartHour + obj.Duration;
			for( int i = obj.StartHour ; i < endTime ; ++i )
			{
				IScheduleEntry entry = (IScheduleEntry)	mSchedule.FullDataTable.Rows[ i ][dayString];
//				int entryObjectCount = entry.Events.Count;
				entry.Remove( obj.Key );
//				if (entry.Events.Count == entryObjectCount)
//					return;
			}

			if ( !obj.UserDefined )
			{
                if ( obj.StartHour == this.StartHour && endTime == this.EndHour )
                {//The only !UserDefined object, reset the hours
					if (GetScheduleObject(this.StartHour).IsEmpty)
					{
						mnStartHour			  = 25;
						mnEndHour			  = -1;
					}
                }
                else if ( obj.StartHour == this.StartHour )
                {

					for ( int i = obj.StartHour;
						i <= this.EndHour;
						++i)
					{//Find the next !UserDefined object
						IScheduleEntry scheduleEntry = GetScheduleObject(  i );
						if( !scheduleEntry.UserDefined && !scheduleEntry.IsEmpty)
						{
							mnStartHour = i;
							return;
						}       
					}
                }
                else if (endTime == this.EndHour )
                {
					for ( int i = endTime;
						i >= this.StartHour;
						--i)
					{//Find the next !UserDefined object
						IScheduleEntry scheduleEntry = GetScheduleObject( i );
						if( !scheduleEntry.UserDefined && !scheduleEntry.IsEmpty)
						{
							mnEndHour = i+1;
							return;
						}       
					}
                }
			}
		}
		/// <summary>
		/// Remove the ScheduleObject from the Schedule
		/// </summary>
		/// <param name="hour"></param>
		/// <param name="duration"></param>
		/// <param name="key"></param>
		public void RemoveScheduleObjectAt( int hour, string key )
		{
			IScheduleEntry entry = (IScheduleEntry)	mSchedule.FullDataTable.
				Rows[ hour ][dayString];
			IScheduleObject obj = entry[key];
			if( obj != null )
			{
				RemoveScheduleObject( obj );
			}
		}
        /// <summary>
        /// Check if the IScheduleObject can be added
        /// </summary>
        /// <param name="obj">The IScheduleObject to be added</param>
        /// <returns>True if the object can be added</returns>
		public bool CanBeAdded( IScheduleObject obj )
		{
			int endTime = obj.StartHour + obj.Duration;

			if ( !obj.UserDefined )
			{   //!User defined objects should be checked for constraints
				if ( this.FreeDayConstraint )
				{//No job at free day.
					return false;
					//throw new ScheduleException( obj, this, ScheduleErrors.FreeDay );
				}
				if ( obj.StartHour < this.StartHourConstraint )
				{//No job before the begining of the day
					return false;
					//throw new ScheduleException( obj, this, ScheduleErrors.StartHour );
				}
				if ( endTime > this.EndHourConstraint )
				{//No job after the end of the day
					return false;
                   // throw new ScheduleException( obj, this, ScheduleErrors.EndHour );
				}
			}

			for( int i = obj.StartHour ; i < endTime ; ++i )
			{
				IScheduleEntry entry = mSchedule.FullDataTable.Rows[ i ][dayString] as IScheduleEntry;
				if( !entry.IsEmpty )
				{
					if ( obj.UserDefined != entry.UserDefined )
					{//Only objects with the same UserDefined propery can be in the same entry;
						return false;
						//throw new ScheduleException( obj, this, ScheduleErrors.UserEvent );
					}
					if ( entry.ContainsKey( obj.Key ) )
					{
						return false;
					}
					if ( entry.Count >= mSchedule.AllowedOverlaps )
					{
						return false;
						//throw new ScheduleException( obj, this, ScheduleErrors.Overlaps );
					}
				}
			}
			return true;
		}
		private string ConfKey()
		{
			string name = string.Empty;
			switch( meDay )
			{
				case DayOfWeek.ראשון: 
				{
					name = System.DayOfWeek.Sunday.ToString();
					break;
				}
				case DayOfWeek.שני  : 
				{
					name = System.DayOfWeek.Monday.ToString();
					break;
				}
				case DayOfWeek.שלישי: 
				{
					name = System.DayOfWeek.Tuesday.ToString();
					break;
				}
				case DayOfWeek.רביעי: 
				{
					name = System.DayOfWeek.Wednesday.ToString();
					break;
				}
				case DayOfWeek.חמישי:
				{
					name = System.DayOfWeek.Thursday.ToString();
					break;
				}
				case DayOfWeek.שישי:
				{
					name = System.DayOfWeek.Friday.ToString();
					break;
				}
				case DayOfWeek.שבת:
				{
					name = System.DayOfWeek.Saturday.ToString();
					break;
				}
				case DayOfWeek.שעות:
				{
					name = "Hours";
					break;
				}
			}
			return string.Concat(@"Schedule\",name);
		}
		private void ConfigurationChange( string path, string name, object newVal, object oldVal )
		{
			switch( name.ToLower() )
			{
				case "free":
				{
					mbFreeDayConstraint = (bool)newVal;
					break;
				}
				case "endhour":
				{
					mnEndHourConstraint = int.Parse(newVal.ToString());
					break;
				}
				case "starthour":
				{
					mnStartHourConstraint = int.Parse(newVal.ToString());
					break;
				}
			}
			return;
		}
		#endregion Methods
		#region Properties
        public DayOfWeek  Day
        {
            get{ return meDay; }
        }
        public int  StartHour
        {
            get{ return mnStartHour; }
        }

        public int  EndHour
        {
            get{ return mnEndHour; }
        }
        public int  Holes
        {
            get
            {
                if( this.IsFreeDay )
                {
                    return 0;
                }
                int holes = 0;
                int holeSize = 0;
                bool inBusy = false;
                bool wasBusy = false;

                for( int i = this.StartHour; i < this.EndHour + 1 ; ++i )
                {
                    IScheduleEntry entry = GetScheduleObject( i ); 
                    if( ( entry.IsEmpty || entry.UserDefined ) && inBusy )
                    {//If we were in a course and now we are not
                        inBusy = false;
                        wasBusy = true;
                    }
                    if( ( entry.IsEmpty || entry.UserDefined ) && wasBusy )
                    {//If we has been in a course and now we are not
                        ++holeSize;
                    }
                    if( !entry.IsEmpty && !entry.UserDefined )
                    {
                        if( !inBusy && wasBusy )
                        {//We were in course and now we are there again
                            holes += holeSize;
                            holeSize = 0;
                        }
                        inBusy = true;
                    }
                }
                return holes;
            }
        }
        public bool IsFreeDay
        {
            get
            {
                return mnEndHour == -1 && 
                     mnStartHour == 25; 
            }
        }
        
		public int  StartHourConstraint
		{
			get{ return mnStartHourConstraint; }
			set 
			{
				if ( value >= this.StartHour )
				{
					return;
					//TODO: throw new ArgumentOutOfRangeException( "value", value, "StartHourConstraint must be smaller than StartHour" );
				}
                if ( value >= this.EndHourConstraint )
                {
					return;
                    //TODO: throw new ArgumentOutOfRangeException( "value", value, "StartHourConstraint must be smaller than EndHourConstraint" );
                }
				Configuration.Set( msConfigurationPath,"StartHour", value );
			}
		}
		public int  EndHourConstraint
		{
			get{ return mnEndHourConstraint; }
			set
			{ 
				if ( value <= this.EndHour )
				{
                    return;
					//TODO: throw new ArgumentOutOfRangeException( "value", value, "EndHourConstraint must be larger than EndHour" );
				}
                if ( value <= this.StartHourConstraint )
                {
                    return;
                    //TODO: throw new ArgumentOutOfRangeException( "value", value, "EndHourConstraint must be larger than StartHourConstraint" );
                }
				Configuration.Set( msConfigurationPath,"EndHour",value);
			}
		}

		public int  StartHourRowConstraint
		{
			get{ return mSchedule.HourToRow( StartHourConstraint ); }
		}
		public int  EndHourRowConstraint
		{
			get{ return mSchedule.HourToRow( EndHourConstraint ); }
		}

        public bool FreeDayConstraint
        {
            get{ return mbFreeDayConstraint; }
            set{ Configuration.Set( msConfigurationPath,"Free",value); }
        }
		#endregion Properties
	}
}
