using System;
using System.Collections;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for CourseEventCollection.
    /// </summary>
    public class CourseEventCollection: SpecializedArrayList
    {
        public CourseEventCollection(): base(typeof(CourseEvent))
        {
        }
        public CourseEventCollection( int capacity ):
            base( typeof(CourseEvent), capacity )
        {
        }

    }
}
