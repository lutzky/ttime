using System;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for StartHourStatisitc.
    /// </summary>
    public class HolesStatistic: AbstractScheduleStatistic
    {
        #region AbstractScheduleStatistic
        public override string Name
        {
            get{ return "HolesStatistic"; }
        }
        public override void CalculateDayData( ScheduleDayData data )
        {
            if( !data.IsFreeDay )
            {
                mnResult += data.Holes;
            }
        }
        public override float FinalCalculation()
        {
            return mnResult;
        }
        public override int NormalizeResult( float result )
        {
            int maxHoles = 15;
            //Less Holes are better
 
            return (int)( (result > maxHoles)? 0 : (maxHoles - result ) * 100 / maxHoles );
        }
        #endregion AbstractScheduleStatistic
    }
}
