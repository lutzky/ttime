using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for AbstractScheduleStatistic.
	/// </summary>
	public abstract class AbstractScheduleStatistic: IScheduleStatistic
	{
        protected float mnResult;
        protected int   mnNumberOfDays;
        protected int   mnNumberOfFreeDays;
		protected bool  mPositive;

		public AbstractScheduleStatistic()
		{
            mnResult     = 0;
			mPositive   = true;
		}
        
        #region Abstract Methods
        /// <summary>
        /// Calculate the statistics for each daydata
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract void CalculateDayData( ScheduleDayData data );
        /// <summary>
        /// Sum and create the final statistic
        /// </summary>
        /// <returns></returns>
        public abstract float FinalCalculation();
        /// <summary>
        /// Normilize result to be between 0 to 100
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public abstract int NormalizeResult( float result );
        #endregion Abstract Methods
        #region IScheduleStatistic
        public virtual void  Calculate( Schedule schedule )
        {
            mnResult     = 0;
            mnNumberOfDays = 0;
            mnNumberOfFreeDays = 0;
            foreach( System.Collections.DictionaryEntry entry in schedule.DaysData )
            {
                ScheduleDayData dayData = (ScheduleDayData) entry.Value;
               
                if( dayData.Day == DayOfWeek.שעות )
                    continue;

                ++mnNumberOfDays;
                
                if( dayData.IsFreeDay )
                {
                    ++mnNumberOfFreeDays;
                }
                CalculateDayData( dayData );
            }
            mnResult = FinalCalculation();
        }
        public abstract string Name { get; }
        /// <summary>
        /// Raw result of this statistic
        /// </summary>
        public virtual float Result 
        { 
            get{ return mnResult; }
        }
        /// <summary>
        /// A Mark between 0 (lowest) to 100(best) for the result
        /// </summary>
        public virtual int   Mark   
        { 
            get
            { 
                int mark = NormalizeResult( this.Result ); 
                if( mark < 0 || mark > 100 )
                {
                    throw new ArgumentOutOfRangeException
                        ( "Mark", mark, "Mark must be 0=< mark =<100" );
                }
                return (Positive)? mark : 100 - mark;
            }
        }
		public virtual bool  Positive 
		{ 
			get{ return mPositive; }
			set{ mPositive = value; }
		}
        #endregion IScheduleStatistic

	}
}
