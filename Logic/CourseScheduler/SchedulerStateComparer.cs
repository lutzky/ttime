using System;
using System.Collections;
namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ScedulerStateComparer.
	/// </summary>
	public class SchedulerStateComparer: System.Collections.IComparer
	{
        private Hashtable mhtStatisticsWeight;

		public SchedulerStateComparer()
		{
            mhtStatisticsWeight = new Hashtable(); 
		}
		#region IComparer Members
		public int Compare(object x, object y)
		{
			if ( !( x is ISchedulerState && y is ISchedulerState ) )
				throw new ArgumentException( "x,y must be of type ISchedulerState", "x,y" );
			
			ISchedulerState X = x as ISchedulerState;
			ISchedulerState Y = y as ISchedulerState;

			return CalculateValue(Y) - CalculateValue(X);
		}

		public int CalculateValue( ISchedulerState state )
		{
            int ret = 0;
            int weightSum = 0;
			foreach( string key in state.StatisticsMarks.Keys )
			{
                object weight = mhtStatisticsWeight[ key ];
                if( weight is int )
                {
                    weightSum += (int)weight;
                    ret += 	(int)state.StatisticsMarks[ key ] * (int)weight;
                }			
			}
            state.Mark = ( weightSum == 0 )? ret: ret / weightSum;
			return ret;
		}

		#endregion
        /// <summary>
        /// Returns IDictionary full of the weight for each 
        /// statistic in the SchedulerState
        /// </summary>
        public IDictionary StatisticsWeight
        {
            get{ return mhtStatisticsWeight; }
        }
	}
}
