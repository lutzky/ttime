using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventsCollection.
	/// </summary>
	public class AbstractCourseIteratorsList: SpecializedSortedList
	{
		public AbstractCourseIteratorsList(): base(typeof(string),typeof(AbstractCourseIterator))
		{
		}
		public AbstractCourseIteratorsList( int capacity ):
			base( typeof(string), typeof(AbstractCourseIterator), capacity )
		{
		}

	}
}
