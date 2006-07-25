using System;

namespace UDonkey
{

    public enum ScheduleObjectType
    {
        Empty,
        Free,
        Busy
    }
    
    public enum DayOfWeek
    {
        שעות   = 0,
        ראשון = 1,
        שני,
        שלישי,
        רביעי,
        חמישי,
        שישי,
        שבת
    };

    public enum CourseEventType
    {
        הרצאה,
        תרגול,
        פרויקט,
        מעבדה
    }

	public enum VerbosityFlag: long
	{
		EventNumber   = 1,
		CourseNumber  = 2,
		CourseName    = 4,
		EventType     = 8,
		EventGiver    = 16,
		EventLocation = 32,
		Minimal,      
	    Middle,     
	    Full
	}

	public enum ConstraintCheckType
	{
		Pre,
		Middle,
		Post,
		None
	}

    public enum ScheduleErrors
    {
        FreeDay,
        StartHour,
        EndHour,
        UserEvent,
        Overlaps
    }

}
