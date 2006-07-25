using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// The base class for all Constraints
	/// </summary>
	public abstract class AbstractConstraint: ISchedulerConstraint
	{
		private bool mbSet = false;
		private ConstraintCheckType meCheckType;

		public AbstractConstraint()
		{
			mbSet       = false;
			meCheckType = ConstraintCheckType.Middle;
		}

		public virtual bool Check( Schedule schedule )
		{
			if( !Set )
			{
				throw new ArgumentException("The constraint is not set");
			}
			return true;
		}
		/// <summary>
		/// Indicates if this contraint should be checked
		/// </summary>
		public bool Set
		{
			get{ return mbSet;  }
			set{ mbSet = value; }
		}
		
		public ConstraintCheckType CheckType
		{
			get{ return meCheckType; }
			set{ meCheckType = value; }
		}
		public abstract string Key{ get; }
	}
}
