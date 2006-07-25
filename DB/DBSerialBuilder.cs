using System;
using System.Xml;

namespace UDonkey.DB
{
	/// <summary>
	/// This class receives information from the REP parser, and creates an XML data file
	/// with the course information.
	/// </summary>
	public class DBSerialBuilder
	{
		private static XmlTextWriter DBWriter;
		private static int mLectureNum;
		private static int mLastRegNum;

		private DBSerialBuilder()
		{
		}

		/// <summary>
		/// This needs to be called before starting the building.  It creates the blank XML file
		/// and sets the parameters for the writing.
		/// </summary>
		/// <param name="fileName">The filename to create the XML by</param>
		public static void startSerialBuild( string fileName)
		{
			DBWriter = new XmlTextWriter (fileName, null);

			DBWriter.Formatting = Formatting.Indented;
			DBWriter.Indentation= 6;
			DBWriter.Namespaces = false;
			
			try
			{
				DBWriter.WriteStartDocument();  // Write the XML header element
				DBWriter.WriteStartElement("", "CourseDB", ""); // Write the root element <CourseDB>
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

		/// <summary>
		/// Call this before any Faculty.  Writes The faculty opening node
		/// </summary>
		/// <param name="facultyName">The Faculty name</param>
		/// <param name="semester">The semester for which it is updated</param>
		public static void startFaculty(string facultyName, string semester)
		{
			if (facultyName == "רפואית") // This is a quick hack to overcome a bug in the REP parser
				facultyName = "הנדסה ביו-רפואית";
			try
			{
				DBWriter.WriteStartElement("", "Faculty", "");
				DBWriter.WriteAttributeString("name", facultyName);
				DBWriter.WriteAttributeString("semester", semester);
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

		/// <summary>
		/// Call this before any course
		/// </summary>
		/// <param name="courseName">The Course Name</param>
		/// <param name="courseNumber">The Course Number</param>
		/// <param name="courseAcademicPoints">Academic Points</param>
		/// <param name="lectureHours">Number of lecture hours</param>
		/// <param name="tutorialHours">Number of tutorial hours</param>
		/// <param name="labHours">Number of lab hours</param>
		/// <param name="projectHours">nuber of project hours</param>
		public static void startCourse(string courseName, 
									   string courseNumber, 
									   string courseAcademicPoints,
									   string lectureHours,
									   string tutorialHours,
									   string labHours,
									   string projectHours)
        {
			try
			{
				mLectureNum=10; // Initialize Lecture group number counter
				mLastRegNum=11; // Initialize Last Registration Group Number
				DBWriter.WriteStartElement("", "Course", "");
				DBWriter.WriteAttributeString("name", courseName);
				DBWriter.WriteAttributeString("number", courseNumber);
				DBWriter.WriteAttributeString("courseAcademicPoints", courseAcademicPoints);
				if ( lectureHours.Length != 0 )
					DBWriter.WriteAttributeString("lectureHours", lectureHours);
				if ( tutorialHours.Length != 0 )
					DBWriter.WriteAttributeString("tutorialHours", tutorialHours);
				if ( labHours.Length != 0 )
					DBWriter.WriteAttributeString("labHours", labHours);
				if ( projectHours.Length != 0 )
					DBWriter.WriteAttributeString("projectHours", projectHours);
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}
		
		/// <summary>
		/// This is actually part of the course node attributes, but is called seperately
		/// because the structure of the rep file was such that it was easier this way.
		/// </summary>
		/// <param name="lecturerInCharge">Lecturer in Charge</param>
		/// <param name="moedADate">Moed A test date</param>
		/// <param name="moedBDate">Moed B test date</param>
		public static void addCourseDetails(string lecturerInCharge,
									 string moedADate,
									 string moedBDate)
		{
			try
			{
				if ( lecturerInCharge.Length != 0)
					DBWriter.WriteAttributeString("lecturerInCharge", lecturerInCharge);
				if ( moedADate.Length != 0)
					DBWriter.WriteAttributeString("moedADate", moedADate);
				else 
					DBWriter.WriteAttributeString("moedADate", DateTime.MinValue.ToShortDateString());
				if ( moedBDate.Length != 0 )
					DBWriter.WriteAttributeString("moedBDate", moedBDate);
				else 
					DBWriter.WriteAttributeString("moedBDate", DateTime.MinValue.ToShortDateString());
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

	
		/// <summary>
		/// Call this before any course event starts
		/// </summary>
		/// <param name="regNumber">The registration group number</param>
		/// <param name="eventType">Lecture/Tutorial/Lab/Project</param>
		/// <param name="teacher">The teacher of this event</param>
		public static void startGroupEvent(	string regNumber,
											string eventType,
											string teacher)
		{
			try
			{
				DBWriter.WriteStartElement("", "CourseEvent", "");
				if (eventType.Length==0)
				{
					eventType="הרצאה";
				}
				if (eventType.Equals("הרצאה")) // Event is a lecture.  Need to feed reg num manually
				{
					regNumber=mLectureNum.ToString();
					mLastRegNum=mLectureNum;
					mLectureNum+=10;
				}
				else // Event is not a lecture.  Need to check if we got a reg number.
				{
					if (regNumber.Length == 0) // Event is not lecture and no reg num was given
					{
						regNumber=mLastRegNum.ToString();
					}
					else // Event is not a lecture, and we got a new reg num.
					{
						mLastRegNum=int.Parse(regNumber);  // Update last registration group number
					}
				}
				
				DBWriter.WriteAttributeString("regNumber", regNumber);
				DBWriter.WriteAttributeString("eventType", eventType);
				DBWriter.WriteAttributeString("teacher", teacher);
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}

		}

		/// <summary>
		/// Add an occurence to the current event
		/// </summary>
		/// <param name="EventDay">The day the occurence is on</param>
		/// <param name="StartTime">Starting time in HH.MM format</param>
		/// <param name="Duration">Duration in hours</param>
		/// <param name="Location">Where the occurence is held</param>
		public static void addPlaceTime(string EventDay,
										string StartTime,
										string Duration,
										string Location)
		{
			DBWriter.WriteStartElement("", "PlaceTime", "");

			DBWriter.WriteAttributeString("EventDay", EventDay);
			DBWriter.WriteAttributeString("EventTime", StartTime);
			DBWriter.WriteAttributeString("EventDuration", Duration);
			DBWriter.WriteAttributeString("EventLocation", Location);

			DBWriter.WriteEndElement(); // End Placetime
		}
	
		/// <summary>
		/// Close a course event
		/// </summary>
		public static void closeGroupEvent()
		{
			try
			{
				DBWriter.WriteEndElement(); // End GroupEvent
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

		/// <summary>
		/// Close a course
		/// </summary>
		public static void closeCourse()
		{
			try
			{
				DBWriter.WriteEndElement(); // End Course
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

		/// <summary>
		/// Close a faculty
		/// </summary>
		public static void closeFaculty()
		{
			try
			{
				DBWriter.WriteEndElement(); // End Faculty
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
		}

		/// <summary>
		/// Call this after you finish adding all the information to the database
		/// </summary>
		public static void endSerialBuild()
		{
			try
			{
				DBWriter.WriteEndDocument(); // Close all open tags
				DBWriter.Flush(); // Flush everything in the buffers to the file
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
			if (DBWriter != null)
			{
				DBWriter.Close(); // Close the writer
			}
		}

	}
}
