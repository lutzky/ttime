using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CoursesList.
	/// </summary>
	public class CoursesList: SpecializedSortedList
	{
		public CoursesList(): base(typeof(string),typeof(Course))
		{
		}
		public CoursesList( int capacity ):
			base( typeof(string),typeof(Course), capacity )
		{
		}
	}
}
