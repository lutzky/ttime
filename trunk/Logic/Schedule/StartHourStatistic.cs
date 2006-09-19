using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for StartHourStatisitc.
	/// </summary>
	public class StartHourStatistic: AbstractScheduleStatistic
	{
        #region AbstractScheduleStatistic
        public override string Name
        {
            get{ return "StartHourStatistic"; }
        }
        public override void CalculateDayData( ScheduleDayData data )
        {
            if( !data.IsFreeDay )
            {
                mnResult += data.StartHour;
            }
        }
        public override float FinalCalculation()
        {
            return mnResult / (float)(mnNumberOfDays - mnNumberOfFreeDays);
        }
        public override int NormalizeResult( float result )
        {
            //Later is better
            return (int)(result * 100 / 23);
        }
        #endregion AbstractScheduleStatistic
	}
}
