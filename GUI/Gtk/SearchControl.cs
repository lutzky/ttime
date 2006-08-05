//
using System;
using System.Collections;
using System.Collections.Specialized;
using Gtk;
using GtkSharp;
using Glade;
using Gdk;

using UDonkey.Logic;

namespace UDonkey.GUI
{
	public class SearchControl 
	{
#region Glade Widgets
		[Widget] Dialog AdvancedSearchDialog;
		[Widget] Entry courseName;
		[Widget] Entry courseNumber;
		[Widget] ComboBoxEntry coursePoints;
		[Widget] ComboBoxEntry cbFaculties;
		[Widget] Entry lecturer;
	       	[Widget] CheckButton sunday, monday, tuesday, wednesday, thursday, friday;
		/*[Widget] Button btChooseAll;
		[Widget] Button btChooseNone;
		[Widget] Button btReverseChecked;
		[Widget] TreeView tvOccurences;*/
#endregion
		// members of cbFaculties
		ListStore storeFaculties;

		public SearchControl() 
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "AdvancedSearchDialog", null); 
			gxml.Autoconnect (this);

			// cbFaculties stuff
			storeFaculties = new ListStore(typeof (string));
			cbFaculties.Model = storeFaculties;
			cbFaculties.TextColumn = 0;
		}

		public void Show()
		{
			AdvancedSearchDialog.ShowAll();
		}
		
		public void Hide()
		{
			AdvancedSearchDialog.Hide();
		}

#region Properties
		private StringCollection mFaculties;
		public StringCollection Faculties {
			get { return mFaculties; }
			set {
				mFaculties = value;
				storeFaculties.Clear();
				foreach (string faculty in mFaculties)
				{
					storeFaculties.AppendValues(faculty);
				}
			}
		}

		public string SelectedFaculty {
			get { return cbFaculties.Entry.Text; }
			set { cbFaculties.Entry.Text = value; }
		}

#endregion
		

#region Event handlers
		private void on_response(object obj, ResponseArgs a)
		{
			switch (a.ResponseId)
			{
				case ResponseType.Close:
				       Hide();
				       break;
				case ResponseType.Ok:  // Find
				       	{
					       	SearchEventArgs args = new SearchEventArgs();
					        if ( courseName.Text != "")
							args.Name = courseName.Text;

						if ( courseNumber.Text != "")
							args.Number = courseNumber.Text;

						if ( coursePoints.Entry.Text != "")
							args.Points = coursePoints.Entry.Text;

						if ( cbFaculties.Entry.Text != "")
							args.Faculty = cbFaculties.Entry.Text;

						if ( lecturer.Text !="")
							args.Lecturer = lecturer.Text;

						if( sunday.Active )
							args.Days[0]="א";

						if( monday.Active )
							args.Days[1]="ב";

						if( tuesday.Active )
							args.Days[2]="ג";

						if( wednesday.Active )
							args.Days[3]="ד";

						if( thursday.Active )
							args.Days[4]="ה";

						if( friday.Active )
							args.Days[5]="ו";

						if (mSearch != null) mSearch( this, args );

				       		break;
				       	}
			}
		}
		
		private void on_delete(object obj, DeleteEventArgs args)
		{
			args.RetVal = true;  // Prevent closing
			Hide();
		}
#endregion

#region Event 
		private event SearchEventHandler mSearch;
		public event SearchEventHandler Search
		{
			add { mSearch += value; }
			remove { mSearch -= value; }
		}

		public event EventHandler Load
		{
			add { AdvancedSearchDialog.Realized += value; }
			remove { AdvancedSearchDialog.Realized -= value; }
		}
				 
#endregion

		/*public static void Main()
		{
			Application.Init();
			Gtk.Window win = new Gtk.Window("Hello World");
			win.Resize(800,600);
			DBbrowser dbb = new DBbrowser();

			win.Add(dbb);
			win.ShowAll();

			UDonkey.DB.CourseDB cdb = new UDonkey.DB.CourseDB();
			cdb.Load("MainDB.xml");

			dbb.Faculties = cdb.GetFacultyList();
			dbb.Courses = cdb.GetCoursesByFacultyName("מדעי המחשב");
			dbb.AddCourse(cdb.GetCourseByNumber("046267"));
			Application.Run();
		}*/
		
	}
	
}
