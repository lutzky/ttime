using System;

namespace UDonkey.Logic
{
    /// <summary>
    /// CourseEvent represent one occurrence of a CourseEvent.
    /// </summary>
    public class CourseEventOccurrence
    {
        #region Private Variables
        private CourseEvent     mCourseEvent;
        private DayOfWeek       mDay;
        private int             mHour;
        private int             mDuration;
        private string          mLocation;
		private bool			mSelected;
        #endregion Private Variables
        /// <summary>
        /// Base construtor for CourseEvent
        /// </summary>
        /// <param name="type">The type of this event</param>
        /// <param name="day">The day in which this event happens</param>
        /// <param name="hour">The hour in which this event happens</param>
        /// <param name="duration">The duration of this event</param>
        public CourseEventOccurrence(
            CourseEvent     courseEvent,
            DayOfWeek       day,
            int             hour,
            int             duration,
            string          location
            )
        {
            this.CourseEvent = courseEvent;
            this.Day         = day;
            this.Hour        = hour;
            this.Duration    = duration;
            this.Location    = location;
			this.Selected	 = true;
        }

        /// <summary>
        /// The type of this occurrence
        /// </summary>
        public CourseEvent CourseEvent
        {
            get{ return mCourseEvent;  }
            set{ mCourseEvent = value; }
        }
        /// <summary>
        /// The day in which this occurrence happens
        /// </summary>
        public DayOfWeek Day
        {
            get{ return mDay;  }
            set{ mDay = value; }
        }
        /// <summary>
        /// The hour in which this occurrence happens
        /// </summary>
        public int Hour
        {
            get{ return mHour;  }
            set{ mHour = value; }
        }
        /// <summary>
        /// The duration of this occurrence
        /// </summary>
        public int Duration
        {
            get{ return mDuration;  }
            set{ mDuration = value; }
        }
        /// <summary>
        /// The location of this occurrence
        /// </summary>
        public string Location
        {
            get{ return mLocation;  }
            set{ mLocation = value; }
        }
		
		
		public bool Selected
		{
			get{ return mSelected;  }
			set{ mSelected = value; }
		}

        
		public override bool Equals(object obj)
		{
			CourseEventOccurrence to = (CourseEventOccurrence) obj;
			if ((this.Day.Equals(to.Day))
				&&(this.Hour.Equals(to.Hour))
				&&(this.Duration.Equals(to.Duration)))
				return true;
			else return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}


    }
}
