using System;
using System.Collections;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for CourseEventsCollection.
    /// </summary>
    public class CoursesCollection: SpecializedArrayList
    {
        public CoursesCollection(): base(typeof(Course))
        {
        }
        public CoursesCollection( int capacity ):
            base( typeof(Course), capacity )
        {
        }

    }
}
