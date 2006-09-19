using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for Course.
	/// </summary>
	public class Course: IComparable
	{
		#region Private Variables
		private string   mName;
		private string	 mNickName;
		private string   mNumber;
		private string	 mLecturer;
		private string	 mFaculty;
		private float    mAcademicPoints;
		private int      mLectureHours;
		private int      mTutorialHours;
		private int      mLabHours;
		private int      mProjectHours;
		private DateTime mMoadA; 
		private DateTime mMoadB;
		private RegistrationGroupsCollection mcRegistrationGroups;
		private CourseEventsList mcLectures;
		private CourseEventsList mcTutorials;
		private CourseEventsList mcLabs;
		private CourseEventsList mcProjects;

		#endregion Private Variables
		public Course()
		{
			mName                = string.Empty;
			mNickName			 = string.Empty;
			mNumber              = string.Empty;
			mLecturer            = string.Empty;
			mFaculty			 = string.Empty;
			mcLectures			 = new CourseEventsList();
			mcTutorials 		 = new CourseEventsList();
			mcLabs				 = new CourseEventsList();
			mcProjects			 = new CourseEventsList();
		}
		#region Methods

		public CourseEvent FindEvent (CourseEvent toFind)
		{
			string typeToFind = toFind.Type;
			CourseEventsList col;
			switch (typeToFind)
			{
				case "הרצאה": { col = this.Lectures; break; }
				case "מעבדה": { col = this.Labs; break; }
				case "קבוצה": { col = this.Projects; break; }
				default: { col = this.Tutorials; break; }
			}
			
			foreach (int i in col.Keys)
			{
				CourseEvent ce=(CourseEvent)(col[i]);
				if (ce.Equals(toFind))
					return ce;
			}
			return null;
		}

		private void AddRegistrationGroup( RegistrationGroup group )
		{
			mcRegistrationGroups.Add( group );
		}

		public CourseEventCollection GetAllCourseEvents()
		{
			CourseEventCollection courseEvents = new CourseEventCollection();
			if (mcLectures.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcLectures)
				{
					courseEvents.Add((CourseEvent)entry.Value);
				}
			}
			
			if (mcTutorials.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcTutorials)
				{
					courseEvents.Add((CourseEvent)entry.Value);
				}
			}

			if (mcLabs.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcLabs)
				{
					courseEvents.Add((CourseEvent)entry.Value);
				}
			}

			if (mcProjects.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcProjects)
				{
					courseEvents.Add((CourseEvent)entry.Value);
				}
			}
			return courseEvents;
		}

		public void CreateRegistrationGroups()
		{
			mcRegistrationGroups = new RegistrationGroupsCollection();
			bool flag=false;
			RegistrationGroup aRegGroup;
			CourseEventsList doneList = new CourseEventsList();
			
			
			// Mode 1 - Only lectures
			if ((mcLectures.Count>0)&&(mcTutorials.Count==0)&&(mcLabs.Count==0)&&(mcProjects.Count==0))
			{
				foreach(System.Collections.DictionaryEntry entry in mcLectures)
				{
                    aRegGroup=new RegistrationGroup(this, (int)entry.Key);
					aRegGroup.Lecture=(CourseEvent)entry.Value;
					this.AddRegistrationGroup(aRegGroup);
				}
				flag=true;
			}

			// Mode 2 - Only Tutorials
			if ((mcLectures.Count==0)&&(mcTutorials.Count>0)&&(mcLabs.Count==0)&&(mcProjects.Count==0))
			{
				foreach(System.Collections.DictionaryEntry entry in mcTutorials)
				{
					aRegGroup=new RegistrationGroup(this, (int)entry.Key);
					aRegGroup.Tutorial=(CourseEvent)entry.Value;
					this.AddRegistrationGroup(aRegGroup);
				}
				flag=true;
			}

			// Mode 3 - Only Labs
			if ((mcLectures.Count==0)&&(mcTutorials.Count==0)&&(mcLabs.Count>0)&&(mcProjects.Count==0))
			{
				foreach(System.Collections.DictionaryEntry entry in mcLabs)
				{
					aRegGroup=new RegistrationGroup(this, (int)entry.Key);
					aRegGroup.Lab=(CourseEvent)entry.Value;
					this.AddRegistrationGroup(aRegGroup);
				}
				flag=true;
			}
			
			// Mode 4 - Only Project Groups
			if ((mcLectures.Count==0)&&(mcTutorials.Count==0)&&(mcLabs.Count==0)&&(mcProjects.Count>0))
			{
				foreach(System.Collections.DictionaryEntry entry in mcProjects)
				{
					aRegGroup=new RegistrationGroup(this, (int)entry.Key);
					aRegGroup.Project=(CourseEvent)entry.Value;
					this.AddRegistrationGroup(aRegGroup);
				}
				flag=true;
			}

			// Mode 5 - Mixed
			if (!flag) // If mode is not any of the previous modes
			{
				// Iterate over Tutorials
				foreach(System.Collections.DictionaryEntry entry in mcTutorials)
				{
					// If we did not do this group yet
					if (!(doneList.ContainsKey(entry.Key)))
					{
						aRegGroup=new RegistrationGroup(this, (int)entry.Key);
						aRegGroup.Tutorial=(CourseEvent)entry.Value;
						int lectureKey = (((int)(entry.Key))/10)*10;
						if (Lectures.ContainsKey(lectureKey))
						{
							aRegGroup.Lecture=(CourseEvent)Lectures[lectureKey];
						}
						if (Labs.ContainsKey(entry.Key))
						{
							aRegGroup.Lab=(CourseEvent)Labs[entry.Key];
						}
						if (Projects.ContainsKey(entry.Key))
						{
							aRegGroup.Project=(CourseEvent)Projects[entry.Key];
						}
						this.AddRegistrationGroup(aRegGroup);
						doneList.Add(entry.Key, entry.Value);
					}
				}

				// Iterate over Labs
				foreach(System.Collections.DictionaryEntry entry in mcLabs)
				{
					// If we did not do this group yet
					if (!(doneList.ContainsKey(entry.Key)))
					{
						aRegGroup=new RegistrationGroup(this, (int)entry.Key);
						aRegGroup.Lab=(CourseEvent)entry.Value;
						int lectureKey = (((int)(entry.Key))/10)*10;
						if (Lectures.ContainsKey(lectureKey))
						{
							aRegGroup.Lecture=(CourseEvent)Lectures[lectureKey];
						}
						if (Tutorials.ContainsKey(entry.Key))
						{
							aRegGroup.Tutorial=(CourseEvent)Tutorials[entry.Key];
						}
						if (Projects.ContainsKey(entry.Key))
						{
							aRegGroup.Project=(CourseEvent)Projects[entry.Key];
						}
						this.AddRegistrationGroup(aRegGroup);
						doneList.Add(entry.Key, entry.Value);
					}
				}

				// Iterate over Project Groups
				foreach(System.Collections.DictionaryEntry entry in mcProjects)
				{
					// If we did not do this group yet
					if (!(doneList.ContainsKey(entry.Key)))
					{
						aRegGroup=new RegistrationGroup(this, (int)entry.Key);
						aRegGroup.Project=(CourseEvent)entry.Value;
						int lectureKey = (((int)(entry.Key))/10)*10;
						if (Lectures.ContainsKey(lectureKey))
						{
							aRegGroup.Lecture=(CourseEvent)Lectures[lectureKey];
						}
						if (Tutorials.ContainsKey(entry.Key))
						{
							aRegGroup.Tutorial=(CourseEvent)Tutorials[entry.Key];
						}
						if (Labs.ContainsKey(entry.Key))
						{
							aRegGroup.Lab=(CourseEvent)Labs[entry.Key];
						}
						this.AddRegistrationGroup(aRegGroup);
						doneList.Add(entry.Key, entry.Value);
					}
				}
			}
		}

		#region Debug Methods		
		public void PrintCourseToConsoleByGroups()
		{
			string text = "Course " + mName + " " + mNumber+ "\n"
				+ "Lecturer= " + mLecturer + "\n"
				+ "Academic Points= " + mAcademicPoints + "\n"
				+ "Lecture/Tutorial/Lab/Project hours: " + mLectureHours + "/" + mTutorialHours + "/" + mLabHours + "/" + mProjectHours + "\n"
				+ "Tests A/B= " + mMoadA.ToShortDateString() + "/" + mMoadB.ToShortDateString() + "\n";
						
			foreach (RegistrationGroup group in mcRegistrationGroups)
			{
				string regGroupText="Registration Group: " + group.Number.ToString() + "\n";
				
				if (group.Lecture!=null)
				{
					regGroupText+="	Type=" + group.Lecture.Type + " Teacher=" + group.Lecture.Giver + "\n";
					foreach (CourseEventOccurrence ceo in group.Lecture.Occurrences)
					{
						regGroupText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
				if (group.Tutorial!=null)
				{
					regGroupText+="	Type=" + group.Tutorial.Type + " Teacher=" + group.Tutorial.Giver + "\n";
					foreach (CourseEventOccurrence ceo in group.Tutorial.Occurrences)
					{
						regGroupText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
				if (group.Lab!=null)
				{
					regGroupText+="	Type=" + group.Lab.Type + " Teacher=" + group.Lab.Giver + "\n";
					foreach (CourseEventOccurrence ceo in group.Lab.Occurrences)
					{
						regGroupText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
				
				if (group.Project!=null)
				{
					regGroupText+="	Type=" + group.Project.Type + " Teacher=" + group.Project.Giver + "\n";
					foreach (CourseEventOccurrence ceo in group.Project.Occurrences)
					{
						regGroupText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
				text+=regGroupText;
			}

			System.Windows.Forms.MessageBox.Show(text);
		}

		public void PrintCourseToConsoleByEvents()
		{
			string text = "Course " + mName + " " + mNumber+ "\n"
				+ "Lecturer= " + mLecturer + "\n"
				+ "Academic Points= " + mAcademicPoints + "\n"
				+ "Lecture/Tutorial/Lab/Project hours: " + mLectureHours + "/" + mTutorialHours + "/" + mLabHours + "/" + mProjectHours + "\n"
				+ "Tests A/B= Moed A at:" + mMoadA.ToShortDateString() + " Moed B at:" + mMoadB.ToShortDateString() + "\n";
						

			string EventListText=string.Empty;

			if (mcLectures.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcLectures)
				{
					EventListText+="Lecture number: " + entry.Key + " ";
					CourseEvent ce=(CourseEvent)entry.Value;
					EventListText+="Teacher: " + ce.Giver + "\n";
					foreach (CourseEventOccurrence ceo in ce.Occurrences)
					{
						EventListText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
			}
			else
			{
				EventListText+="No Lectures\n";
			}

			if (mcTutorials.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcTutorials)
				{
					EventListText+="Tutorial number: " + entry.Key + " ";
					CourseEvent ce=(CourseEvent)entry.Value;
					EventListText+="Teacher: " + ce.Giver + "\n";
					foreach (CourseEventOccurrence ceo in ce.Occurrences)
					{
						EventListText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
			}
			else
			{
				EventListText+="No Tutorials\n";
			}

			if (mcLabs.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcLabs)
				{
					EventListText+="Tutorial number: " + entry.Key + " ";
					CourseEvent ce=(CourseEvent)entry.Value;
					EventListText+="Teacher: " + ce.Giver + "\n";
					foreach (CourseEventOccurrence ceo in ce.Occurrences)
					{
						EventListText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
			}
			else
			{
				EventListText+="No Labs\n";
			}

			if (mcProjects.Count>0)
			{
				foreach (System.Collections.DictionaryEntry entry in mcProjects)
				{
					EventListText+="Project Group number: " + entry.Key + " ";
					CourseEvent ce=(CourseEvent)entry.Value;
					EventListText+="Teacher: " + ce.Giver + "\n";
					foreach (CourseEventOccurrence ceo in ce.Occurrences)
					{
						EventListText+="		on " + ceo.Day + " time:" + ceo.Hour.ToString() + "-" + (ceo.Hour+ceo.Duration).ToString() + " at " + ceo.Location + "\n";
					}
				}
			}
			else
			{
				EventListText+="No Project Groups\n";
			}

			text+=EventListText;
			System.Windows.Forms.MessageBox.Show(text);
		}
		
		#endregion
	
		public override string ToString()
		{
			return mNickName;
		}

        #endregion Methods
        #region Properties
        /// <summary>
        /// Course's name
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
			set
			{
				mName=value;
				mNickName=value;
			}
        } 
		/// <summary>
		/// The Course's nickname  (i.e Hedva instead of Differential and Integral Calculus)
		/// </summary>
		public string NickName
		{
			get
			{
				return mNickName;
			}
			set
			{
				mNickName = value;
			}
		}
		/// <summary>
		///  Faculty this course is given by
		/// </summary>
		public string Faculty
		{
			get
			{
				return mFaculty;
			}
			set
			{
				mFaculty=value;
			}
		} 
		/// <summary>
		/// Course's Lecturer In Charge
		/// </summary>
		public string Lecturer
		{
			get
			{
				return mLecturer;
			}
			set
			{
				mLecturer=value;
			}
		}
        /// <summary>
        /// Course's number
        /// </summary>
        public string Number
        {
            get
            {
                return mNumber;
            }
			set
			{
				mNumber=value;
			}
        }
        /// <summary>
        /// The number of academic points for this Course
        /// </summary>
        public float AcademicPoints
        {
            get
            {
                return mAcademicPoints;
            }
			set
			{
				mAcademicPoints=value;
			}
        }
        /// <summary>
        /// The number of lecture's hours for this Course
        /// </summary>
        public int LectureHours
        {
            get
            {
                return mLectureHours;
            }
			set
			{
				mLectureHours=value;
			}
        }
        /// <summary>
        /// The number of tutorial's hours for this Course
        /// </summary>
        public int TutorialHours
        {
            get
            {
                return mTutorialHours;
            }
			set
			{
				mTutorialHours=value;
			}
        }
        /// <summary>
        /// The number of lab's hours for this Course
        /// </summary>
        public int LabHours
        {
            get
            {
                return mLabHours;
            }
			set
			{
				mLabHours=value;
			}
        }
        /// <summary>
        /// The number of project's hours for this Course
        /// </summary>
        public int ProjecHours
        {
            get
            {
                return mProjectHours;
            }
			set
			{
				mProjectHours=value;
			}
        }
        /// <summary>
        /// Course Moad A date
        /// </summary>
        public DateTime MoadA
        {
            get
            {
                return mMoadA;
            }
			set
			{
				mMoadA=value;
			}
        }
        /// <summary>
        /// Course Moad B date
        /// </summary>
        public DateTime MoadB
        {
            get
            {
                return mMoadB;
            }
			set
			{
				mMoadB=value;
			}
        }
        /// <summary>
        /// Course's registration groups
        /// </summary>
        public RegistrationGroupsCollection RegistrationGroups
        {
			get
			{
				if( mcRegistrationGroups == null )
				{
					this.CreateRegistrationGroups();
				}
				return mcRegistrationGroups; 
			}
			set
			{
				mcRegistrationGroups = value;
			}
        }

		public CourseEventsList Lectures
		{
			get{ return mcLectures; }
		}

		public CourseEventsList Tutorials
		{
			get{ return mcTutorials; }
		}

		public CourseEventsList Labs
		{
			get{ return mcLabs; }
		}

		public CourseEventsList Projects
		{
			get{ return mcProjects; }
		}

        #endregion Properties
		#region IComparable Members
		public int CompareTo(object obj)
		{
			Course to = (Course) obj;
			return mNumber.CompareTo( to.Number );
		}
		#endregion
	}


	public struct CourseID
	{
		public string CourseName;
		public string CourseNumber;
		public override string ToString()
		{
			return CourseName;
		}

		#region Operators
		public static bool operator == (CourseID obj1, CourseID obj2)
		{
			if (obj1.CourseNumber==obj2.CourseNumber)
				return true;
			else
				return false;
		}

		public static bool operator != (CourseID obj1, CourseID obj2)
		{
			if (obj1.CourseNumber!=obj2.CourseNumber)
				return true;
			else
				return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is CourseID)
			{
				if (this.CourseNumber==((CourseID)obj).CourseNumber)
					return true;
				else
					return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		#endregion

	}
}
