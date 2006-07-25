using System;

namespace UDonkey.Logic
{
    /// <summary>
    ///TODO: Summary description for RegistrationGroupCollection.
    /// </summary>
    public class ScheduleDayDataCollection : SpecializedArrayList
    {
        public ScheduleDayDataCollection(): base(typeof(ScheduleDayData))
        {
        }
        public ScheduleDayDataCollection( int capacity):
            base( typeof(ScheduleDayData), capacity )
        {
        }
    }
}
