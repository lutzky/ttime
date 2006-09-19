using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for UserEventConstraint.
	/// </summary>
	public class UsersEventConstraint: AbstractConstraint
	{
		private UsersEventScheduleObject mScheduleObject;
		
		public UsersEventConstraint( UsersEventScheduleObject obj )
		{
			this.CheckType  = ConstraintCheckType.None;
			mScheduleObject = obj;
		}
        #region AbstractConstraint
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

        #endregion AbstractConstraint
		public UsersEventScheduleObject UserEvent
		{
			get{ return mScheduleObject; }
		}
	}
}
