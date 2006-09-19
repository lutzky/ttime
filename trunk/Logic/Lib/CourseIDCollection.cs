using System;
using System.Collections;

namespace UDonkey.Logic
{
    /// <summary>
    /// Summary description for CourseIDCollection.
    /// </summary>
    public class CourseIDCollection: SpecializedArrayList
    {
        public CourseIDCollection(): base(typeof(CourseID))
        {
        }
        public CourseIDCollection( int capacity ):
            base( typeof(Course), capacity )
        {
        }

    }
}
