using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventsCollection.
	/// </summary>
	public class AbstractConstraintList: SpecializedSortedList
	{
		public AbstractConstraintList(): base(typeof(string), typeof(AbstractConstraint))
		{
		}
		public AbstractConstraintList( int capacity ):
			base(typeof(string), typeof(AbstractConstraint), capacity )
		{
		}

	}
}
