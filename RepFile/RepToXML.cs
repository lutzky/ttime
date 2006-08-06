using System;
using System.Text.RegularExpressions;
using UDonkey.DB;

namespace UDonkey.RepFile
{
	/// <summary>
	/// Summary description for RepToXML.
	/// </summary>
	public class RepToXML
	{
		public delegate void ConvertProgress( int precentage );
		public static event ConvertProgress Progress;
		public static event EventHandler    StartConvertion;
		#region Regular Expressions Constants
		/// <summary>
		/// TODO
		/// Regex for:
		/// (Faculty:
		/// 
		/// +=====+
		/// | XXX |
		/// +=====+
		///   XXX )*
		/// </summary>
		private const string FILE_REGEX = @"";
		#region Faculty
		/// <summary>
		/// Regex for:
		/// +=================+
		/// | Faculties Header|
		/// +=================+
		/// +-----------------+
		/// (Course
		/// +-----------------+
		///        XXX
		/// +-----------------+
		/// +)*
		/// </summary>
		private const string FACULTY_REGEX = 
			@"\+=+\+(?<FacultyHeader>[^+]*)\+=+\+[^+]*\+-+\+" + //Faculty Header
			@"(?<Course>.*?\+-+\+.*?\+-+\+)*"; //Course
		/// <summary>
		/// Regex for:
		/// | FacultyName - XXX |
		/// | Semster סמסטר  |
		/// or
		/// | Semster סמסטר  - FacultyName |
		/// </summary>
		private const string FACULTY_HEADER_REGEX =
			@"\|\s+(?<FacultyName>[^-]+)-.*\|.*\|\s+(?<Semster>.*)רטסמס\s+\|" + 
			"|" +
			@"\|\s+(?<Semster>.*)רטסמס - (?<FacultyName>[^|]+)\|"; 
   
		private const string FACULTY_SEPERATOR = "\r\n\r\n";
		#endregion Faculty
		#region Course
		/// <summary>
		/// Regex for:
		/// +--------------+
		/// | CourseHeader |
		/// +--------------+
		/// | CourseParams |
		/// </summary>
		private const string COURSE_REGEX =
			@"(?<CourseHeader>.*)\+-+\+(?<CourseParams>.*)\+-+\+";
		/// <summary>
		/// Regex for:
		/// | CourseNumber CourseID                   |
		/// | CourseAcademicPoint:נק CourseHours:XXX  |
		/// </summary>
		private const string COURSE_HEADER = 
			"\\|\\s*(?<CourseName>.*)\\s+(?<CourseNumber>\\d+?)\\s+\\|\r\n" +
			"\\|(?<CourseAcademicPoints>[^:]+):קנ(?<CourseHours>[^:]+).*\\|";
		/// <summary>
		/// Regex for:
		/// | LacturerInCharge : מורה אחראי |
		/// | -+                            |
		/// | FirstTestDate    : מועד ראשון |
		/// | -+                            |
		/// | SecondTestDate     : מועד שני |
		/// | -+                            |
		/// </summary>
		private const string COURSE_BODY   = 
			"\r\n" +
			"(\\|(?<LacturerInCharge>.*): יארחא  הרומ \\|\r\n"+
			"\\|\\s*-+\\s*\\|\r\n)?" +
			"(\\|(?<FirstTestDate>.*)\\s+םוי: ןושאר דעומ \\|\r\n" +
			"\\|\\s*-+\\s*\\|\r\n)?" +
			"(\\|(?<SecondTestDate>.*)םוי:   ינש דעומ \\|\r\n" +
			"\\|\\s*-+\\s*\\|\r\n)?";
        
		/// <summary>
		/// Regex for:
		///(|               ++++++                  .מס|
		/// |                                     רישום| Or
		/// |                                          |)
		/// |(Building) (Room)   Hours'Day (:)Type (No)|
		///(|(Building) (Room)   Hours'Day             |)
		///(|                     GiverName : GiverType|
		/// |                               -----      |)
		/// </summary>
		private const string COURSE_GROUPS =
			"((?<PlaceTime>\\|\\s*([^|]+)?(\\s+\\d+)?\\s+(\\d{1,2}.\\d{2}-\\s?\\d{1,2}.\\d{2}'\\w{1})\\s*\\|\r\n)?" +
			"(?<Header>\\|\\s+\\++\\s+.סמ\\|\r\n" +
			"\\|\\s+םושיר\\|\r\n)" +
			"|" +
			"\\|\\s+\\|\r\n)?" +
			"\\|(\\s*(?<Place>[^|]+)?\\s+(?<DayTime>\\d{1,2}.\\d{2}-\\s?\\d{1,2}.\\d{2}'\\w{1})|\\s*-\\s*)\\s+:?\\s*(?<GroupType>.*?)\\s+(?<Number>\\d{2})?\\s+\\|\r\n" +
			"(?<PlaceTime>\\|\\s*([^|]+)?(\\s+\\d+)?\\s+(\\d{1,2}.\\d{2}-\\s?\\d{1,2}.\\d{2}'\\w{1})\\s*\\|\r\n)*" +
			"(\\|\\s+(?<Giver>[^|]*) : (?<GiverType>\\w+)\\s+\\|\r\n" + 
			"\\|\\s+-+\\s+\\|\r\n)?";
		private const string COURSE_PLACE_TIME =
			"\\|\\s*(?<Place>[^|]+)?\\s+(?<DayTime>\\d{1,2}.\\d{2}-\\s?\\d{1,2}.\\d{2}'\\w{1})\\s*\\|\r\n";
		#endregion Course
		#endregion Regular Expressions Constants
		#region Static Methods
		#region Public Methods
		/*public static void Convert( object fileName, string toFile  )
		{
			RepToXML.Convert( fileName as string );
		}*/
		public static void Convert( string fileName, string toFile )
		{  
			Console.WriteLine("RepToXML.Convert("+fileName+", " + toFile + ")");
			string file = DosHeb.Translate( fileName );
			if (StartConvertion != null) StartConvertion( null , new EventArgs() );
			ConvertFiles( file, toFile );
		}
		#endregion Public Methods
		#region Private Convert Methods
		private static void ConvertFiles( string file, string toFile )
		{
			DBSerialBuilder.startSerialBuild( toFile );
			int currentFaculty = 0;
			int numOfFaculties = 0;

			int lastIndex = 0;
			for( int index = file.IndexOf( FACULTY_SEPERATOR, 0 );
				lastIndex != -1 ;
				index = file.IndexOf(FACULTY_SEPERATOR,lastIndex + FACULTY_SEPERATOR.Length ) )
			{
				int to = ( index == -1 )? file.Length: index;
				numOfFaculties++;  
				lastIndex = index;
			}
            
			if (Progress != null) Progress( 0 );
			lastIndex = 0;
			for( int index = file.IndexOf( FACULTY_SEPERATOR, 0 );
				lastIndex != -1 ;
				index = file.IndexOf(FACULTY_SEPERATOR,lastIndex + FACULTY_SEPERATOR.Length ) )
			{
				int to = ( index == -1 )? file.Length: index;
				ConvertFaculty( file.Substring(lastIndex, to -  lastIndex ) );  
				if (Progress != null) Progress( ++currentFaculty * 100 / numOfFaculties );
				lastIndex = index;
			}
			if (Progress != null) Progress( 100 );

			/*TODO When FILE_REGEX will be comleted
			Regex regex = new Regex( FILE_REGEX, RegexOptions.Singleline );
			Match match = regex.Match( file ) ;
			Group faculty = match.Groups[ "Faculty" ];
			foreach( Capture capture in faculty.Captures )
			{
				result += ConvertFaculty( capture.Value );
			}*/
			DBSerialBuilder.endSerialBuild();
		}
		#region Faculty
		private static void ConvertFaculty( string faculty )
		{         
			faculty = faculty.Trim();
			if ( faculty.Length == 0 )
				return;

			Regex regex = new Regex( FACULTY_REGEX, RegexOptions.Singleline );
			for ( Match match = regex.Match( faculty ) ;
				match != Match.Empty ;
				match = match.NextMatch() )
			{
				ConvertFacultysHeader( match.Groups[ "FacultyHeader" ].Value );
                
				Group course = match.Groups[ "Course" ];
				foreach( Capture capture in course.Captures )
				{
					ConvertCourse( capture.Value );
				}
			}
			DBSerialBuilder.closeFaculty();
		}
		private static void ConvertFacultysHeader( string header )
		{
			Regex regex = new Regex( FACULTY_HEADER_REGEX, RegexOptions.Singleline );
            
			Match match = regex.Match( header );

			string facultyName = DosHeb.Revert(  match.Groups[ "FacultyName" ].Value.Trim() );
			string semster = DosHeb.Revert(  match.Groups[ "Semster" ].Value.Trim() );
            
			DBSerialBuilder.startFaculty( facultyName, semster );
		}
		#endregion Faculty
		#region Course
		private static void ConvertCourse( string course )
		{
			Regex regex = new Regex( COURSE_REGEX, RegexOptions.Singleline );
            
			for ( Match match = regex.Match( course ) ;
				match != Match.Empty ;
				match = match.NextMatch() )
			{
				ConvertCoursesHeader( match.Groups[ "CourseHeader" ].Value );
				ConvertCoursesBody( match.Groups[ "CourseParams" ].Value );
				DBSerialBuilder.closeCourse();
			}
		}
		private static void ConvertCoursesHeader( string header )
		{
			Regex regex = new Regex( COURSE_HEADER );
			Match match = regex.Match( header );
 
			string courseName = DosHeb.Revert(  match.Groups[ "CourseName" ].Value.Trim() );
			string courseNumber = match.Groups[ "CourseNumber" ].Value.Trim();
			string academicPoints = match.Groups[ "CourseAcademicPoints" ].Value.Trim();
			string[] courseHours = ConvertCoursesHourse( DosHeb.Revert( match.Groups[ "CourseHours" ].Value.Trim() ) );
            
			DBSerialBuilder.startCourse( courseName, courseNumber, academicPoints, 
				courseHours[0], courseHours[1],courseHours[2],courseHours[3] );
		}
		private static void ConvertCoursesBody( string body )
		{
			Regex regex = new Regex( COURSE_BODY, RegexOptions.RightToLeft );

			//Find the longest match (starts at the begining)
			Match match;           
			for ( match = regex.Match( body );
				match.Index != 0 && match != Match.Empty;
				match = match.NextMatch() );

			string lacturerInCharge = DosHeb.Revert(  match.Groups[ "LacturerInCharge" ].Value.Trim() );
			string firstTestDate    = match.Groups[ "FirstTestDate" ].Value.Split('\'')[0].Trim();
			string secondTestDate   = match.Groups[ "SecondTestDate" ].Value.Split('\'')[0].Trim();
			DBSerialBuilder.addCourseDetails( lacturerInCharge, firstTestDate, secondTestDate );
            
			ConvertCoursesGroups ( body.Substring( match.Index + match.Length ) );
		}
		private static void ConvertCoursesGroups( string groups )
		{
			Regex regex = new Regex( COURSE_GROUPS );

			for ( Match match = regex.Match( groups ) ;
				match != Match.Empty ;
				match = match.NextMatch() )
			{   
				string header      = DosHeb.Revert( match.Groups[ "Header" ].Value.Trim() );
				string groupType   = DosHeb.Revert( match.Groups[ "GroupType" ].Value.Trim() );
				string[] dayTime   = match.Groups[ "DayTime" ].Value.Trim().Split('\'');
				string day         = string.Empty;
				string start       = string.Empty;
				string duration    = string.Empty;
				if( dayTime[0].Length != 0 )
				{
					day         = dayTime[1];
					start       = GetStartTime( dayTime[0] );
					duration    = GetDuration ( dayTime[0] );
				}
				string place       = GetPlace( match.Groups[ "Place" ].Value.Trim() );
				string giverType   = DosHeb.Revert( match.Groups[ "GiverType"].Value.Trim() );
				string giver       = DosHeb.Revert( match.Groups[ "Giver"].Value.Trim() );
				string regNumber   = match.Groups[ "Number" ].Value.Trim();

				DBSerialBuilder.startGroupEvent( regNumber, groupType, giver );
				DBSerialBuilder.addPlaceTime( day ,start ,duration ,place ); 

				Group events = match.Groups[ "PlaceTime" ];
				foreach( Capture capture in events.Captures )
				{
					ConvertPlaceTime( capture.Value );
				}

				DBSerialBuilder.closeGroupEvent();
			}
		}
		private static void ConvertPlaceTime( string placeTime )
		{
			Regex regex = new Regex( COURSE_PLACE_TIME );
			Match match = regex.Match( placeTime );
			
			if( match == Match.Empty ) return;

			string[] dayTime   = match.Groups[ "DayTime" ].Value.Trim().Split('\'');
			string day         = string.Empty;
			string start       = string.Empty;
			string duration    = string.Empty;
			if( dayTime[0].Length != 0 )
			{
				day         = dayTime[1];
				start       = GetStartTime( dayTime[0] );
				duration    = GetDuration ( dayTime[0] );
			}
			string place       = GetPlace( match.Groups[ "Place" ].Value.Trim() );

			DBSerialBuilder.addPlaceTime( day ,start ,duration ,place ); 			
		}

		private static string[] ConvertCoursesHourse( string hours )
		{
			string[] retHours = new string[] {   string.Empty, 
												 string.Empty, 
												 string.Empty, 
												 string.Empty };
			string[] hoursSplits = hours.Split(' ');
			foreach( string  hour in hoursSplits )
			{
				if ( hour.Length == 0 )
				{
					continue;
				}
				string[] hourSplit = hour.Split('-');
				switch ( hourSplit[0] )
				{
					case "ה":
					{
						retHours[0] = hourSplit[1];
						break;
					}
					case "ת":
					{
						retHours[1] = hourSplit[1];
						break;
					}
					case "מ":
					{
						retHours[2] = hourSplit[1];
						break;
					}
					case "פ":
					{
						retHours[3] = hourSplit[1];
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "hours", hours, "Do know " + hourSplit[0]);
					}
				}
			}
			return retHours;
		}
		private static string   GetStartTime( string time )
		{
			return time.Split('-')[0];
		}
		private static string   GetDuration( string time )
		{
			if ( time.Length == 0 )
			{
				return string.Empty;
			}
			string[] timeSplit = time.Split('-');
			
			int start   = int.Parse( timeSplit[0].Split('.')[1] ) == 30 ?
				int.Parse( timeSplit[0].Split('.')[0] ) :
				int.Parse( timeSplit[0].Split('.')[0] )  - 1;

			int  end = int.Parse( timeSplit[1].Split('.')[0] );

			return (end - start).ToString();
		}
		private static string   GetPlace( string place )
		{
			string[] split = place.Split();
			string ret = string.Empty;
			foreach( string s in split )
			{
				string t = s;
				try
				{
					int i = int.Parse( s );
					ret += string.Format("{0}{1}",
						(ret.Length == 0)?string.Empty:" ", t );
				}
				catch
				{
					//Revert the hebrew string
					t = DosHeb.Revert( s );
					//Add the string at the begining.
					ret = string.Format("{1}{0}{2}",
						(ret.Length == 0)?string.Empty:" ", t , ret);
				}

			}
			return ret;
		}		
        #endregion Course
        #endregion Private Convert Methods
        #endregion Static Methods
        #region Constructors
        /// <summary>
        /// Private Constructor to prevent instances of this class
        /// </summary>
		private RepToXML()
		{
		}
        #endregion Constructors
	}
}
