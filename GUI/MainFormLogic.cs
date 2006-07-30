using System;
using System.Windows.Forms;
using UDonkey.RepFile;
using UDonkey.Logic;
using System.Threading;

namespace UDonkey.GUI
{
  /// <summary>
  /// MainFormLogic controls the MainForm events.
  /// </summary>
  public class MainFormLogic
  {
    private MainForm          mMainForm;

    //screw good code. damn C#
    private CoursesScheduler  mScheduler {get { return CoursesScheduler;}};
    private UDonkeyClass	  mDonkey {get{return UDonkeyClass;}}
    private const string RESOURCES_GROUP = "MainForm";  
    private SchedulingProgressbar mProgressBar;
    private int				  progressCounter;

    public MainFormLogic( UDonkeyClass udonkey, CoursesScheduler sceduler )
    {	
      mDonkey    = udonkey;
      mScheduler = sceduler;
      mScheduler.StartScheduling += new SchedulingProgress( this.StartScheduling );
      mScheduler.ContinueScheduling += new SchedulingProgress( this.ContinueScheduling );
      mScheduler.EndScheduling   += new SchedulingProgress( this.EndScheduling );
      mProgressBar = new SchedulingProgressbar();
      InitComponents();
      progressCounter=0;
    }

    private void InitComponents()
    {		
      mMainForm = new MainForm( this );
    }




    /*
    public void ToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
    {
      ToolBarButton button = e.Button;
      switch ( button.Tag.ToString() )
      {
        case "Reset":
        {
          mDonkey.Reset();
          break;
        }
        case "Scedule":
        {
          mMainForm.ConfigControl.Save.PerformClick();
          this.ScheduleSchedules();
          break;
        }
        case "Prev10StatesButton":
        {
          this.SetScedulerState( this.mScheduler.Index - 10 );
          break;
        }
        case "PrevStateButton":
        {
          this.SetScedulerState( this.mScheduler.Index - 1 );
          break;
        }
        case "NextStateButton":
        {
          this.SetScedulerState( this.mScheduler.Index + 1 );
          break;
        }
        case "Next10StatesButton":
        {
          this.SetScedulerState( this.mScheduler.Index + 10 );
          break;
        }

        case "SaveView":
        {
          string file = GetFileName( false );
          UDonkey.IO.IOManager.ExportSystemState( file, mScheduler );
          break;
        }
        case "LoadView":
        {
          string file = GetFileName( true );
          mDonkey.ImportSystemState( file );
          mMainForm.ConfigControl.Save.PerformClick();
          // Set the Academic point counter to a correct value
          float counter =0;
          foreach (Course c in mDonkey.Scheduler.Courses.Values)
          {
            counter+=c.AcademicPoints;
          }
          mMainForm.DBBrowserControl.SelectedPoints = counter.ToString();
          break;
        }
        case "Print":
        {
          this.Print();
          break;
        }
        case "CourseList":
        {
          this.CourseList();
          break;

        }
        case "Something":
        {
          this.Something();
          break;
        }
      }
      return;
    }*/

    public void Run()
    {
      mMainForm.Start();
    }

    /// depricated
    /// should be made private
    public void SetScedulerState( int index )
    {
      this.mScheduler.Index = index;
      this.SetStatusBarLine( 
          string.Format( Resources.String( RESOURCES_GROUP, "ScheduleText" ), 
            mScheduler.Index + 1,
            mScheduler.Count ,
            mScheduler.CurrentState.Mark) );
      mMainForm.RefreshView();
    }

    public void MoveSchedulerState(int diff){
      SetScedulerState(mScheduler.Index + diff);
    }

    public void SetStatusBarLine( string line )
    {
      mMainForm.StatusBar.Text = line;
    }

    /// <summary>
    /// Load schedule from XML file
    /// </summary>
    /// <param name="file">XML file that contains a schedule</param>
    public void LoadSchedule( string file )
    {
      Schedule s = Schedule.CreateFromXml( file );
      if( s != null )
      {
        string name = file.Substring( file.LastIndexOf(@"\") + 1 );
        TabPageContainer page = new TabPageContainer(name);
        ScheduleDataGrid grid = new ScheduleDataGrid();
//        grid.Dock = System.Windows.Forms.DockStyle.Fill; // not needed.
        grid.DataSource = s;
        page.AddScheduleDataGrid( grid );
        mMainForm.AddPage ( page );
      }
      else
      {
        throw new Exception();
        MessageBox.Show(Resources.String( RESOURCES_GROUP, "LoadError" ));
      }
    }
    /// <summary>
    /// Create printable Html file and load it in the default
    /// browser
    /// </summary>
    public void Print()
    {
      UDonkey.IO.IOManager.ExportSchedToHtml("Print.html", mScheduler.Schedule );
      System.Diagnostics.Process.Start( "Print.html" );
    }
    public void CourseList()
    {
      UDonkey.IO.IOManager.ExportCourseListToHtml("Courses.html", mScheduler.Courses );
      System.Diagnostics.Process.Start( "Courses.html" );
    }
    /// <summary>
    /// Schedule schedules according to the constraints
    /// </summary>
    /// <returns>If the scheduling succeeded</returns>
    public void ScheduleSchedules()
    {   
      if( mScheduler.Courses.Count == 0 )
      {   
        System.Windows.Forms.MessageBox.Show("נא לבחור קורס אחד לפחות על מנת לסדר מערכות");
        return;
      }
      mProgressBar = new SchedulingProgressbar();
      mProgressBar.Reset();
      mMainForm.Grid.DataSource = null;
      mProgressBar.Show();
      Thread thread  = new Thread( new ThreadStart( this.mScheduler.CreateSchedules ) );
      thread.Start();
      thread.Join();
      mProgressBar.Close();
      mMainForm.BringToFront();

      mDonkey.RefreshSchedule();
      if( this.mScheduler.Count != 0 )
      {
        mScheduler.States.MaxGrade = ((SchedulerState)mScheduler.States[0]).Mark;
        mScheduler.States.MinGrade = ((SchedulerState)mScheduler.States[this.mScheduler.Count-1]).Mark;
        foreach( SchedulerState state in mScheduler.States)
        {
          if( mScheduler.States.MaxGrade != mScheduler.States.MinGrade )
          {
            state.Mark = (state.Mark-mScheduler.States.MinGrade) * 100 / (mScheduler.States.MaxGrade-mScheduler.States.MinGrade);
          }
          else
          {
            state.Mark = 100;
          }
        }
        this.SetScedulerState( this.mScheduler.Index + 1 );
        mMainForm.SelectedTab = 0;
        mMainForm.SetNavigationButton( true );
      }
      else
      {
        mMainForm.SetNavigationButton( false );
        if ( mScheduler.Errors.Count != 0 )
        {
          string s = Resources.String( RESOURCES_GROUP, "ConstraintFailMessage1" );
          s += Resources.String( RESOURCES_GROUP, "ConstraintFailMessage2" );
          s += CreateErrorMessage( mScheduler.Errors );
          //MessageBox.Show( s );
          MessageBox.Show( null, s, Resources.String( RESOURCES_GROUP, "ConstraintFailMessage4" ), MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        }
        if( mScheduler.Courses.Count != 0 )
        {
          MessageBox.Show( null, "לא מצא מערכות עבורך UDonkey", Resources.String( RESOURCES_GROUP, "ConstraintFailMessage4" ), MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
          this.SetStatusBarLine("לא מצא מערכות עבורך UDonkey");
        }
        else
        {
          this.SetStatusBarLine("נא לבחור קורס אחד לפחות על מנת לסדר מערכות");
        }
      }
    }
    public MainForm MainForm
    {
      get { return mMainForm; }
    }

    // 
    public string UpdateAndGetAcademicPoints(string file){
      mDonkey.ImportSystemState( file );
      float counter =0;
      foreach (Course c in mDonkey.Scheduler.Courses.Values)
      {
        counter+=c.AcademicPoints;
      }
      return counter.ToString();
    }


    public void LoadView(){
      mMainForm.LoadView();
    }

    public void SaveView(){
      mMainForm.LoadView();
    }

    private string CreateErrorMessage( System.Collections.IDictionary dic )
    {   string ret = string.Empty;
      foreach( System.Collections.DictionaryEntry entry in dic )
      {
        string name = string.Empty;
        switch( (ScheduleErrors)entry.Key )
        {
          case ScheduleErrors.EndHour:
            {
              name = Resources.String( RESOURCES_GROUP, "EndHourConstraint" );
              break;
            }
          case ScheduleErrors.FreeDay:
            {
              name = Resources.String( RESOURCES_GROUP, "FreeDayConstraint" );
              break;
            }
          case ScheduleErrors.Overlaps:
            {
              name = Resources.String( RESOURCES_GROUP, "NoOverlapConstraint" );
              break;
            }
          case ScheduleErrors.StartHour:
            {
              name = Resources.String( RESOURCES_GROUP, "StartHourConstraint" );
              break;
            }
          case ScheduleErrors.UserEvent:
            {
              name = Resources.String( RESOURCES_GROUP, "UserEventConstraint" );
              break;
            }
        }
        string s = Resources.String( RESOURCES_GROUP, "ConstraintFailMessage3" );
        ret += string.Format( s,
            name,
            entry.Value
            );
      }
      return ret;
    }

    private void StartScheduling( int progress )
    {
      progressCounter = 0;
      mProgressBar.SetMax( progress );
    }
    private void ContinueScheduling( int progress )
    {
      progressCounter+=progress;
      if (progressCounter>5000)
      {
        mProgressBar.Progress( progressCounter );
        progressCounter =0;
      }

    }

    private void EndScheduling( int progress )
    {
      mProgressBar.Close();

    }

    public void AutoUpdate(){
      mDonkey.CourseDB.AutoUpdate(); // this my throw System.Net.WebException
                                     // handeled by GUI.
      mDonkey.CourseDB.Unload(Constants.MainDB);
      RepFileConvertForm form = new RepFileConvertForm();
      // try defualt REPY
      RepToXML.Convert("REPY", System.IO.Directory.GetCurrentDirectory() + "\\" + Constants.MainDB);
      mDonkey.CourseDB.Load(Constants.MainDB);
      mDonkey.Reset();
      mDonkey.DBLogic.Load();
    }
    
    // FIXME: why does this exist?
    private void Something()
    {
    }

    public void SetNavigationButton(bool en){
      mMainForm.SetNavigationButton(en);
    }
  }
}
