using System;
using System.Collections;
namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ScedulerStateComparer.
	/// </summary>
	public class CourseIteratorComparer: System.Collections.IComparer
	{
		#region IComparer Members
		public int Compare(object x, object y)
		{
			if ( !( x is ICourseIterator && y is ICourseIterator ) )
				throw new ArgumentException( "x,y must be of type ICourseIterator", "x,y" );
			
			ICourseIterator X = x as ICourseIterator;
			ICourseIterator Y = y as ICourseIterator;

			return X.Count - Y.Count;
		}
		#endregion
	}
}
