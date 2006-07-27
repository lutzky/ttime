using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for SchedulerState.
	/// </summary>
	public class SchedulerState: ISchedulerState
	{
		private int[] mIndexes;
		private ArrayList mcStatistics;
        private Hashtable mdStatisticsResults;
        private Hashtable mdStatisticsMarks;
        private int mnMark;

        public SchedulerState(int numOfIterators)
        {
            mIndexes            = new int[ numOfIterators ];
            mcStatistics        = new ArrayList();
            mdStatisticsResults = new Hashtable();
            mdStatisticsMarks   = new Hashtable();
            mnMark = 0;
        }
        #region ISchedulerState
		public void  CalculteStatistics( Schedule schedule )
		{
            foreach ( IScheduleStatistic statistic in mcStatistics )
            {
                statistic.Calculate( schedule );
                mdStatisticsResults[ statistic.Name ] = statistic.Result;
                mdStatisticsMarks  [ statistic.Name ] = statistic.Mark  ;
            }
		}

		public int[] Indexes
		{
			get{ return mIndexes; }
		}

        public IList Statistics
        {
            get{ return mcStatistics; }
        }
        public IDictionary StatisticsResults
        {
            get{ return mdStatisticsResults; }
        }
        public IDictionary StatisticsMarks
        {
            get{ return mdStatisticsMarks; }
        }
        public int Mark
        {
            get{ return mnMark; }
            set{ mnMark = value; }
        }
        #endregion ISchedulerState
	}
}
