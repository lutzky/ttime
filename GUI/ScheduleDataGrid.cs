using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using UDonkey.Logic;

namespace UDonkey.GUI
{
	/// <summary>
	/// ScheduleGrid displays a schedule.
	/// </summary>
	public class ScheduleDataGrid : System.Windows.Forms.DataGrid
	{
		private Schedule        mSchedule;
		private HitTestInfo     mHitTestInfo;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip ToolTip;
		private Point           mPoint;
		private string          msHoverString;
		private int             visibleColumns;
        private bool            mbStyleCreated;
		/// <summary>
		/// Base constructor set the default base DataGrid properties
		/// for this class
		/// </summary>
		public ScheduleDataGrid(): base()
		{
			//Default values
			this.AllowNavigation    = false;
			this.AllowSorting       = false;
			this.ReadOnly           = true;
			this.Dock               = System.Windows.Forms.DockStyle.Fill;
			this.HeaderForeColor    = System.Drawing.SystemColors.ControlText;
			this.Name               = "scheduleGrid";
			this.RightToLeft        = System.Windows.Forms.RightToLeft.Yes;
			this.BorderStyle        = BorderStyle.None;
			this.RowHeadersVisible  = false;
			this.mPoint = new Point( this.Left, this.Top );
            mbStyleCreated          = false;
			visibleColumns = 0;
			InitializeComponent();
		}
		public new  HitTestInfo HitTest(Point position)
		{
			return this.HitTest( position.X, position.Y );
		}
		public new  HitTestInfo HitTest(int x, int y)
		{
			DataGrid.HitTestInfo info = base.HitTest( x, y );
			return new HitTestInfo( this, info, mSchedule );
		}
		public bool GetUsersEventParameters( 
			out string eventName, out DayOfWeek day,
			out int hour, out int duration, bool setByPoint )
		{
			eventName = string.Empty;
			day       = DayOfWeek.ראשון;
			hour      = 0;
			duration  = 1;

			UsersEventForm form = new UsersEventForm();

			if ( setByPoint )
			{
				form.Location  = mPoint;
				form.EventDay  = mHitTestInfo.Day;
				form.EventHour = mHitTestInfo.Hour;
			}

			form.ShowDialog( this.Parent );
            
			if( form.OK )
			{
				eventName = form.EventName;
				day       = form.EventDay; 
				hour      = form.EventHour;
				duration  = form.EventDuration;
			}
			return form.OK;
		}

		public void HideColumn( string name )
		{
			if( TableStyles[ Schedule.DATA_TABLE_NAME]
				.GridColumnStyles[ name ].Width != 0 )
			{
				TableStyles[ Schedule.DATA_TABLE_NAME]
					.GridColumnStyles[ name ].Width = 0;
				--visibleColumns;
				this.OnResize( null );
			}
		}
		public void ShowColumn( string name )
		{
			if( TableStyles[ Schedule.DATA_TABLE_NAME]
				.GridColumnStyles[ name ].Width == 0 )
			{
				TableStyles[ Schedule.DATA_TABLE_NAME]
					.GridColumnStyles[ name ].Width = 1;
				++visibleColumns;
				this.OnResize( null );
			}
		}
		public void ResetTableStyles()
		{
			if ( this.TableStyles.Contains( Schedule.DATA_TABLE_NAME ) )
			{
				this.TableStyles.Remove( this.TableStyles[Schedule.DATA_TABLE_NAME] );
			}
			visibleColumns = mSchedule.DaysData.Count;
			this.TableStyles.Add( CreateScheduleDataGridTableStyle( mSchedule ) );
		}
		protected override void OnMouseDown(MouseEventArgs e)  
		{ 
			mHitTestInfo = this.HitTest( e.X, e.Y );
			mPoint       = new Point( e.X, e.Y );
			this.ContextMenu = new ContextMenu( );
			base.OnMouseDown(e); 
			this.ContextMenu.Show( this, mPoint);
			return;
		} 
		protected override void OnMouseMove(MouseEventArgs e)
		{
			mHitTestInfo = this.HitTest( e.X, e.Y );
			mPoint       = new Point( e.X, e.Y );
			this.ToolTip.SetToolTip( this, msHoverString );
			base.OnMouseMove (e);
		}
        protected override void OnResize( EventArgs args )
        {
            if (this.Width==0)
				return;
			int width = this.Width;
            if( this.TableStyles[ Schedule.DATA_TABLE_NAME ] != null )
            {
                foreach( DataGridColumnStyle style in this.TableStyles[ Schedule.DATA_TABLE_NAME ].GridColumnStyles )
                {
					if( style.Width != 0 )
					{
						style.Width = width / visibleColumns;
					}
                }
            }
            return;
        }
        
		protected override void OnVisibleChanged(EventArgs e)
		{
			this.OnResize( null );
			base.OnVisibleChanged (e);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ToolTip.AutomaticDelay = 1000;
			this.ToolTip.InitialDelay   = 1000;
			this.ToolTip.ReshowDelay    = 1000;
			msHoverString = string.Empty;
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
		}
	
		private void mSchedule_Changed(object sender, EventArgs e)
		{
			this.Refresh();
		}
        protected DataGridTableStyle CreateScheduleDataGridTableStyle( Schedule scehdule )
        {
            DataGridTableStyle DGStyle = new DataGridTableStyle();
            DGStyle.MappingName = Schedule.DATA_TABLE_NAME;
            DGStyle.AllowSorting = false;
            DGStyle.RowHeadersVisible = false;
          
            DataGridColumnStyle DGCStyle;
                /*= (DataGridColumnStyle)new DataGridTextBoxColumn();
            DGCStyle.MappingName = "שעות";
            DGCStyle.HeaderText  = "שעות";
            DGStyle.GridColumnStyles.Add(DGCStyle);*/

			foreach ( DayOfWeek day in Enum.GetValues( typeof(DayOfWeek) ) )
			{
				DGCStyle = new ScheduleDataGridColumnStyle( 
					scehdule.ScheduleDayData( day ) );
				DGStyle.GridColumnStyles.Add(DGCStyle);
			}			
            return DGStyle;
        }		
		/// <summary>
		/// Gets only Schedule object
		/// </summary>
		public new object DataSource
		{
			set
			{
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
				base.DataSource  = mSchedule.DataTable;
			}
		}
		public Schedule Schedule
        {
            get{ return mSchedule; }
        }

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
            private DataGrid.HitTestInfo mHitTestInfo;
            private Schedule             mSchedule;

            public HitTestInfo( ScheduleDataGrid grid, DataGrid.HitTestInfo hitTestinfo, Schedule scedule )
            {
				mGrid        = grid;
                mHitTestInfo = hitTestinfo;
                mSchedule    = scedule;
            }

            /// <summary>
            /// Gets the number of the column the user has clicked.
            /// </summary>
            public int Column
            {
                get { return mHitTestInfo.Column; }
            }
            /// <summary>
            /// Gets the number of the row the user has clicked.
            /// </summary>
            public int Row
            {
                get { return mHitTestInfo.Row; }
            }
            /// <summary>
            /// /// Gets the part of the UDonkey.GUI.ScheduleGrid control, other than the row or column, that was clicked.
            /// </summary>
            public DataGrid.HitTestType Type 
            {
                get{ return mHitTestInfo.Type; }
            }

            /// <summary>
            /// Gets the day of the column the user has clicked.
            /// </summary>
            public DayOfWeek Day
            {
				get
				{
					return (DayOfWeek)Enum.Parse(typeof(DayOfWeek),
						mGrid.TableStyles[ Schedule.DATA_TABLE_NAME ].GridColumnStyles[ this.Column ].HeaderText
						,true); 
				}
            }

            /// <summary>
            /// Gets the hour of the row the user has clicked.
            /// </summary>
            public int Hour
            {
                get{ return this.Row + mSchedule.StartHour; }
            }

            public IScheduleEntry Object
            {
                get{ return mSchedule.FullDataTable.Rows[mHitTestInfo.Row + mSchedule.StartHour][mHitTestInfo.Column] as IScheduleEntry; }
            }
		}
	}
}
