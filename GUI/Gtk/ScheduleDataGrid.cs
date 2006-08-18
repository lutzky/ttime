
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using UDonkey.Logic;
using Gtk;

namespace UDonkey.GUI
{
	/// <summary>
	/// ScheduleGrid displays a schedule.
	/// </summary>
	public class ScheduleDataGrid 
	{
		private Schedule        mSchedule;
		private HitTestInfo     mHitTestInfo;
		private HTML mHTML;
		
		public ScheduleDataGrid()
		{
            mHTML = new HTML();
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
		}

		public void ShowColumn( string name )
		{
		}
		public void ResetTableStyles()
		{
		}
		/// <summary>
		/// Gets only Schedule object
		/// </summary>
		public new Schedule DataSource
		{
			set
			{
                if (value == null)
                {
                    mHTML.LoadEmpty();
                    return;
                }

                UDonkey.IO.IOManager.ExportSchedToHtml("Print.html", (Schedule)value);

                FileStream fstream = new FileStream("Print.html", FileMode.Open, FileAccess.Read);

                HTMLStream stream = mHTML.Begin();
                byte [] buffer = new byte[8192];
                int count;
                while ((count =  fstream.Read(buffer, 0, 8192)) != 0)
                        stream.Write(buffer, count);
                mHTML.End(stream, HTMLStreamStatus.Ok);

                /*
				if( (value != null) && !(value is Schedule) )
				{
					throw new ArgumentException( "DataSource is not of type Schedule", "Value" );
				}
				if( mSchedule != null )
				{
					mSchedule.Changed -= new EventHandler(mSchedule_Changed);
				}
                if( value == null )
                {
                    base.DataSource = null;
                    return;
                }
   				mSchedule = value as Schedule;
				mSchedule.Changed += new EventHandler(mSchedule_Changed);
                if( !mbStyleCreated )
                {
                    ResetTableStyles();
                    mbStyleCreated = true;
                }
				base.DataSource  = mSchedule.DataTable;*/
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
        }

        public void ParentRefresh()
        {
            Refresh();
        }

		public static implicit operator Widget(ScheduleDataGrid grid)
        {
            return grid.mHTML;
        }

	}

}
