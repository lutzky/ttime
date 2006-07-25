using System;
using System.Xml;
using System.IO;
using UDonkey.GUI;
using UDonkey.Logic;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.ComponentModel;

namespace UDonkey.IO
{
	/// <summary>
	/// This class provides import and export services to the entire system.  It is responsible of
	/// exporting schedules to HTML and XML, exporting and importing configuration from XML, and saving
	/// and loading the entire system state from and to XML.
	/// </summary>
	public class IOManager
	{
		#region Schedule
		/// <summary>
		/// A struct used in exporting a schedule to html, which contains the cell information 
		/// for each cell in the output table.
		/// </summary>
		public struct  Cell 
		{
			public string BGColor;
			public string FGColor;
			public string sevent;
			public int span;

			public Cell(string BG, string FG, string eve, int tspan) 
			{
				this.BGColor = BG;
				this.FGColor = FG;
				this.sevent = eve;
				this.span = tspan;
			}				
		}

		/// <summary>
		/// This goes over a datatable, and fixes it so that courses which span over more than one
		/// hour contain the correct spanning information in their first cell, and the rest of the cells are 
		/// deleted
		/// </summary>
		/// <param name="oldDataTable">The DataTable to fix</param>
		/// <returns>A Fixed DataTable</returns>
		private static DataTable spanTable( DataTable oldDataTable) 
		{

			DataTable newDataTable = new DataTable();
			int i=0, j=0;
			

			// creating columns for newDataTable
			foreach (DataColumn myCol in oldDataTable.Columns)
			{
				newDataTable.Columns.Add(myCol.ColumnName, typeof(Cell) );
			}

			DataColumnCollection myColumn = newDataTable.Columns;

			// Make a duplicate table with the info we need
			for (i=0;i<oldDataTable.Rows.Count;i++)
			{
				DataRow orig = oldDataTable.Rows[i];
				DataRow newRow = newDataTable.NewRow();

				for (j=0;j<oldDataTable.Columns.Count;j++)
				{
					IScheduleEntry entry = (IScheduleEntry)orig[ (oldDataTable.Columns[j]).ColumnName];
					string val = entry.ToString( (VerbosityFlag)ConfigurationController.GetExportVerbosityFlag() );
					string fColor = entry.ForeColor.ToKnownColor().ToString();
					string bColor = entry.BackColor.ToKnownColor().ToString();
					val = val.Replace( "\n", "<br>");
					Cell newCell = new Cell(bColor, fColor, val, 1);
					newRow[j]=newCell;
				}
				newDataTable.Rows.Add(newRow);
			}

			// Make a pass on the new table replacing the span values
			i=0;
			foreach (DataRow myRow in newDataTable.Rows) // For each row
			{

				foreach (DataColumn myCol in newDataTable.Columns) // For each column
				{
					if ( myCol.ColumnName == day .ToString() ) // Skip the "hours" column
					{
						continue;
					}
					if( (((Cell)(myRow[myCol])).sevent).Length!=0 ) // If this cell contains an event
					{
						for (j=(i+1) ; j<newDataTable.Rows.Count ; ++j) // Check if the next hours belong to the same event
						{
							if  ((((Cell)((newDataTable.Rows[j])[myCol.ColumnName])).BGColor) == 
								((Cell)((newDataTable.Rows[i])[myCol.ColumnName])).BGColor  && 
								(((Cell)((newDataTable.Rows[j])[myCol.ColumnName])).sevent) == 
								((Cell)((newDataTable.Rows[i])[myCol.ColumnName])).sevent )
							{
								// Update the spanning of the original cell
								Cell cell1 = (Cell)newDataTable.Rows[i][myCol]; 
								(cell1.span)++;
								(newDataTable.Rows[i])[myCol]=cell1;

								// Insert "skip" into the current cell in order to skip it when creating the html
								Cell cell2 = (Cell)newDataTable.Rows[j][myCol];
								cell2.sevent="skip";
								(newDataTable.Rows[j])[myCol]=cell2;
							}
							else break; // When we reach a cell which isn't part of the course, end the loop.
						}
					}
				}
				i++;
			}

			return newDataTable;

		}

		/// <summary>
		/// Exports a Schedule to HTML
		/// </summary>
		/// <param name="filename">The desired output filename</param>
		/// <param name="aSchedule">The Schedule to export</param>
		public static void ExportSchedTableHtml (HtmlTextWriter scheduleWriter, Schedule aSchedule )
		{			
			DataTable aDataTable = spanTable(aSchedule.DataTable);
			
			int i=0;
			
			bool[] visibleDays= new bool[] {true,
											   Configuration.Get("Display","Sunday",    true ),
											   Configuration.Get("Display","Monday",    true ),
											   Configuration.Get("Display","Tuesday",   true ),
											   Configuration.Get("Display","Wednesday", true ),
											   Configuration.Get("Display","Thursday",  true ),
											   Configuration.Get("Display","Friday",    false ),
											   Configuration.Get("Display","Saturday",  false ) };		

			scheduleWriter.WriteBeginTag("Table");
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Width.ToString(), "100%");
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Border.ToString(), "5" );
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Cellpadding.ToString(), "1");
			scheduleWriter.WriteAttribute( "Dir", "rtl" );
			scheduleWriter.Write(">\r\n");

			scheduleWriter.WriteFullBeginTag("TR");
			// First Header line only
			foreach (DataColumn myCol in aDataTable.Columns)
			{
				try
				{
					if (visibleDays[i]==true)
					{
						scheduleWriter.WriteBeginTag("TH");
						if (myCol.ColumnName == "")
						{
							scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Height.ToString(), "60");
						}
						scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Height.ToString(), "35");
						
						scheduleWriter.Write(">");
						scheduleWriter.Write(myCol.ColumnName);
						scheduleWriter.WriteEndTag("TH");
						scheduleWriter.Write("\r\n");
					}
					++i;

				}
				catch(Exception e)
				{
					Console.WriteLine("Exception: {0}", e.ToString());
				}
			}

			scheduleWriter.WriteEndTag("TR");
			scheduleWriter.Write("\r\n");


			// Write Table Data
				
			foreach (DataRow myRow in aDataTable.Rows)
			{
				i=0;
				try
				{
					scheduleWriter.WriteBeginTag("TR");
					scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Bordercolor.ToString(), "#000000");
					scheduleWriter.Write(">\r\n");
				}
				catch(Exception e)
				{
					Console.WriteLine("Exception: {0}", e.ToString());
				}


				foreach (DataColumn myCol in aDataTable.Columns)
				{
					try
					{
						if (visibleDays[i]==true)
						{
							string val = ((Cell)(myRow[myCol])).sevent;
							val = val.Replace( "\r\n", "<br>");
							if (val!="skip")
							{
								if (myCol.ColumnName=="") // The Hours column gets special treatment
								{
									scheduleWriter.WriteBeginTag("TH");
									scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Width.ToString(), "60");
									scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Height.ToString(), "40");
									scheduleWriter.WriteBeginTag("div");
									scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Align.ToString(), "center");
									scheduleWriter.Write(">\r\n");
		
									scheduleWriter.Write(val);
									scheduleWriter.WriteEndTag("div");
									scheduleWriter.WriteEndTag("TH");
									scheduleWriter.Write("\r\n");
								}
								else // The current column is not the hours column
								{
									scheduleWriter.WriteBeginTag("TD");
									scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Width.ToString(), "150");
									scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Height.ToString(), "40");
									if (val.Length>0) // If there is something to write to this cell
									{
										scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Rowspan.ToString(), ((Cell)(myRow[myCol])).span.ToString() );
										scheduleWriter.WriteAttribute ( "BGColor" , ((Cell)(myRow[myCol])).BGColor );
										scheduleWriter.WriteAttribute(HtmlTextWriterAttribute.Align.ToString(), "right" );
										scheduleWriter.Write(">");

										scheduleWriter.Write( "<FONT COLOR="+((Cell)(myRow[myCol])).FGColor+">" );	
										
										scheduleWriter.Write(val);
									}
									else // There is nothing to write to the current cell
									{
										scheduleWriter.Write(">");
										// Enter a "non breaking space" to avoid "collapse" of the empty cell
										scheduleWriter.Write("&nbsp"); 
									
									}
									scheduleWriter.WriteEndTag("TD");
									scheduleWriter.Write("\r\n");
								}
							}
							
						}
						i++;
					}
					catch(Exception e)
					{
						Console.WriteLine("Exception: {0}", e.ToString());
					}
				}
			}
			scheduleWriter.WriteEndTag("Table");
		}		

		/// <summary>
		/// Exports a given schedule to an HTML file
		/// </summary>
		/// <param name="filename">The Filename to write</param>
		/// <param name="aSchedule">The source schedule</param>
		public static void ExportSchedToHtml ( string filename, Schedule aSchedule )
		{
			if (filename.Length == 0)
				return;
			StreamWriter w =  File.CreateText( filename );
			HtmlTextWriter scheduleWriter = new HtmlTextWriter( w );

			scheduleWriter.WriteFullBeginTag("html");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteFullBeginTag("head");
			scheduleWriter.Write("\r\n");
			scheduleWriter.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
			scheduleWriter.Write("\r\n");
			scheduleWriter.Write("<style type=\"text/css\">\r\n");
			scheduleWriter.Write("<!--\r\nth {color: #660066 ; background-color: #99CCFF ; font-size: 22px}\r\n-->\r\n");
			scheduleWriter.Write("</style>\r\n");
			scheduleWriter.WriteEndTag("head");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteBeginTag("body");
			scheduleWriter.Write(" bgcolor=\"white\"");
			scheduleWriter.Write(">\r\n");
			scheduleWriter.WriteBeginTag("h1");		
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Align.ToString(), "center" );
			scheduleWriter.WriteFullBeginTag("strong");
			scheduleWriter.Write(" ");
			scheduleWriter.WriteEndTag("strong");
			scheduleWriter.WriteEndTag("h1");
			scheduleWriter.Write("\r\n");

			ExportSchedTableHtml ( scheduleWriter,  aSchedule);

			scheduleWriter.WriteEndTag("body");
			scheduleWriter.WriteEndTag("html");
			scheduleWriter.Flush();
			scheduleWriter.Close();

		}

		#endregion

		#region Configuration
		/// <summary>
		/// Creates the skeleton empty XML file of the configuration with all the paths.  This was required
		/// since we don't build the config XML serially.
		/// </summary>
		/// <param name="d">The XML DataDocument to add the information to</param>
		private static void CreateConfigSkeletonXml(XmlDataDocument d)
		{
			try
			{
				XmlElement rootElement = d.CreateElement("Config");
				XmlElement node1;
				XmlElement node2;
				node1 = d.CreateElement("General");
				rootElement.AppendChild(node1);
				node1 = d.CreateElement("Display");
				rootElement.AppendChild(node1);
				node1 = d.CreateElement("Pref");
				rootElement.AppendChild(node1);
				node1 = d.CreateElement("Constraints");
				rootElement.AppendChild(node1);
				node1 = d.CreateElement("Export");
				rootElement.AppendChild(node1);
				node1 = d.CreateElement("Schedule");
				node2 = d.CreateElement("Sunday");
				node1.AppendChild(node2);
				node2 = d.CreateElement("Monday");
				node1.AppendChild(node2);
				node2 = d.CreateElement("Tuesday");
				node1.AppendChild(node2);
				node2 = d.CreateElement("Wednesday");
				node1.AppendChild(node2);
				node2 = d.CreateElement("Thursday");
				node1.AppendChild(node2);
				rootElement.AppendChild(node1);

				d.AppendChild(rootElement);
			}
			catch(Exception e2)
			{
				Console.WriteLine("Exception: {0}", e2.ToString());
			}
		}

		/// <summary>
		/// Adds the configuration data to an existing XML file
		/// </summary>
		/// <param name="writer">The XmlWriter to add the data into</param>
		public static void ExportConfigBodyToXml(XmlWriter writer)
		{
			try
			{
				XmlDataDocument d = new XmlDataDocument();
				CreateConfigSkeletonXml(d); // Build all the empty nodes before we begin adding information
				foreach( string key in new ArrayList( Configuration.ConfigDictionary.Keys ) )
				{
					int i=0;
					string s = (string)Configuration.ConfigDictionary[key];
					string fullpath = string.Empty;
					string[] temppath = key.Split('\\');
					if (temppath.Length>1)
					{
						fullpath = "/Config";
						for (i=0 ; i<(temppath.Length-1) ; i++)
						{
							fullpath += "/" + temppath[i];
						}
					}
					try
					{
						XmlElement newNode = (XmlElement)d.SelectSingleNode(fullpath);
						newNode.SetAttribute(temppath[i], s);
					}
					catch (XmlException e)
					{
						System.Windows.Forms.MessageBox.Show(e.Message, "XPath Error: " + fullpath);
					}
		
				}
				d.WriteContentTo(writer);
			}
			catch(Exception e2)
			{
				Console.WriteLine("Exception: {0}", e2.ToString());
			}
		}

		/// <summary>
		/// Imports configuration data from an XML node into the system
		/// </summary>
		/// <param name="rootNode">The node containing the config data.</param>
		public static void ImportConfigFromXml(XmlNode rootNode)
		{
			if (rootNode.Name=="Config")
			{
				FindAttributesRecursivly( rootNode,"" );
			}
			return;
		}
		
		/// <summary>
		/// recursive function that goes into the attributes and updates the configuration accordingly
		/// </summary>
		/// <param name="node">The current node</param>
		/// <param name="path">The path so far</param>
		private static void FindAttributesRecursivly( XmlNode node, string path)
		{
			foreach( XmlNode n in node )
			{
				string newPath = (path.Length == 0)? n.Name : path + @"\" + n.Name;
				FindAttributesRecursivly( n, newPath );
			}
			foreach( XmlAttribute att in node.Attributes )
			{
				Configuration.ConfigDictionary[ path + @"\" + att.Name ] = att.Value;						
			}
		}

		#endregion

		#region Course List
		/// <summary>
		/// Exports a CourseList to a new XML file.
		/// </summary>
		/// <param name="fileName">The filename to write the XML to</param>
		/// <param name="courses">The list of courses to export</param>
		public static void ExportCourseListToXml ( string fileName, CoursesList courses )
		{
			if (fileName.Length==0)
				return;
			XmlTextWriter writer = new XmlTextWriter (fileName, null);

			writer.Formatting = Formatting.Indented;
			writer.Indentation= 6;
			writer.Namespaces = false;
			
			writer.WriteStartDocument();

			ExportCourseListToXml_body(writer, courses);

			writer.WriteEndDocument();
			writer.Flush();
			if (writer != null)
			{
				writer.Close();
			}

		}

		/// <summary>
		/// Exports a CourseList to an existing XML
		/// </summary>
		/// <param name="filename">The desired output filename</param>
		/// <param name="courses">The CourseList to export</param>
		public static void ExportCourseListToXml_body (XmlTextWriter writer, CoursesList courses)
		{
			
			try
			{
				writer.WriteStartElement("", "CourseList", "");
				foreach (Course aCourse in courses.Values)
				{
					writer.WriteStartElement("", "Course", "");
					writer.WriteAttributeString("name", aCourse.Name);
					writer.WriteAttributeString("number", aCourse.Number);
					writer.WriteAttributeString("faculty", aCourse.Faculty);
					writer.WriteAttributeString("courseAcademicPoints", aCourse.AcademicPoints.ToString());
					writer.WriteAttributeString("lectureHours", aCourse.LectureHours.ToString());
					writer.WriteAttributeString("tutorialHours", aCourse.TutorialHours.ToString());
					writer.WriteAttributeString("labHours", aCourse.LabHours.ToString());
					writer.WriteAttributeString("projectHours", aCourse.ProjecHours.ToString());
					writer.WriteAttributeString("lecturerInCharge", aCourse.Lecturer);
					writer.WriteAttributeString("moedADate", aCourse.MoadA.ToShortDateString());
					writer.WriteAttributeString("moedBDate", aCourse.MoadB.ToShortDateString());
					if (aCourse.Lectures.Count>0)
					{
						writer.WriteStartElement("", "Lectures", "");
						foreach (CourseEvent ce in aCourse.Lectures.Values)
						{
							writer.WriteStartElement("", "Lecture", "");
							writer.WriteAttributeString("regNumber", ce.EventNum.ToString());
							writer.WriteAttributeString("teacher", ce.Giver);
							foreach (CourseEventOccurrence ceo in ce.Occurrences)
							{
								writer.WriteStartElement("", "PlaceTime", "");

								writer.WriteAttributeString("EventDay", ceo.Day.ToString());
								writer.WriteAttributeString("EventTime", ceo.Hour.ToString());
								writer.WriteAttributeString("EventDuration", ceo.Duration.ToString());
								writer.WriteAttributeString("EventLocation", ceo.Location);

								writer.WriteEndElement(); // End Placetime
							}
							writer.WriteEndElement(); // End Lecture
						}
						writer.WriteEndElement(); // End Lectures
					}

					if (aCourse.Tutorials.Count>0)
					{
						writer.WriteStartElement("", "Tutorials", "");
						foreach (CourseEvent ce in aCourse.Tutorials.Values)
						{
							writer.WriteStartElement("", "Tutorial", "");
							writer.WriteAttributeString("regNumber", ce.EventNum.ToString());
							writer.WriteAttributeString("teacher", ce.Giver);
							foreach (CourseEventOccurrence ceo in ce.Occurrences)
							{
								writer.WriteStartElement("", "PlaceTime", "");

								writer.WriteAttributeString("EventDay", ceo.Day.ToString());
								writer.WriteAttributeString("EventTime", ceo.Hour.ToString());
								writer.WriteAttributeString("EventDuration", ceo.Duration.ToString());
								writer.WriteAttributeString("EventLocation", ceo.Location);

								writer.WriteEndElement(); // End Placetime
							}
							writer.WriteEndElement(); // End Tutorial
						}
						writer.WriteEndElement(); // End Tutorials
					}

					if (aCourse.Labs.Count>0)
					{
						writer.WriteStartElement("", "Labs", "");
						foreach (CourseEvent ce in aCourse.Labs.Values)
						{
							writer.WriteStartElement("", "Lab", "");
							writer.WriteAttributeString("regNumber", ce.EventNum.ToString());
							writer.WriteAttributeString("teacher", ce.Giver);
							foreach (CourseEventOccurrence ceo in ce.Occurrences)
							{
								writer.WriteStartElement("", "PlaceTime", "");

								writer.WriteAttributeString("EventDay", ceo.Day.ToString());
								writer.WriteAttributeString("EventTime", ceo.Hour.ToString());
								writer.WriteAttributeString("EventDuration", ceo.Duration.ToString());
								writer.WriteAttributeString("EventLocation", ceo.Location);

								writer.WriteEndElement(); // End Placetime
							}
							writer.WriteEndElement(); // End Lab
						}
						writer.WriteEndElement(); // End Labs
					}

					if (aCourse.Projects.Count>0)
					{
						writer.WriteStartElement("", "Projects", "");
						foreach (CourseEvent ce in aCourse.Projects.Values)
						{
							writer.WriteStartElement("", "Project", "");
							writer.WriteAttributeString("regNumber", ce.EventNum.ToString());
							writer.WriteAttributeString("teacher", ce.Giver);
							foreach (CourseEventOccurrence ceo in ce.Occurrences)
							{
								writer.WriteStartElement("", "PlaceTime", "");

								writer.WriteAttributeString("EventDay", ceo.Day.ToString());
								writer.WriteAttributeString("EventTime", ceo.Hour.ToString());
								writer.WriteAttributeString("EventDuration", ceo.Duration.ToString());
								writer.WriteAttributeString("EventLocation", ceo.Location);

								writer.WriteEndElement(); // End Placetime
							}
							writer.WriteEndElement(); // End Project
						}
						writer.WriteEndElement(); // End Projects
					}
					writer.WriteEndElement(); // End Course
				}
				writer.WriteEndElement(); // End CourseList
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
			
		}

		/// <summary>
		/// Exports a courselist to an HTML file.
		/// </summary>
		/// <param name="filename">the desired html filename</param>
		/// <param name="courses">the courselist to export</param>
		public static void ExportCourseListToHtml ( string filename, CoursesList courses )
		{
			if (filename.Length == 0)
				return;
			StreamWriter w =  File.CreateText( filename );
			string toWrite;
			HtmlTextWriter scheduleWriter = new HtmlTextWriter( w );

			scheduleWriter.WriteFullBeginTag("html");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteFullBeginTag("head");
			scheduleWriter.Write("\r\n");
			scheduleWriter.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
			scheduleWriter.Write("\r\n");
			scheduleWriter.Write("<style type=\"text/css\">\r\n");
			scheduleWriter.Write("<!--\r\nth {color: #660066 ; background-color: #99CCFF ; font-size: 22px}\r\n-->\r\n");
			scheduleWriter.Write("</style>\r\n");
			scheduleWriter.WriteEndTag("head");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteBeginTag("body");
			scheduleWriter.Write(" bgcolor=\"white\"");
			scheduleWriter.Write(">\r\n");
			scheduleWriter.WriteBeginTag("h1");		
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Align.ToString(), "center" );
			scheduleWriter.WriteFullBeginTag("strong");
			scheduleWriter.Write(" ");
			scheduleWriter.WriteEndTag("strong");
			scheduleWriter.WriteEndTag("h1");
			scheduleWriter.Write("\r\n");

			scheduleWriter.WriteBeginTag("Table");
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Width.ToString(), "100%");
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Border.ToString(), "5" );
			scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Cellpadding.ToString(), "1");
			scheduleWriter.WriteAttribute( "Dir", "rtl" );
			scheduleWriter.Write(">\r\n");

			scheduleWriter.WriteFullBeginTag("TR");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteFullBeginTag("TH");
			scheduleWriter.Write("");
			scheduleWriter.WriteEndTag("TH");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteFullBeginTag("TH");
			scheduleWriter.Write("");
			scheduleWriter.WriteEndTag("TH");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteFullBeginTag("TH");
			scheduleWriter.Write("");
			scheduleWriter.WriteEndTag("TH");
			scheduleWriter.Write("\r\n");
			scheduleWriter.WriteEndTag("TR");


			foreach (Course aCourse in courses.Values)
			{
				string testA = string.Empty;
				string testB = String.Empty;
				if (aCourse.MoadA != DateTime.MinValue)
					testA = aCourse.MoadA.ToShortDateString();
				else
					testA = " ";
				if (aCourse.MoadB != DateTime.MinValue)
					testB = aCourse.MoadB.ToShortDateString();
				else
					testB = " ";
				
				scheduleWriter.WriteBeginTag("TR");
				scheduleWriter.WriteAttribute( HtmlTextWriterAttribute.Bordercolor.ToString(), "#000000");
				scheduleWriter.Write(">\r\n");
				scheduleWriter.WriteFullBeginTag("TD");
				toWrite = aCourse.Number + " " + aCourse.NickName;
				scheduleWriter.Write(toWrite);
				scheduleWriter.WriteEndTag("TD");
				scheduleWriter.WriteFullBeginTag("TD");
				toWrite = " : " +
					aCourse.AcademicPoints.ToString() +
					"<br> : " + testA + 
					"<br> : " + testB + 
					"<br>"  + " : " + aCourse.Lecturer;
				scheduleWriter.Write(toWrite);
				scheduleWriter.WriteEndTag("TD");
				scheduleWriter.WriteFullBeginTag("TD");
				toWrite = string.Empty;
				foreach (CourseEvent ce in aCourse.GetAllCourseEvents())
				{
					foreach (CourseEventOccurrence ceo in ce.Occurrences)
					{
						int toHour = (ceo.Hour+ceo.Duration);
						string toHourString = toHour.ToString() + ":30";
						toWrite= ce.Type + " " + ce.EventNum.ToString() + ":  " + ceo.Day + " " + ceo.Hour + ":30-" + toHourString + " " + ceo.Location + "<br>";
						scheduleWriter.Write(toWrite);
					}
				}
				scheduleWriter.WriteEndTag("TD");
				scheduleWriter.WriteEndTag("TR");

			}
			scheduleWriter.WriteEndTag("Table");
			scheduleWriter.WriteEndTag("body");
			scheduleWriter.WriteEndTag("html");
			scheduleWriter.Flush();
			scheduleWriter.Close();
		}

		/// <summary>
		/// Imports a courselist from an XML node.
		/// </summary>
		/// <param name="sourceNode">The XMLnode with the course information</param>
		/// <returns>A CourseList containing the course object representation of the courses in the XML</returns>
		public static CoursesList ImportCourseListFromXmlNode(XmlNode sourceNode)
		{
			CoursesList courseList = new CoursesList();
			XmlNodeList nodeList = sourceNode.ChildNodes;
			foreach (XmlNode theNode in nodeList)
			{
				Course theCourse = new Course(); // Create new empty course
			
				// Update Course Number
				theCourse.Number=theCourse.Name=theNode.Attributes["number"].Value;

				// Update Course Name
				if (theNode.Attributes["name"]!=null)
				{
					theCourse.Name=theNode.Attributes["name"].Value;
				}
			
				// Update Faculty Name
				if (theNode.Attributes["faculty"].Value!=null)
				{
					theCourse.Faculty=theNode.Attributes["faculty"].Value;
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
					theCourse.MoadA=UDonkey.DB.CourseDB.ConvertDateTime(theNode.Attributes["moedADate"].Value);
				}

				// Update MoedB
				if (theNode.Attributes["moedBDate"]!=null)
				{
					theCourse.MoadB=UDonkey.DB.CourseDB.ConvertDateTime(theNode.Attributes["moedBDate"].Value);
				}

				AddEventsToCourse(theNode, theCourse);
				courseList.Add(theCourse.Number, theCourse);
				
			}
			return courseList;
		}

		public static CoursesList ImportCourseListFromXml (string filename)
		{
			if (filename.Length==0)
				return new CoursesList();
			Stream strm;
			XmlNode node;
			try
			{
				strm = File.OpenRead(filename);
			}
			catch(System.IO.FileNotFoundException)
			{
				System.Windows.Forms.MessageBox.Show(filename + " does not exist");
				return null;
			}
			XmlDataDocument dataFile = new XmlDataDocument();
			dataFile.Load(strm);
			XmlElement root = dataFile.DocumentElement;
			node = root.SelectSingleNode("/CourseList");
			if (node==null)  // No course found by that number
			{
				return new CoursesList();
			}

			return ImportCourseListFromXmlNode(node);
		}

		private static void AddEventsToCourse(XmlNode theNode, Course theCourse)
		{
			int currentRegGroup=0;
			string currentEventType;
			string currentTeacher;
			XmlNodeList eventLists = theNode.ChildNodes;
			XmlNodeList singleEventList;

			foreach (XmlNode listNode in eventLists)
			{
				singleEventList = listNode.ChildNodes;



				foreach (XmlNode aCourseEvent in singleEventList)
				{
					if (listNode.Name == "Lectures") currentEventType="";
					else if (listNode.Name == "Tutorials") currentEventType="";
					else if (listNode.Name == "Labs") currentEventType="";
					else if (listNode.Name == "Projects") currentEventType="";
					else throw new ApplicationException("This should never happened");

					// Get event's reg group info from XML
					currentRegGroup=int.Parse(aCourseEvent.Attributes["regNumber"].Value);
				
					currentTeacher=aCourseEvent.Attributes["teacher"].Value;
					CourseEvent tempCourseEvent = new CourseEvent(theCourse, currentEventType, currentTeacher, currentRegGroup);
				
					// Iterate over the PlaceTimes and enter them into the event
					XmlNodeList placeTimes = aCourseEvent.ChildNodes;
					foreach (XmlNode aPlaceTime in placeTimes)
					{
						// If placetime does exist
						if (aPlaceTime.Attributes["EventDay"].Value.Length>0)
						{
							DayOfWeek theDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), aPlaceTime.Attributes["EventDay"].Value);
							int theHour=Convert.ToInt32(float.Parse(aPlaceTime.Attributes["EventTime"].Value));
							int theDuration=int.Parse(aPlaceTime.Attributes["EventDuration"].Value);
							string theLocation=aPlaceTime.Attributes["EventLocation"].Value;

							CourseEventOccurrence ceo = new CourseEventOccurrence(tempCourseEvent, theDay, theHour, theDuration,theLocation);

							tempCourseEvent.Occurrences.Add(ceo);
						}
					}

					if (listNode.Name == "Lectures") theCourse.Lectures.Add(currentRegGroup, tempCourseEvent);
					else if (listNode.Name == "Tutorials") theCourse.Tutorials.Add(currentRegGroup, tempCourseEvent);
					else if (listNode.Name == "Labs") theCourse.Labs.Add(currentRegGroup, tempCourseEvent);
					else if (listNode.Name == "Projects") theCourse.Projects.Add(currentRegGroup, tempCourseEvent);
				}
			}
		}
			
		#endregion
		
		#region System State
		public static void ExportSystemState (string filename, CoursesScheduler scheduler)
		{
			if (filename.Length == 0)
				return;
			XmlTextWriter writer;
			try
			{
				writer = new XmlTextWriter (filename, null);
			}
			catch
			{
				//TODO: Better handling
				return;
			}
			writer.WriteStartDocument();
			writer.WriteStartElement("SystemState");
			ExportConfigBodyToXml(writer);
			ExportCourseListToXml_body(writer, scheduler.Courses);
			ExportEventstoXml(writer, scheduler.UserEvents);
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
			
		}

		public static void ImportSystemState (string filename, UDonkeyClass theUDonkeyClass)
		{
			if (filename.Length==0)
				return;
			Stream strm;
			XmlElement node;
			try
			{
				strm = File.OpenRead(filename);
			}
			catch(System.IO.FileNotFoundException)
			{
				System.Windows.Forms.MessageBox.Show(filename + " does not exist");
				return;
			}
			XmlDataDocument dataFile = new XmlDataDocument();
			dataFile.Load(strm);
			XmlElement root = dataFile.DocumentElement;
			theUDonkeyClass.Reset();
			try
			{
				node = root["Config"];
				ImportConfigFromXml(node);

				node = root["CourseList"];
				CoursesList courses = ImportCourseListFromXmlNode(node);
			
				node = root["EVENT_LIST"];
				ICollection events = ImportEventsFromXml(node);

				CoursesScheduler scheduler = theUDonkeyClass.Scheduler;
				foreach (Course c in courses.Values)
				{
					scheduler.AddCourse(c);
				}
				foreach (UsersEventScheduleObject u in events)
				{
					scheduler.AddUserEvent(u.Event, u.DayOfWeek, u.StartHour, u.Duration);
				}
			}
			catch  // No course found by that number
			{
				System.Windows.Forms.MessageBox.Show("Error with Xpath");
			}
			

		}
		/// <summary>
		/// Exports User Events to XML.  This is used in the full system export
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="collection">The Collection of User Events to export</param>
		private static void ExportEventstoXml (XmlWriter writer, ICollection collection)
		{
			writer.WriteStartElement("EVENT_LIST");

			foreach (UsersEventScheduleObject sevent in collection)
			{
											
				writer.WriteStartElement("EVENT");
				writer.WriteAttributeString("NAME", sevent.Event );
				writer.WriteAttributeString("STARTHOUR", sevent.StartHour.ToString() );
				writer.WriteAttributeString("DURATION", sevent.Duration.ToString() );
				writer.WriteAttributeString("DAY",sevent.DayOfWeek.ToString() );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static ICollection ImportEventsFromXml (XmlTextReader reader)
		{
			ArrayList col = new ArrayList();

			//reads the root
			reader.ReadElementString();
			
			while (reader != null)
			{
				string s = reader.ReadAttributeValue().ToString();
				int sstarthour = int.Parse(reader.ReadElementString());
				int sduration = int.Parse(reader.ReadElementString());
				DayOfWeek sday = (DayOfWeek)Enum.Parse(typeof(DayOfWeek),reader.ReadElementString(),true);

				UsersEventScheduleObject userEvent 
					= new UsersEventScheduleObject(s, sday, sstarthour, sduration);
				
				col.Add(userEvent);
			}

			//reads the end-root element.
			reader.ReadElementString();

			return col;

		}
		
		private static ICollection ImportEventsFromXml (XmlNode eventList)
		{
			XmlNodeList eventlist = eventList.ChildNodes;

			ArrayList col = new ArrayList();

			foreach (XmlNode node in eventlist)
			{
				string tevent = node.Attributes["NAME"].Value;
				string tstarthour = node.Attributes["STARTHOUR"].Value;
				string tduration = node.Attributes["DURATION"].Value;
				DayOfWeek tday = (DayOfWeek)Enum.Parse(typeof(DayOfWeek),node.Attributes["DAY"].Value);


				UsersEventScheduleObject userEvent 
					= new UsersEventScheduleObject(tevent, tday, int.Parse(tstarthour), int.Parse(tduration));

				col.Add(userEvent);

			}
					
			return col;

		}

		#endregion
	}
}		
