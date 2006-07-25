using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// TODO: Summary description for RegistrationGroup.
	/// </summary>
	public class RegistrationGroup
	{
        #region Private Variables
        private Course                 mCourse;
        private int                    mNumber;
        private CourseEvent mLecture;
        private CourseEvent mTutorial;
        private CourseEvent mLab;
        private CourseEvent mProject;
        #endregion Private Methods
        #region Consturctors
        /// <summary>
        /// Basic constructor
        /// </summary>
		public RegistrationGroup( Course course, int number)
		{
            mCourse    = course;
            mNumber    = number;
		}

        #endregion Consturctors
        #region Properties
        /// <summary>
        /// Return the course that this RegistrationGroup belongs to 
        /// </summary>
        public Course Course
        {
            get{ return mCourse; }
        }
        /// <summary>
        /// The RegistrationGroup's number;
        /// </summary>
        public int Number
        {
            get{ return mNumber; }
        }
        /// <summary>
        /// The lecture of this RegistrationGroup
        /// </summary>
        public CourseEvent Lecture
        {
			set{ mLecture = value; }
            get{ return mLecture; }
        }
        /// <summary>
        /// The tutorial of this RegistrationGroup
        /// </summary>
        public CourseEvent Tutorial
        {
			set{ mTutorial = value; }
            get{ return mTutorial; }
        }
        /// <summary>
        /// The lab of this RegistrationGroup
        /// </summary>
        public CourseEvent Lab
        {
			set{ mLab = value; }
            get{ return mLab; }
        }
        /// <summary>
        /// The project of this RegistrationGroup
        /// </summary>
		public CourseEvent Project
		{
			set{ mProject = value; }
			get{ return mProject; }
		}
        #endregion Properties
	}
}
