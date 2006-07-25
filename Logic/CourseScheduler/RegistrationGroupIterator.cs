using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for RegistrationGroupIterator.
	/// </summary>
	public class RegistrationGroupIterator : AbstractCourseIterator
	{

		private RegistrationGroup mRegistrationGroup;
		
		public RegistrationGroupIterator( Course course ): 
			base( course )
		{
			this.GoTo( mnIndex );
		}
		#region AbstractCourseIterator
		public override    bool Previous()
		{
			return GoTo( --mnIndex );
		}

		public override    bool Next()
		{
			return GoTo( ++mnIndex );
		}
		public override bool GoTo( int index )
		{
			if ( index < 0 || index >= mCourse.RegistrationGroups.Count )
			{
				if ( mCourse.RegistrationGroups.Count == 0 && index == 0 )
				{//Empty course can have one Next
					mnIndex = index;
					return true;
				}
				//mnIndex =( index < 0)? -1 : mCourse.RegistrationGroups.Count;
				mnIndex = -1;
				mRegistrationGroup = null;
				mLecture  = null; 
				mTutorial = null;
				mLab      = null;
				mProject  = null;
				return false;
			}
			else
			{
				mnIndex = index;
				mRegistrationGroup = (RegistrationGroup)mCourse.RegistrationGroups[ mnIndex ];
				mLecture  = mRegistrationGroup.Lecture; 
				mTutorial = mRegistrationGroup.Tutorial;
				mLab      = mRegistrationGroup.Lab;
				mProject  = mRegistrationGroup.Project;
				return true;
			}
		}
		public override int  Count
		{
			get{ return mCourse.RegistrationGroups.Count; }
		}
		#endregion AbstractCourseIterator

	}
}
