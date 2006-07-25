using System;

namespace UDonkey.Logic
{
	/// <summary>
	///  ScheduleSchedulingException being thrown when a Schedule cannot schedule IScheduleObject.
	/// </summary>
	public class ScheduleException :  Exception
	{
        private IScheduleObject mObject;
        private ScheduleDayData mDayData;
        private ScheduleErrors  mError;
       
        #region Constructors
        public ScheduleException(IScheduleObject obj,ScheduleDayData  dayData,ScheduleErrors error): 
            this( string.Empty, obj, dayData, error) {}

        public ScheduleException(string message, IScheduleObject obj,ScheduleDayData  dayData, ScheduleErrors error):
            base( message )
        {
            mObject  = obj;
            mDayData = dayData;
            mError   = error;
        }
        #endregion Constructors
        #region Properties
        /// <summary>
/// The SchduleObject that caused this exception
/// </summary>
        public IScheduleObject ScheduleObject
        {
            get{ return mObject; }
        }
        /// <summary>
        /// The ScheduleDayData that throwed this exception
        /// </summary>
        public ScheduleDayData DayData
        {
            get{ return mDayData; }
        }
        /// <summary>
        /// The ScheduleError of this exception
        /// </summary>
        public ScheduleErrors ScheduleError
        {
            get{ return mError; }
        }

    

        #endregion Properties

	}
}
