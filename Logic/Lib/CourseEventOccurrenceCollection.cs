using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventsCollection.
	/// </summary>
	public class CourseEventOccurrenceCollection: SpecializedArrayList
	{
        public CourseEventOccurrenceCollection(): base(typeof(CourseEventOccurrence))
        {
        }
        public CourseEventOccurrenceCollection( int capacity):
            base( typeof(CourseEventOccurrence), capacity )
        {
        }
	}
}
