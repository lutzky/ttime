using System;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using UDonkey.Logic;
using UDonkey.GUI;
using System.Collections.Specialized;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Globalization;

namespace UDonkey.DB
{
	/// <summary>
	/// Summary description for CourseDB.
	/// </summary>
	public class CourseDB
	{
		public const string DEFAULT_DB_FILE_NAME = @"MainDB.xml";
		private string m_strFileName; // The Database XML filename
		public const string m_zipFileName = "REPFILE.zip";
		public const string m_zipFileURI  = "http://ug.technion.ac.il/rep/REPFILE.zip";
		private const string RESOURCES_GROUP = "CourseDB";
		private XmlDataDocument mXMLDataFile;
		private bool mbInitialized; // True if loaded with a database, false otherwise
		private bool mbSchemaExists; 

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public CourseDB()
		{
			mbInitialized = false;
			mbSchemaExists = false;
			mXMLDataFile=new XmlDataDocument();
			try
			{
				mXMLDataFile.DataSet.ReadXmlSchema("UDonkey.xsd");
				mbSchemaExists = true;
			}
			catch(System.IO.FileNotFoundException)
			{
				MessageBox.Show( null, Resources.String( RESOURCES_GROUP, "xsdFailedMessage1" ), Resources.String( RESOURCES_GROUP, "xsdFailedMessage2" ), MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
				return;
			}
		}
#endregion
		
		#region Public Methods to retrieve information
		/// <summary>
		/// Returns a fully formed course object with data matching the course number
		/// supplied
		/// </summary>
		/// <param name="courseNum">The Course Number to retrieve</param>
		/// <returns>Returns a fully formed course object with data matching the course number</returns>
		public Course GetCourseByNumber(string courseNum)
		{
			XmlNodeList nodeList;
			CultureInfo culture = new CultureInfo( "ru-RU" );
			XmlElement root = mXMLDataFile.DocumentElement;
			// XPATH QUERY:  /CourseDB/Faculty/Course[@number="courseNum"]
			nodeList = root.SelectNodes("/CourseDB/Faculty/Course[@number=\""+courseNum+"\"]");
			if (nodeList.Count==0)  // No course found by that number
			{
				return null;
			}
			XmlNode theNode = nodeList.Item(0); // Get the first item which should be the only item.

			Course theCourse = new Course(); // Create new empty course
			
			// Update Course Number
			theCourse.Number=courseNum;

			// Update Course Name
			if (theNode.Attributes["name"]!=null)
			{
				theCourse.Name=theNode.Attributes["name"].Value;
			}
			
			// Update Faculty Name
			if (theNode.ParentNode.Attributes["name"].Value!=null)
			{
				theCourse.Faculty=theNode.ParentNode.Attributes["name"].Value;
			}

			// Update Lecturer in Charge
			if (theNode.Attributes["lecturerInCharge"]!=null)
			{
				theCourse.Lecturer=theNode.Attributes["lecturerInCharge"].Value;
			}
            
			// Update Academic Points
			if (theNode.Attributes["courseAcademicPoints"]!=null)
			{
				theCourse.AcademicPoints=float.Parse(theNode.Attributes["courseAcademicPoints"].Value);
			}
			
			// Update Lecture Hours
			if (theNode.Attributes["lectureHours"]!=null)
			{
				theCourse.LectureHours=int.Parse(theNode.Attributes["lectureHours"].Value);
			}

			// Update Tutorial Hours
			if (theNode.Attributes["tutorialHours"]!=null)
			{
				theCourse.TutorialHours=int.Parse(theNode.Attributes["tutorialHours"].Value);
			}

			// Update Lab Hours
			if (theNode.Attributes["labHours"]!=null)
			{
				theCourse.LabHours=int.Parse(theNode.Attributes["labHours"].Value);
			}

			// Update Project Hours
			if (theNode.Attributes["projectHours"]!=null)
			{
				theCourse.ProjecHours=int.Parse(theNode.Attributes["projectHours"].Value);
			}

			// Update MoedA
			if (theNode.Attributes["moedADate"]!=null)
			{
				theCourse.MoadA=ConvertDateTime(theNode.Attributes["moedADate"].Value);
			}

			// Update MoedB
			if (theNode.Attributes["moedBDate"]!=null)
			{
				theCourse.MoadB=ConvertDateTime(theNode.Attributes["moedBDate"].Value);
			}
			
			AddEventListsToCourse(theNode, theCourse);

			return theCourse;

		}
		
		/// <summary>
		/// Gets a string collection containing all the names of the faculties
		/// in the database.  Used in comboboxes to select a faculty
		/// </summary>
		/// <returns>StringCollection with all faculty names</returns>
		public StringCollection GetFacultyList()
		{
			if( !isInitialized )
			{
				throw new Exception("CourseDB is not initilized");
			}
			StringCollection faculties = new StringCollection();
			XmlTextReader reader = new XmlTextReader(m_strFileName);
			while(reader.Read()) // Move the reader to the first Faculty
			{
				if (reader.Name.Equals("Faculty"))
					break;
			}
			while (!reader.EOF)
			{
				if (reader.Name.Equals("Faculty"))
				{
					faculties.Add(reader.GetAttribute("name"));
				}
				reader.Skip(); // Skip the child nodes and move to the next faculty node
			}
			return faculties;
		}

		/// <summary>
		/// Gets a string collection containing all the names of the courses
		/// in the database that belong to the specified Faculty.  
		/// Used in the listbox that allows the user to select a course after selecting
		/// a faculty
		/// </summary>
		/// <param name="facultyName">The faculty name whose courses should be returned</param>
		/// <returns>A StringCollection with the coursenames of the selected faculty</returns>
		public StringCollection GetCourseNamesByFacultyName(string facultyName)
		{
			StringCollection courseNames = new StringCollection();
			XmlNodeList nodeList;
			XmlElement root = mXMLDataFile.DocumentElement;
			// XPATH QUERY:  /CourseDB/Faculty[@name="facultyName"]
			nodeList = root.SelectNodes("/CourseDB/Faculty[@name=\""+facultyName+"\"]");
			if (nodeList.Count==0)  // No Faculty found by that number
			{
				return courseNames;
			}
			XmlNode theNode = nodeList.Item(0); // Get the first item which should be the only item.
			nodeList=theNode.ChildNodes;
			foreach (XmlNode node in nodeList)
			{
				courseNames.Add(node.Attributes["name"].Value);
			}
			return courseNames;
		}

		/// <summary>
		/// If the specified course name exists in the database, this will return a course
		/// number for it.  If several courses exist by that name, the first one will be returned.
		/// </summary>
		/// <param name="courseName">The name of the course to retrieve</param>
		/// <returns>A String with the course's number</returns>
		public String GetCourseNumberByCourseName(string courseName)
		{
			XmlNodeList nodeList;
			XmlElement root = mXMLDataFile.DocumentElement;
			// XPATH QUERY:  /CourseDB/Faculty/Course[@name="courseName"]
			nodeList = root.SelectNodes("/CourseDB/Faculty/Course[@name=\""+courseName+"\"]");
			if (nodeList.Count==0)  // No Course found by that name
			{
				return string.Empty;
			}
			XmlNode theNode = nodeList.Item(0); // Get the first item which should be the only item.
			return theNode.Attributes["number"].Value;
		}

		/// <summary>
		/// Retrieves a DataTable with the event occurences of a specific course event.
		/// This is currently not used, and was written for a datagrid implementation of the 
		/// event occurrences.
		/// </summary>
		/// <param name="courseNumber">The course's number</param>
		/// <param name="regNum">The event number of the event to retrieve</param>
		/// <returns>A DataTable with the CourseEventOccurences of the selected course
		/// and event</returns>
		public DataTable GetCourseEventDataTable(string courseNumber, string regNum)
		{
			XmlNode node = mXMLDataFile.SelectSingleNode("/CourseDB/Faculty/Course[@number=\""+courseNumber+"\"]/CourseEvent[@regNumber=\""+regNum+"\"]");
			DataTable dt = new DataTable();
			XmlNodeList nodeList = node.ChildNodes;
			foreach (XmlNode eventNode in nodeList)
			{
				XmlElement element = (XmlElement)eventNode;
				dt.Rows.Add(mXMLDataFile.GetRowFromElement(element));
			}
			return dt;
		}

		
		/// <summary>
		/// This is the advanced search method used for the advanced search control.  It recieves
		/// a list of optional parameters, and creates an XPath query using only those parameters
		/// that were actually specified.
		/// </summary>
		/// <param name="courseName">The Course Name.  May be a partial string</param>
		/// <param name="courseNum">The Course's Number.  May be a partial string</param>
		/// <param name="points">Academic Points</param>
		/// <param name="faculty">Faculty</param>
		/// <param name="lecturerName">Lecturer In Charge's Name.  May be a partial string</param>
		/// <param name="days">The days in which there are occurrences</param>
		/// <returns>a CourseID collection (CourseID is a struct of names and numbers) with the
		/// results of the search</returns>
		public CourseIDCollection SearchDBByParam(string courseName, string courseNum, string points, 
			string faculty, string lecturerName, string[] days/*TODO:, string startingTime*/ )
		{
			XmlNodeList nodeList = null;
			XmlElement root = mXMLDataFile.DocumentElement;
			bool flag = false; // This flag tells us if we left something open which we need to close later
			string temp = string.Empty;
			int i;
			
			// Sanitize the input
			courseName = SanitizeInput(courseName);
			courseNum = SanitizeInput(courseNum);
			points = SanitizeInput(points);
			faculty = SanitizeInput(faculty);
			lecturerName = SanitizeInput(lecturerName);

			// Initial XPath = /CourseDB/Faculty[@name="faculty"]/Course
			//				or /CourseDB/Faculty/Course
			string xpath = string.Format("/CourseDB/Faculty{0}/Course",(faculty != null)?"[@name=\"" +faculty+ "\"]":"");

			if (courseName != null) // If Course Name exists add: [contains(@name, "courseName")
			{
				flag = true; // We will add the closing "]" later if there are no more search parameters
				temp = "[contains(@name,\"" + courseName + "\")";
			}
			if (courseNum != null)
			{
				if (flag) // If we also had a courseName add:  and contains(@number, "courseNum")
					temp += " and contains (@number,\"" + courseNum + "\")";
				else // This is the first search param for course so we need to open the brackets
				{
					temp += "[contains(@number,\"" + courseNum + "\")"; // add: [contains(@number,"courseNum")
					flag = true; // We will add the closing "]" later if there are no more search parameters
				}
			}

			if (points != null) // Academic points is a param
			{
				if (flag) // If the bracket is already open
					temp += " and @courseAcademicPoints=\"" + points + "\""; // add:  and @courseAcademicPoints="points"
				else // This is the first course param, we need to open the brackets
				{
					temp += "[@courseAcademicPoints=\"" + points + "\""; // add: [@courseAcademicPoints="points"
					flag = true; //  We will add the closing "]" later if there are no more search parameters 
				}
			}

			if (lecturerName != null) // Lecturer in charge is a param
			{
				if (flag) //  If the bracket is already open
					temp += " and contains(@lecturerInCharge,\"" + lecturerName + "\")"; //add: and contains(@lecturerInCharge,"lecturerName")
				else // This is the first course param, we need to open the brackets
				{
					temp += "[contains(@lecturerInCharge,\"" + lecturerName + "\")"; // add: [contains(@lecturerInCharge,"lecturerName")
					flag = true; //  We will add the closing "]" later.
				}
			}
			if (flag) // If we left a bracket open
				temp += "]"; // Add the closing bracket
			xpath += temp; // Add the course params to the XPath query


			flag = false; // Reset the flag to be used for the CourseEventOccurence params
			temp = null;
			if (days[0] != null) 
			{
				flag = true; // We will need to add the suffix later
				temp = "/CourseEvent/PlaceTime[@EventDay=\"" + days[0] + "\""; // Add: /CourseEvent/PlaceTime[@EventDay="days[0]"
			}

			for (i=1;i<6;++i) // For each day
			{
				if (days[i] != null) // If that day is a param
				{
					if (flag) // If we already had some days before
						temp += " or @EventDay=\"" + days[i] + "\""; // add: or @EventDay="days[i]"
					else
					{
						flag = true; // We will need to add the suffix later
						temp += "/CourseEvent/PlaceTime[@EventDay=\"" + days[i] + "\""; // Add: /CourseEvent/PlaceTime[@EventDay="days[i]"
					}
				}
			}

			if (flag) // If we need to add the suffix to close the bracket and return to Course level
				temp += "]/../.."; // Add: ]/../..  

			xpath += temp; // Append the CourseEventOccurrence params to the XPath query
			nodeList = root.SelectNodes(xpath); // Run the XPath query

			// Add the results to a new CourseID collection
			CourseIDCollection col= new CourseIDCollection();
			foreach (XmlNode myNode in nodeList)
			{
				CourseID cid = new CourseID();
				cid.CourseName = myNode.Attributes["name"].Value;
				cid.CourseNumber = myNode.Attributes["number"].Value;
				col.Add(cid);
			}
			return col;
		}
		
		
		/// <summary>
		/// This is the method used to retrieve CourseIDs by Faculty to fill the Course Selection window
		/// when a specific faculty is chosen.
		/// </summary>
		/// <param name="facultyName">The Faculty whose courses will be retrieved</param>
		/// <returns>A CourseID Collection (CourseID = struct containing name and number)</returns>
		public CourseIDCollection GetCoursesByFacultyName(string facultyName)
		{
			CourseIDCollection courses = new CourseIDCollection();
			XmlNodeList nodeList;
			XmlElement root = mXMLDataFile.DocumentElement;
			// XPATH QUERY:  /CourseDB/Faculty[@name="facultyName"]
			nodeList = root.SelectNodes("/CourseDB/Faculty[@name=\""+facultyName+"\"]");
			if (nodeList.Count==0)  // No Faculty found by that number
			{
				return courses;
			}
			XmlNode theNode = nodeList.Item(0); // Get the first item which should be the only item.
			nodeList=theNode.ChildNodes;
			foreach (XmlNode node in nodeList)
			{
				CourseID temp=new CourseID();
				temp.CourseName=node.Attributes["name"].Value;
				temp.CourseNumber=node.Attributes["number"].Value;
				courses.Add(temp);
			}
			return courses;
		}


		public string GetSemesterInfo()
		{
			if( !isInitialized )
			{
				throw new Exception("CourseDB is not initilized");
			}
			XmlTextReader reader = new XmlTextReader(m_strFileName);
			while(reader.Read()) // Move the reader to the first Faculty
			{
				if (reader.Name.Equals("Faculty"))
					break;
			}
			return reader.GetAttribute("semester");
		}

		#endregion 

		#region Other Public Methods
		
		/// <summary>
		/// Loads the specified XML database into the system.
		/// </summary>
		/// <param name="fileName">The filename containing the XML Database</param>
		public void Load(string fileName)
		{
			if (fileName.Length==0)
				return;
			mXMLDataFile=new XmlDataDocument();
			mXMLDataFile.Load(fileName);
			m_strFileName = fileName;
			mbInitialized = true;
			return;
		}

		public void Unload(string fileName)
		{
			if (fileName.Length==0)
				return;
			mXMLDataFile=new XmlDataDocument();
		}
		
		/// <summary>
		/// Opens a local zip file.  Used to open the zip file containing the REP file.
		/// </summary>
		/// <param name="zipFileName">The filename of the zip file to open</param>
		/// <returns>true if successful, false if zip wasn't found or if filename was empty</returns>
		public bool OpenLocalZip(string zipFileName)
		{
			if (zipFileName.Length==0)
				return false;
			Stream strm;
			try
			{
				strm = File.OpenRead(zipFileName);
			}
			catch(System.IO.FileNotFoundException)
			{
				//local zip not found
				return false;
			}
			OpenZipStream(strm);
			return true;
		}

		/// <summary>
		/// Saves the XMLDataDocument back to the XML file.  Used only for the DBEditor currently
		/// </summary>
		public void SaveXMLData()
		{
			mXMLDataFile.Save(m_strFileName);
		}
		/// <summary>
		/// Connects to the UG servers, downloads the most recent REP file, opens up the zip file, and
		/// deletes the zip file.
		/// </summary>
		public void AutoUpdate()
		{
			System.Net.WebClient Client = new System.Net.WebClient ();
			Stream strm;
			strm = Client.OpenRead (m_zipFileURI);
			OpenZipStream(strm);
			return;
		}
		

		public static DateTime ConvertDateTime(string s)
		{
			if (s.Length==0)
				return DateTime.MinValue;
			try
			{
				CultureInfo culture = new CultureInfo( "ru-RU" );
				return Convert.ToDateTime(s,culture );
			}
			catch
			{
				return DateTime.MinValue;
			}
		}
		
		#endregion

		#region private methods
		/// <summary>
		/// Opens a Zip file from a stream.
		/// </summary>
		/// <param name="strm">The stream containing the zip file</param>
		private void OpenZipStream(Stream strm)
		{
			ZipInputStream s = new ZipInputStream(strm);
			ZipEntry theEntry; // A file inside the zip
			while ((theEntry = s.GetNextEntry()) != null) 
			{
			
				Console.WriteLine(theEntry.Name);
			
				string directoryName = Path.GetDirectoryName(theEntry.Name);
				string fileName      = Path.GetFileName(theEntry.Name);

				if (fileName != String.Empty) 
				{
					FileStream streamWriter = File.Create(theEntry.Name);
				
					int size = 2048;
					byte[] data = new byte[2048]; // Read buffer
					while (true) // Loop to read the entire file from the stream
					{
						size = s.Read(data, 0, data.Length);
						if (size > 0) 
						{
							streamWriter.Write(data, 0, size);
						} 
						else 
						{
							break;
						}
					}
				
					streamWriter.Close();
				}
			}
			s.Close(); // Close the Zip File
			File.Delete(m_zipFileName); // Delete the Zip File
			return;
		}

		
		/// <summary>
		/// Convert a day letter to a DayOfWeek
		/// </summary>
		/// <param name="theDay">A Hebrew letter representing a day</param>
		/// <returns>DayOfWeek representation of the day letter</returns>
		DayOfWeek ParseDay(string theDay)
		{
			switch(theDay)
			{
				case "א": {return DayOfWeek.ראשון;}
				case "ב": return DayOfWeek.שני;
				case "ג": return DayOfWeek.שלישי;
				case "ד": return DayOfWeek.רביעי;
				case "ה": return DayOfWeek.חמישי;
				case "ו": return DayOfWeek.שישי;
				default: throw new ArgumentOutOfRangeException("theDay");
			}
			
		}

		private int ConvertStartTime(string time)
		{			
			string[] times = time.Split(".".ToCharArray());
			System.Diagnostics.Trace.Assert(times.Length==2, "Assertion failed - time is not of XX.YY format");
			int hour = Int32.Parse(times[0]);
			if (times[1].StartsWith("0"))
			{
				return hour-1;
			}
			else
			{
				return hour;
			}

		}

		/// <summary>
		/// This creates and adds CourseEvents from the XML to the Course Object
		/// </summary>
		/// <param name="theNode">The XML node of the Course</param>
		/// <param name="theCourse">The course object to add the events to</param>
		private void AddEventListsToCourse(XmlNode theNode, Course theCourse)
		{

			int currentRegGroup=0;
			string currentEventType;
			string currentTeacher;

			XmlNodeList courseEvents = theNode.ChildNodes;			

			foreach (XmlNode aCourseEvent in courseEvents)
			{
				// Get event's reg group info from XML
				currentRegGroup=int.Parse(aCourseEvent.Attributes["regNumber"].Value);
				currentEventType=aCourseEvent.Attributes["eventType"].Value;
				currentTeacher=aCourseEvent.Attributes["teacher"].Value;
				CourseEvent tempCourseEvent = new CourseEvent(theCourse, currentEventType, currentTeacher, currentRegGroup);
				
				// Iterate over the PlaceTimes and enter them into the event
				XmlNodeList placeTimes = aCourseEvent.ChildNodes;
				foreach (XmlNode aPlaceTime in placeTimes)
				{
					// If placetime does exist
					if (aPlaceTime.Attributes["EventDay"].Value.Length>0)
					{
						DayOfWeek theDay = ParseDay(aPlaceTime.Attributes["EventDay"].Value);												
						int theHour=ConvertStartTime(aPlaceTime.Attributes["EventTime"].Value);
						int theDuration=int.Parse(aPlaceTime.Attributes["EventDuration"].Value);
						string theLocation=aPlaceTime.Attributes["EventLocation"].Value;

						CourseEventOccurrence ceo = new CourseEventOccurrence(tempCourseEvent, theDay, theHour, theDuration,theLocation);

						tempCourseEvent.Occurrences.Add(ceo);
					}
				}
				if (tempCourseEvent.Occurrences.Count>0) //Don't add Events with no occurrences
				{
					switch (currentEventType)
					{
						case "הרצאה": { theCourse.Lectures.Add(currentRegGroup, tempCourseEvent); break; }
						case "מעבדה": { theCourse.Labs.Add(currentRegGroup, tempCourseEvent); break; }
						case "קבוצה": { theCourse.Projects.Add(currentRegGroup, tempCourseEvent); break; }
						default: { theCourse.Tutorials.Add(currentRegGroup, tempCourseEvent); break; }
					}
				}
			}
		}

		
		/// <summary>
		/// This removes illegal characters from user input in the search strings
		/// </summary>
		/// <param name="param">The string to sanitize</param>
		/// <returns>the santized string</returns>
		private string SanitizeInput(string param)
		{
			if (param==null)
				return null;
			string sanitized = param.Trim(); // Remove unncessary spaces
			sanitized = sanitized.Replace("\"",""); // Removes " signs
			return sanitized;
		}

		#endregion

		#region Properties
		public bool isInitialized
		{
			get {return mbInitialized;}
		}
		public bool isSchemaExists
		{
			get {return mbSchemaExists;}
		}
		public DataSet DataSet
		{
			get {return mXMLDataFile.DataSet;}
		}
		#endregion
	}
}
