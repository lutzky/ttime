using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for AbstractScheduleObject.
	/// </summary>
	public abstract class AbstractScheduleObject: IScheduleObject
	{
		protected Schedule        mSchedule;
		protected DayOfWeek       mDayOfWeek;
		protected int             mnStartHour;
		protected int             mnDuration;
		protected bool			mbUserDefined;
        
		protected System.Collections.IDictionary mdEvents;
		protected System.Drawing.Color           mForeColor;
		protected System.Drawing.Color           mBackColor;
		protected VerbosityFlag                  mVerbosity;

		public AbstractScheduleObject()
		{
			mdEvents = new System.Collections.Hashtable();
			/*mForeColor = System.Drawing.Color.Black;
			mBackColor = System.Drawing.Color.White;*/
            mVerbosity = VerbosityFlag.Minimal;
		}
		public AbstractScheduleObject( IScheduleObject from )
		{
			this.Schedule  = from.Schedule;
			this.DayOfWeek = from.DayOfWeek;
			this.StartHour = from.StartHour;
			this.Duration  = from.Duration;
			this.Verbosity = from.Verbosity;
			mdEvents = from.Events;
			mBackColor = from.BackColor;
			mForeColor = from.ForeColor;
		}
		#region IScheduleObject
		public abstract string Key { get; }
        public virtual Schedule Schedule
        { 
            get{ return mSchedule;  }
            set{ mSchedule = value; }
        }
        public virtual DayOfWeek DayOfWeek
        { 
            get{ return mDayOfWeek;  }
            set{ mDayOfWeek = value; }
        }
        public virtual int StartHour
        { 
            get{ return mnStartHour;  }
            set{
                if ( value < 0 )
                {
                    throw new ArgumentOutOfRangeException( "value", value, "StartHour must be greater than zero" );
                }
                                
                if ( this.Duration + value > 24 )
                {
                    throw new ArgumentOutOfRangeException( "value", value, "StartHour + Duration mush be under 24" );
                }
                mnStartHour = value; 
            }
        }
        public virtual int Duration
        { 
            get{ return mnDuration;  }
            set
            {
                if ( value <= 0 )
                {
                    throw new ArgumentOutOfRangeException( "value", value, "Duration must be greater than zero" );
                }
                                
                if ( this.StartHour + value > 24 )
                {
                    throw new ArgumentOutOfRangeException( "value", value, "StartHour + Duration mush be under 24" );
                }

                mnDuration = value; 
            }
        }

        public virtual System.Collections.IDictionary Events
        { 
            get{ return mdEvents; } 
        }
        public virtual System.Drawing.Color           ForeColor
        {
            get{ return mForeColor; }
            set{ mForeColor = value; }
        }
        public virtual System.Drawing.Color           BackColor 
        { 
            get{ return mBackColor; }
            set{ mBackColor = value; }
        }

        public virtual VerbosityFlag                  Verbosity
        {
            get{ return mVerbosity; }
            set{ mVerbosity = value; }
        }

		public virtual bool							  UserDefined
		{ 
			get{ return mbUserDefined; }
			set{ mbUserDefined = value; }
		}

        public abstract string ToString( VerbosityFlag flag );
        public override string ToString()
        {
            return this.ToString( this.Verbosity );
        }
        #endregion IScheduleObject
	}
}
