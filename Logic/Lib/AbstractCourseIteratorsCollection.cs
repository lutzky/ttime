using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventsCollection.
	/// </summary>
	public class AbstractCourseIteratorsCollection: SpecializedArrayList
	{
		public AbstractCourseIteratorsCollection(): base(typeof(AbstractCourseIterator))
		{
		}
		public AbstractCourseIteratorsCollection( int capacity ):
			base( typeof(AbstractCourseIterator), capacity )
		{
		}

	}
}
