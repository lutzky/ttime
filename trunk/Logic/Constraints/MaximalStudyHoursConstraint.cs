using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for MinimalStudyHoursConstraint.
	/// </summary>
	public class MaximalStudyHoursConstraint: AbstractConstraint	 
	{
		private const string KEY = "MaximalStudyHoursConstraint";

		public MaximalStudyHoursConstraint()
		{
			this.CheckType = ConstraintCheckType.Post;
		}

		public override bool Check(Schedule schedule)
		{
			base.Check (schedule);
			int  count=0;
			int maxHours = NumberOfStudyHours;
			foreach( System.Collections.DictionaryEntry entry in schedule.DaysData )
			{
				ScheduleDayData day = (ScheduleDayData)entry.Value;
				if( !day.IsFreeDay )
				{
					count = 0;
					for( int i = day.StartHour; i < day.EndHour + 1 ; ++i )
					{
						IScheduleEntry sEntry = day.GetScheduleObject( i );  
						if( !sEntry.IsEmpty && !sEntry.UserDefined )
						{
							++count;
						}
						if ( count > maxHours )
						{
							return false;
						}
					}
				}	
			}
			foreach( System.Collections.DictionaryEntry entry in schedule.DaysData )
			{
				ScheduleDayData day = (ScheduleDayData)entry.Value;
				if( !day.IsFreeDay )
				{
					count = 0;
					for( int i = day.StartHour; i < day.EndHour + 1 ; ++i )
					{
						IScheduleEntry sEntry = day.GetScheduleObject( i );  
						if( !sEntry.IsEmpty && !sEntry.UserDefined )
						{
							++count;
						}
						if ( count > NumberOfStudyHours )
						{
							return false;
						}
					}
				}	
			}
			return true;
		}
		public int NumberOfStudyHours
		{
			get{ return Configuration.Get( Configuration.CONSTRAINTS,"MaxDailyHours", 1 ); }
			set{ Configuration.Set( Configuration.CONSTRAINTS,"MaxDailyHours", value ); }
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
