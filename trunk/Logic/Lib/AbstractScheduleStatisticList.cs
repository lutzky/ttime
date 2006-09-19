using System;
using System.Collections;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for CourseEventsCollection.
    /// </summary>
    public class AbstractScheduleStatisticList: SpecializedSortedList
    {
        public AbstractScheduleStatisticList(): base(typeof(string),typeof(AbstractScheduleStatistic))
        {
        }
        public AbstractScheduleStatisticList( int capacity ):
            base(typeof(string),typeof(AbstractScheduleStatistic), capacity )
        {
        }

    }
}
