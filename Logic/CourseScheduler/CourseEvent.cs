using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// CourseEvent represent one event (of any type) of a Course
	/// </summary>
	public class CourseEvent
	{
		private Course							mCourse;
		private string						    mType;
		private string						    mGiver;
		private int								mEventNum;
		private CourseEventOccurrenceCollection mOccurrences;

		public CourseEvent( Course theCourse, string type, string giver, int eventNum )
		{
			mCourse		 = theCourse;
			mType		 = type;
			mGiver		 = giver;
			mEventNum	 = eventNum;
			mOccurrences = new CourseEventOccurrenceCollection();
		}

		/// <summary>
		/// Return the Course of this CourseEvent
		/// </summary>
		public Course				   Course
		{
			get{ return mCourse; }
			set{ mCourse = value; }
		}
	
		public int						EventNum
		{
			get { return mEventNum; }
			set { mEventNum = value; }
		}
		/// <summary>
		/// Return the CourseEventType of this CourseEvent
		/// </summary>
		public string				   Type
		{
			get{ return mType; }
			set{ mType = value; }
		}

		/// <summary>
		/// Return the CourseEvent's giver 
		/// </summary>
		public string						   Giver
		{
			get{ return mGiver; }
			set{ mGiver = value; }
		}
		/// <summary>
		/// Return all the occurrences of this event
		/// </summary>
		public CourseEventOccurrenceCollection Occurrences
		{
			get{ return mOccurrences; }
		}

		public override bool Equals(object obj)
		{
			CourseEvent to = (CourseEvent) obj;
			int i;
			if (this.mType.Equals(to.mType)&&(this.Occurrences.Count==to.Occurrences.Count))
			{
				for (i=0;i<this.Occurrences.Count;i++)
				{
					CourseEventOccurrence event1 = (CourseEventOccurrence)this.Occurrences[i];
					CourseEventOccurrence event2 = (CourseEventOccurrence)to.Occurrences[i];
					if (!event1.Equals(event2))
						return false;
				}
				return true;
			}
			else
				return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return this.Type + " " + this.EventNum.ToString() + " " + this.Giver;
		}



	}
}
