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
       Hour=0,
       Sunday= 1,
       Monday,
       Tuesday ,
       Wednsday ,
       Thursday ,
       Friday ,
       Saturday 
        
    };

    public enum CourseEventType //boazg: hebrew FUBAR'd, guessing
    {
        Lecture,
        Tutorial,
        SomeThingElse,
        NoIdea
        
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
