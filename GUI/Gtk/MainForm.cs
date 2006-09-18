//
using System;
using Gtk;
using GtkSharp;
using Glade;
using Gecko;

using UDonkey.Logic;

namespace UDonkey.GUI
{
	public class MainForm 
	{

        public enum TabIndices : int
        {
            ScheduleGrid = 1, DBBrowser = 0
        };

		private const uint StatusBarId = 1;
			
		private MainFormLogic Logic;
		private UDonkeyClass mDonkey;

		private DBbrowser mDBBrowser;
		private ConfigControl mConfigControl;
        private ScheduleDataGrid mScheduleGrid;
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
			boxDBBrowser.PackStart(mDBBrowser, true, true, 0);

            mScheduleGrid = new ScheduleDataGrid();
			boxScheduleGrid.PackStart(mScheduleGrid, true, true, 0);

			mConfigControl = new ConfigControl();

            MainFormWindow.Maximize();
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
		public ScheduleDataGrid Grid  
		{
			get { return mScheduleGrid; }
		}


		public DBbrowser DBBrowserControl 
		{
			get { return mDBBrowser; }
		}

		public ConfigControl ConfigControl
		{
			get { return mConfigControl; }
		}

		public TabIndices SelectedTab
		{
			get { return (TabIndices)notebook.CurrentPage; }
			set { notebook.CurrentPage = (int)value; }
		}

		public bool Enabled
		{
			get { return notebook.Sensitive; }
			set { notebook.Sensitive = value; }
		}
#endregion
		

#region Event handlers
		private void on_new_activate(object obj, EventArgs a) { } 
		private void on_open_activate(object obj, EventArgs a) { } 
		private void on_save_activate(object obj, EventArgs a) { } 
		private void on_save_as_activate(object obj, EventArgs a) { } 
		private void on_quit_activate(object obj, EventArgs a) 
        {
            Application.Quit();
        } 
		private void on_schedule_activate(object obj, EventArgs a) 
        { 
            Logic.ScheduleSchedules();
        } 

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
		
		private void on_delete_event(object obj, DeleteEventArgs args)
		{
			//args.RetVal = true;  // Prevent closing
			Application.Quit();
		}

        private void on_btPrint_clicked(object obj, EventArgs args)
        {
            (mScheduleGrid as WebControl).LoadUrl(System.IO.Path.GetFullPath("PrintTC6VLZ"));
        }
        private void on_btCourseList_clicked(object obj, EventArgs args)
        {
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
