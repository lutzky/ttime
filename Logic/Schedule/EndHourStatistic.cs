using System;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for StartHourStatisitc.
    /// </summary>
    public class EndHourStatistic: AbstractScheduleStatistic
    {
        #region AbstractScheduleStatistic
        public override string Name
        {
            get{ return "EndHourStatistic"; }
        }
        public override void CalculateDayData( ScheduleDayData data )
        {
            if( !data.IsFreeDay )
            {
                mnResult += data.EndHour;
            }
        }
        public override float FinalCalculation()
        {
            return mnResult / (float)(mnNumberOfDays - mnNumberOfFreeDays);
        }
        public override int NormalizeResult( float result )
        {
            //Earlier is better
            return (int)((23 - result) * 100 / 23);
        }
        #endregion AbstractScheduleStatistic
    }
}
