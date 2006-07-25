using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for StartHourStatisitc.
	/// </summary>
	public class FreeDaysStatistic: AbstractScheduleStatistic
	{
		#region AbstractScheduleStatistic
		public override string Name
		{
			get{ return "FreeDaysStatistic"; }
		}
		public override void CalculateDayData( ScheduleDayData data )
		{
			//No need to to anything
		}
		public override float FinalCalculation()
		{
			return mnNumberOfFreeDays;
		}
		public override int NormalizeResult( float result )
		{
			//Earlier is better
			float mark = (result - 2)  * 100 / 5;
			return (int)( ( mark < 0 )? 0 : mark );
		}
		#endregion AbstractScheduleStatistic
	}
}
