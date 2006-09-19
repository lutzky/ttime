using System;
//using System.Drawing;
using UDonkey.Logic;


namespace UDonkey
{
	public interface IScheduleObjectBase
	{
		System.Collections.IDictionary Events    { get; }
		// FIXME: color shouldn't be associated with an event this way,
		// since it breaks seperation of GUI and logic.
		// perhaps we need some kind of polymorphic colorizer.
		//System.Drawing.Color           ForeColor { get; }
		//System.Drawing.Color           BackColor { get; }
		bool						   UserDefined { get; }
		string                         ToString();
        string                         ToString( VerbosityFlag flag );
	}
	public interface IScheduleObject : IScheduleObjectBase
	{
		string    Key         { get; }
		Schedule  Schedule    { get; set; }
		DayOfWeek DayOfWeek   { get; set; }
		int       StartHour   { get; set; }
		int       Duration	  { get; set; }
		new bool  UserDefined { get; set; }
		//new System.Drawing.Color ForeColor { get; set; }
		//new System.Drawing.Color BackColor { get; set; }
		VerbosityFlag Verbosity { get; set; }
	}

	public interface IScheduleEntry: IScheduleObjectBase, System.Collections.IEnumerable
	{
		void Add( string key, IScheduleObject obj );
		bool ContainsKey( string key );
		void Remove( string key );
		bool IsEmpty     { get; }
		int  Count       { get; }
		IScheduleObject this[ string key ] { get; }
	}

	public interface ICourseIterator
	{
		Course Course { get; }
		bool Previous();
		bool Next();
		bool GoTo( int index );
		int         Count    { get; }   
        int         Branch   { get;  set; }
		int		    Index    { get; }
		CourseEvent Lecture  { get; }
		CourseEvent Tutorial { get; }
		CourseEvent Lab      { get; }
		CourseEvent Project  { get; }
	}
	public interface ISchedulerState
	{
		void  CalculteStatistics( Schedule schedule );
		int[] Indexes { get; }
		System.Collections.IList       Statistics        { get; }
        System.Collections.IDictionary StatisticsResults { get; }
        System.Collections.IDictionary StatisticsMarks   { get; }
        int Mark                                         { get;  set; }
	}
   
    public interface IScheduleStatistic
    {
        void  Calculate( Schedule schedule );
        string Name  { get; }
        float Result { get; }
        /// <summary>
        /// A Mark between 0 (lowest) to 100(best) for the result
        /// </summary>
        int   Mark   { get; }
		/// <summary>
		/// State if the statistic is possetive (good) or negative (bad)
		/// </summary>
		bool  Positive { get; set; }
    }

	public interface ISchedulerConstraint
	{
		/// <summary>
		/// Check the schedule if the contraint is fullfilled
		/// </summary>
		/// <param name="schedule">To check</param>
		/// <returns></returns>
		bool Check( Schedule schedule );
		/// <summary>
		/// Indicates if this contraint should be checked
		/// </summary>
		bool Set { get; set; }
		/// <summary>
		/// Indicates what is the ConstraintCheckType mode
		/// </summary>
		ConstraintCheckType CheckType { get; set; }
		/// <summary>
		/// Key to identify this Constraint
		/// </summary>
		string    Key { get; }
	}

}
