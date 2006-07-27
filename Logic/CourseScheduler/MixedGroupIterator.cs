using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for MixedGroupIterator.
	/// </summary>
	public class MixedGroupIterator : AbstractCourseIterator
	{
		#region Protected Variables
		private int mnLecturesNum;
		private int mnTutorialsNum;
		private int mnLabsNum;
		private int mnProjectsNum;

		private int mnLectureIndex;
		private int mnTutorialIndex;
		private int mnLabIndex;
		private int mnProjectIndex;

		private int mnNumberOfGroups;
		#endregion Protected Variables

		public MixedGroupIterator( Course course ): 
			base( course )
		{
			this.mnLecturesNum	= this.Course.Lectures.Count;
			this.mnTutorialsNum = this.Course.Tutorials.Count;
			this.mnLabsNum		= this.Course.Labs.Count;
			this.mnProjectsNum	= this.Course.Projects.Count;

			this.mnLectureIndex		= 0;
			this.mnTutorialIndex	= 0;
			this.mnLabIndex			= 0;
			this.mnProjectIndex		= 0;

			mnNumberOfGroups = this.NumberOfGroups;

			mnIndex = -1;

		}
		#region MixedGroupIterator
		public override bool Previous()
		{
			return this.GoTo( mnIndex - 1 );
		}

		public override bool Next()
		{
            return this.GoTo( mnIndex + 1 );
            /*
			if ( mnLecturesNum !=0 && mnLectureIndex < mnLecturesNum ) 
				mnLectureIndex++;
			else
			{
				mnLectureIndex = 0;
				if ( mnTutorialsNum != 0 && mnTutorialIndex < mnTutorialsNum )
					mnTutorialIndex++;
				else
				{
					mnTutorialIndex = 0;
					if ( mnLabsNum != 0 && mnLabIndex < mnLabsNum ) 
						mnLabIndex++;
					else
					{
						mnLabIndex = 0;
						if ( mnProjectsNum != 0 && mnProjectIndex < mnProjectsNum ) 
							mnProjectIndex++;
						else
						{
							mnProjectIndex = 0;					
							Reset();
							return false;
						}
					}
				}
			}
			++mnIndex;
			Set();
			return true;*/
		}
		public override bool GoTo( int index )
		{
			if ( index < 0 || index >= mnNumberOfGroups )
			{//Index is between 0 to (mnNumberOfGroups - 1) (Zero based)
				if ( mnNumberOfGroups == 0 && index == 0 )
				{//Empty Course can have 1 next only
					mnIndex = index;
					return true;
				}
				Reset();
				return false;
			}
			else
			{
				mnIndex = index;
				if( mnLecturesNum != 0 )
				{
					mnLectureIndex = index % mnLecturesNum;
					index /= mnLecturesNum; 
				}

				if( mnTutorialsNum != 0 )
				{
					mnTutorialIndex = index % mnTutorialsNum;
					index /=  mnTutorialsNum; 
				}

				if( mnLabsNum != 0 )
				{
					mnLabIndex = index % mnLabsNum;
					index /=  mnLabsNum; 
				}
				
				if( mnProjectsNum != 0 )
				{
					mnProjectIndex = index % mnProjectsNum;
					index /=  mnProjectsNum; 
				}
				
				Set();

				return true;
			}
		}
		public override int  Count
		{
			get{ return NumberOfGroups; }
		}
		#endregion MixedGroupIterator

		private void Reset()
		{
			mnIndex = -1;

			this.mnLectureIndex		= 0;
			this.mnTutorialIndex	= 0;
			this.mnLabIndex			= 0;
			this.mnProjectIndex		= 0;

			mLecture  = null; 
			mTutorial = null;
			mLab      = null;
			mProject  = null;
		}
		private void Set()
		{
			if ( mnLecturesNum  != 0 )
				mLecture  = (CourseEvent)this.Course.Lectures.GetByIndex(mnLectureIndex); 
			if ( mnTutorialsNum != 0 )
				mTutorial = (CourseEvent)this.Course.Tutorials.GetByIndex(mnTutorialIndex);
			if ( mnLabsNum      != 0 )
                mLab      = (CourseEvent)this.Course.Labs.GetByIndex(mnLabIndex);
			if ( mnProjectsNum  != 0 )
				mProject  = (CourseEvent)this.Course.Projects.GetByIndex(mnProjectIndex);
		}
		private int NumberOfGroups
		{
			get
			{
				return MultiplyNonZeros( mnLecturesNum,
					mnTutorialsNum,
					mnLabsNum,
					mnProjectsNum);
			}
		}

		/// <summary>
		/// Return the multi of all non-zero ints in arg, Returns zero if all are 0
		/// </summary>
		/// <param name="args">ints array</param>
		/// <returns>The sum</returns>
		private int MultiplyNonZeros( params int[] args )
		{
			foreach( int i in args )
			{
				if( i != 0 )
				{
					int ret = 1;
					foreach( int j in args )
					{
						ret *= ( j != 0 )? j : 1;
					}
					return ret;
				}
			}
			//No none-zero ints
			return 0;
		}
	}
}
