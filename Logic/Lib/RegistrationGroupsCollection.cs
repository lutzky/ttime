using System;

namespace UDonkey.Logic
{
	/// <summary>
	///TODO: Summary description for RegistrationGroupCollection.
	/// </summary>
    public class RegistrationGroupsCollection : SpecializedArrayList
    {
        public RegistrationGroupsCollection(): base(typeof(RegistrationGroup))
        {
        }
        public RegistrationGroupsCollection( int capacity):
            base( typeof(RegistrationGroup), capacity )
        {
        }
    }
}
