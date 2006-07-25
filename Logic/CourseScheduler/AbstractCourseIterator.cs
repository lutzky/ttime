using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseIterator.
	/// </summary>
	public abstract class AbstractCourseIterator : ICourseIterator
	{
		#region Protected Variables
		protected Course	  mCourse;
		protected int		  mnIndex;
        protected int         mnBranch;
		protected CourseEvent mLecture;
		protected CourseEvent mTutorial;
		protected CourseEvent mLab;
		protected CourseEvent mProject;
		#endregion Protected Variables
		public AbstractCourseIterator( Course course )
		{
			mCourse = course;
			mnIndex = -1;
		}

		#region ICourseIterator Members
		/// <summary>
		/// The Course that this CourseIterator runs on.
		/// </summary>
		public Course Course
		{
			get{ return mCourse; }
		}

		public abstract bool Previous();
		public abstract bool Next();
		public abstract bool GoTo( int index );
		public abstract int  Count { get; }
		/// <summary>
		/// Return the Iterator's index.
		/// </summary>
		public int Index
		{
			get{ return mnIndex; }
		}
        public int Branch
        {
            get{ return mnBranch; }
            set{ mnBranch = value; }
        }
		/// <summary>
		/// Returns all the CourseEvents of this Iterator
		/// </summary>
		public CourseEventCollection CourseEvents
		{
			get
			{
				CourseEventCollection col = new CourseEventCollection();
				col.Add( mLecture  );
				col.Add( mTutorial );
				col.Add( mLab      );
				col.Add( mProject  );
				return col;
			}
		}
		/// <summary>
		/// The lecture of this CourseIterator
		/// </summary>
		public CourseEvent Lecture
		{
			get{ return mLecture; }
		}
		/// <summary>
		/// The tutorial of this CourseIterator
		/// </summary>
		public CourseEvent Tutorial
		{
			get{ return mTutorial; }
		}
		/// <summary>
		/// The lab of this CourseIterator
		/// </summary>
		public CourseEvent Lab
		{
			get{ return mLab; }
		}
		/// <summary>
		/// The project of this CourseIterator
		/// </summary>
		public CourseEvent Project
		{
			get{ return mProject; }
		}
		#endregion
	}
}
