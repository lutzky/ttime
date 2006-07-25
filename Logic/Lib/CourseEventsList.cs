using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventsSortedList.
	/// </summary>
	public class CourseEventsList: SpecializedSortedList
	{
		public CourseEventsList(): base(typeof(int),typeof(CourseEvent))
		{
		}
		public CourseEventsList( int capacity ):
			base( typeof(int),typeof(CourseEvent), capacity )
		{
		}
	}
}
