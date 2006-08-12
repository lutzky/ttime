//
using System;
using Gtk;
using GtkSharp;
using Glade;

using UDonkey.Logic;

namespace UDonkey.GUI
{
	public class MainForm 
	{
		private const uint StatusBarId = 1;
			
		private MainFormLogic Logic;
		private UDonkeyClass mDonkey;

		private DBbrowser mDBBrowser;
		private HTML mHTML;
		private ConfigControl mConfigControl;
#region Glade Widgets
		[Widget] Window MainFormWindow;
		[Widget] Notebook notebook;
		[Widget] HBox boxDBBrowser;
		[Widget] HBox boxScheduleGrid;
		[Widget] Statusbar statusbar;

		[Widget] ToolButton btPrev10;
		[Widget] ToolButton btPrev;
		[Widget] ToolButton btNext;
		[Widget] ToolButton btNext10;
#endregion

		public MainForm(MainFormLogic logic) 
		{
			Logic = logic;
			mDonkey = logic.mDonkey;
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "MainFormWindow", null); 
			gxml.Autoconnect (this);

			mDBBrowser = new DBbrowser();
			boxDBBrowser.Add(mDBBrowser);

			mHTML = new HTML();
			boxScheduleGrid.Add(mHTML);

			mConfigControl = new ConfigControl();
		}

		public void Start()
		{
			Application.Init();
			MainFormWindow.ShowAll();
			Application.Run();
		}
		
		public void Hide()
		{
			MainFormWindow.Hide();
		}

		public void SetNavigationButton(bool enable)
		{
			btPrev10.Visible = btPrev.Visible =
			btNext.Visible = btNext10.Visible = enable;
		}

		public void SetStatusBarLine(string line)
		{
			statusbar.Pop(StatusBarId);
			statusbar.Push(StatusBarId, line);
		}

		public void RefreshView()
		{
			// TODO Refresh grid
		}

		public void AddPage(string name, ScheduleDataGrid grid)
		{
			// TODO add another grid
		}


		public void BringToFront()
		{
			// TODO
		}


		public void LoadView()
		{
		}

		public void SaveView()
		{
		}

		public static void InitGUI()
 		{
			Console.WriteLine("InitGUI");
			Application.Init();
		}

#region Properties
		public ScheduleDataGrid Grid = new ScheduleDataGrid();

		public DBbrowser DBBrowserControl 
		{
			get { return mDBBrowser; }
		}

		public ConfigControl ConfigControl
		{
			get { return mConfigControl; }
		}

		public int SelectedTab
		{
			get { return notebook.CurrentPage; }
			set { notebook.CurrentPage = value; }
		}

		public bool Enabled
		{
			get { return notebook.Sensitive; }
			set { notebook.Sensitive = value; }
		}
#endregion
		

#region Event handlers
		/*
		private Widget GladeCustomHandler(Glade.XML gxml, string func_name, string name, string str1, string str2,
				int int1, int int2)
		{
			switch (func_name)
			{
				case "createDBBrowser":
					Console.WriteLine("In createDBBrowser");
					mDBBrowser = new DBbrowser();
					return mDBBrowser;
			}

			return null;
		}
		*/
		
		private void on_new_activate(object obj, EventArgs a) { } 
		private void on_open_activate(object obj, EventArgs a) { } 
		private void on_save_activate(object obj, EventArgs a) { } 
		private void on_save_as_activate(object obj, EventArgs a) { } 
		private void on_quit_activate(object obj, EventArgs a) { } 

		private void on_autoupdate_activate(object obj, EventArgs a) 
		{
			MessageDialog md;
			try {
				Logic.AutoUpdate();
			}
			catch(System.Net.WebException)
			{
				// could not download REPY
				md = new MessageDialog(MainFormWindow, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, 
					"החיבור לאינטרנט לא הצליח.");
				md.Run(); md.Destroy();
				return;
			}
			// success
			md = new MessageDialog(MainFormWindow, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, "מסד הנתונים עודכן בהצלחה");
			md.Run(); md.Destroy();
	       	} 

		private void on_preferences_activate(object obj, EventArgs a) { } 
		private void on_udonkey_activate(object obj, EventArgs a) { } 
		private void on_about_activate(object obj, EventArgs a) { } 
/*		private void on_response(object obj, ResponseArgs a)
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
		} */
		
		private void on_delete_event(object obj, DeleteEventArgs args)
		{
			//args.RetVal = true;  // Prevent closing
			Application.Quit();
		}
#endregion

#region Event 
		/*
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
		*/		 
#endregion
	}


	
}
