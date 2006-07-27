using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for FreeDaysContraint.
	/// </summary>
	public class FreeDaysConstraint: AbstractConstraint	 
	{
		private const string KEY = "FreeDaysConstraint";
		public FreeDaysConstraint()
		{
			this.CheckType = ConstraintCheckType.Post;
		}

		public override bool Check(Schedule schedule)
		{
			base.Check (schedule);
			int count = 0;
			foreach( System.Collections.DictionaryEntry entry in schedule.DaysData )
			{
				ScheduleDayData day = (ScheduleDayData)entry.Value;
				if( day.IsFreeDay )
					++count;
			}
			int number = ( NumberOfFreeDays + 2 > 5 )? NumberOfFreeDays : NumberOfFreeDays + 2;
			return ( count >= 3 + NumberOfFreeDays );
		}
		public int NumberOfFreeDays
		{
			get{ return Configuration.Get( Configuration.CONSTRAINTS,"MinFreeDays", 1 ); }
			set{ Configuration.Set( Configuration.CONSTRAINTS,"MinFreeDays", value ); }
		}

		public override string Key
		{
			get
			{
				return KEY;
			}
		}

	}
}
