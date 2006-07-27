using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for FreeDaysContraint.
	/// </summary>
	public class ScheduleObjectConstraint: AbstractConstraint	 
	{
		private IScheduleObject mScheduleObject;

		public ScheduleObjectConstraint(IScheduleObject scheduleObject)
		{
			this.CheckType = ConstraintCheckType.Post;
			mScheduleObject = scheduleObject;
		}

		public override bool Check(Schedule schedule)
		{
			return schedule.ContainsScheduleObject( mScheduleObject );
		}
		
		public override string Key
		{
			get
			{
				return mScheduleObject.Key;
			}
		}
	}
}