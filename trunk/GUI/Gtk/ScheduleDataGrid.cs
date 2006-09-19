
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using UDonkey.Logic;
using Gtk;
using Mono.Unix.Native;
using Gecko;

namespace UDonkey.GUI
{
  /// <summary>
  /// ScheduleGrid displays a schedule.
  /// </summary>
  public class ScheduleDataGrid 
  {
    private Schedule        mSchedule;
    private HitTestInfo     mHitTestInfo;
    private WebControl      mWebControl;

    public ScheduleDataGrid()
    {
      mWebControl = new WebControl();
      mWebControl.Show();

      mWebControl.Progress += new ProgressHandler(on_progress);
      mWebControl.NetState += new NetStateHandler(on_netstate);
    }

    public new  HitTestInfo HitTest(Point position)
    {
      return this.HitTest( position.X, position.Y );
    }
    public new  HitTestInfo HitTest(int x, int y)
    {
      /* TODO
       * HitTestInfo info = base.HitTest( x, y );
       return new HitTestInfo( this, info, mSchedule );
       */
      return null;
    }
    public bool GetUsersEventParameters( 
        out string eventName, out DayOfWeek day,
        out int hour, out int duration, bool setByPoint )
    {
      eventName = "";
      day = DayOfWeek.ראשון;
      hour = 1;
      duration = 1;
      setByPoint = false;
      return false;
    }

    public void HideColumn( string name )
    {
      //this is a BAD thing
    }

    public void ShowColumn( string name )
    {

      //this is a BAD thing
    }
    public void ResetTableStyles()
    {

      //this is a BAD thing
    }
    /// <summary>
    /// Gets only Schedule object
    /// </summary>
    public new Schedule DataSource
    {
      set
      {
        if( mSchedule != null )
        {
          mSchedule.Changed -= new EventHandler(mSchedule_Changed);
        }

        mSchedule = value as Schedule;

        if (mSchedule == null)
        {
          mWebControl.LoadUrl("about:blank");
          return;
        }

        mSchedule.Changed += new EventHandler(mSchedule_Changed);

        Refresh();

      }
    }



    public Schedule Schedule
    {
      get{ return mSchedule; }
    }

    private string msHoverString;
    public string HoverString
    {
      get { return msHoverString; }
      set { msHoverString = value; }
    }
    /// <summary>
    /// Contains information about a part of the UDonkey.GUI.ScheduleGrid at a specified coordinate. 
    /// This class cannot be inherited.
    /// </summary>
    public sealed new class HitTestInfo
    {
      private ScheduleDataGrid     mGrid;
      private Schedule             mSchedule;
      public enum HitTestType { 
        Cell, ColumnHeader
      };

      public HitTestInfo( ScheduleDataGrid grid, HitTestInfo hitTestinfo, Schedule scedule )
      {
        mGrid        = grid;
        mSchedule    = scedule;
      }

      /// <summary>
      /// Gets the number of the column the user has clicked.
      /// </summary>
      public int Column
      {
        get { return -1 /*mHitTestInfo.Column;*/; }
      }
      /// <summary>
      /// Gets the number of the row the user has clicked.
      /// </summary>
      public int Row
      {
        get { return -1; /*return mHitTestInfo.Row;*/ }
      }
      /// <summary>
      /// /// Gets the part of the UDonkey.GUI.ScheduleGrid control, other than the row or column, that was clicked.
      /// </summary>
      HitTestType mType;
      public HitTestType Type 
      {
        get{ return mType; }
      }

      /// <summary>
      /// Gets the day of the column the user has clicked.
      /// </summary>
      public DayOfWeek Day
      {
        get
        {/*
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek),
            mGrid.TableStyles[ Schedule.DATA_TABLE_NAME ].GridColumnStyles[ this.Column ].HeaderText
            ,true); */
          return DayOfWeek.ראשון;
        }
      }

      /// <summary>
      /// Gets the hour of the row the user has clicked.
      /// </summary>
      public int Hour
      {
        get{ return -1; /*return this.Row + mSchedule.StartHour;*/ }
      }

      public IScheduleEntry Object
      {
        get{ return mSchedule.FullDataTable.Rows[Hour][Column] as IScheduleEntry; }
      }
    }

    private event GridMouseEventHandler mMouseMove;
    public new event GridMouseEventHandler MouseMove
    {
      add { mMouseMove += value; }
      remove { mMouseMove -= value; }
    }
    private event GridMouseEventHandler mMouseDown;
    public new event GridMouseEventHandler MouseDown
    {
      add { mMouseDown += value; }
      remove { mMouseDown -= value; }
    }

    public void AddContextMenuItem(ScheduleMenuItem item)
    {
    }

    public void Refresh()
    {
      //      StringBuilder filename = new StringBuilder("PrintXXXXXX");
      //      Mono.Unix.Native.Syscall.mkstemp(filename);
      //      UDonkey.IO.IOManager.ExportSchedToHtml(filename.ToString(), mSchedule);
      //
      //      mWebControl.LoadUrl("file://"+System.IO.Directory.GetCurrentDirectory()+"/HTML/empty.html");

      if(mSchedule == null)
        return;

      Console.WriteLine("msched ok");
      ArrayList al = new ArrayList();
      DataTable mDataTable = mSchedule.FullDataTable;


      foreach (DataColumn myCol in mDataTable.Columns)
      {			
        Console.WriteLine("loop");
        //going throughout the rows (hour)
        foreach(DataRow myRow in mDataTable.Rows)
        {			
          Console.WriteLine("   loop2");
          IScheduleEntry entry =  myRow[myCol] as IScheduleEntry;
          if ( !entry.IsEmpty )
          {

            foreach( System.Collections.DictionaryEntry de in  entry )
            {
              Console.WriteLine("       loop3");

              IScheduleObject iso = (IScheduleObject) de.Value;
              //start ScheduleObject
              
              /*
             * format: " addEvent(course_index,type,day,start,end,desc);\n"
             * course_index - an inumeration of the courses.
             * type - tirgul or lecture. for now ignored
             * day - 1..5
             * start - 8 for 8:30, 16 for 16:30
             * end - 8 for 8:30, 16 for 16:30
             * desc - the string that will be displayed for this
             */
            if(iso!=null){
              al.Add("addEvent(1,'tirgul',"+(int)(iso.DayOfWeek)+","+iso.StartHour+","
                  +(iso.Duration+iso.StartHour - 1)+","+iso.ToString()+");\n");
              Console.WriteLine("event added");
            }

            }												
          }
        }
      }
      foreach ( DayOfWeek day in Enum.GetValues( typeof(DayOfWeek) ) ){
        //FIXME magic number
        int i;
        for(i=8;i<=20;i++){
          IScheduleEntry ise = mSchedule.FullDataTable.Rows[i][(int)day] as IScheduleEntry;
          foreach (IScheduleObject iso in ise){
            /*
             * format: " addEvent(course_index,type,day,start,end,desc);\n"
             * course_index - an inumeration of the courses.
             * type - tirgul or lecture. for now ignored
             * day - 1..5
             * start - 8 for 8:30, 16 for 16:30
             * end - 8 for 8:30, 16 for 16:30
             * desc - the string that will be displayed for this
             */
            if(iso!=null){
              al.Add("addEvent(1,'tirgul',"+(int)(iso.DayOfWeek)+","+iso.StartHour+","
                  +(iso.Duration+iso.StartHour - 1)+","+iso.ToString()+");\n");
              Console.WriteLine("event added");
            }
          }
        }
      }
      Console.WriteLine("no more look");

      TextReader htmlFile = new StreamReader("HTML/SchedTable.html");
      mWebControl.OpenStream("file://"+System.IO.Directory.GetCurrentDirectory(),"text/html");
      String html = htmlFile.ReadToEnd();
      foreach(String ev in al){
        html = html.Replace("//TEMPLATE//","//TEMPLATE//\n"+ev); 
      }
      StreamWriter w =  File.CreateText( "TemporarySchedule.html" );
      w.Write(html);
      w.Close();

      mWebControl.LoadUrl("file://"+System.IO.Directory.GetCurrentDirectory()+"/TemporarySchedule.html");


      //      mWebControl.AppendData(html);
      //      Console.WriteLine(html);
      //      mWebControl.CloseStream();



      //FileStream fstream = new FileStream(filename.ToString(), FileMode.Open, FileAccess.Read);
      //

      //mWebControl.LoadUrl("http://www.google.com");
    }

    public void ParentRefresh()
    {
      Refresh();
    }

    public static implicit operator WebControl(ScheduleDataGrid grid)
    {
      return grid.mWebControl;
    }

#region Event Handlers
    private void mSchedule_Changed(object sender, EventArgs e)
    {
      Console.WriteLine("mSchedule_Changed");
      Refresh();
    }

    private void on_html_LoadDone(object sender, EventArgs e)
    {
      Console.WriteLine("onLoadDone");
    }

    private void on_progress(object sender, Gecko.ProgressArgs e)
    {
      Console.WriteLine("on_progress - " + e.Curprogress);
    }

    private void on_netstate(object sender, NetStateArgs e)
    {
      Console.WriteLine("on_netstate");
    }
#endregion
  }

}
