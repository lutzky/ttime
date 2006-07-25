using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ConstraintsCollection.
	/// </summary>
	public class SchedulerStatesCollection: SpecializedArrayList
	{

		#region Private Variables
		private int								    mnMaxGrade;
		private int								    mnMinGrade;
		#endregion Private Variables

		public SchedulerStatesCollection(): base(typeof(SchedulerState))
		{
		}
		public SchedulerStatesCollection( int capacity):
			base( typeof(SchedulerState), capacity )
		{
		}
		public int MaxGrade
		{
			get{ return mnMaxGrade; }
			set{ mnMaxGrade = value; }
		}
		public int MinGrade
		{
			get{ return mnMinGrade; }
			set{ mnMinGrade = value; }
		}
	
	}
}
