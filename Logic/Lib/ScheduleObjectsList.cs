using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ScheduleObjectList.
	/// </summary>
	public class ScheduleObjectsList: SpecializedSortedList
	{
		public ScheduleObjectsList(): base(typeof(string),typeof(IScheduleObject))
		{
		}
		public ScheduleObjectsList( int capacity ):
			base( typeof(string),typeof(IScheduleObject), capacity )
		{
		}
	}
}
