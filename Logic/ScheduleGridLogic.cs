using System;
using System.Windows.Forms;
using UDonkey.Logic;
using UDonkey.DB;

namespace UDonkey.GUI
{
    /// <summary>
    /// ScheduleGridLogic controls the SceduleGrid events.
    /// </summary>
    public class ScheduleGridLogic
    {
        private CoursesScheduler             mScheduler;
        private ScheduleDataGrid             mGrid;
        private ScheduleDataGrid.HitTestInfo mHitTestInfo;
        private ScheduleDayData              mDayData;
        private ScheduleMenuItem             mEmptyCellMenuItem;
        private EventHandler                 mFreeDayConstraint;
        private EventHandler                 mStartHourConstraint;
        private EventHandler                 mEndHourConstraint;
        private EventHandler                 mHideDay;

        public ScheduleGridLogic( CoursesScheduler scheduler, ScheduleDataGrid grid )
        {
			if ( scheduler != null )
			{
				mScheduler = scheduler;
			}
			else
			{
				mScheduler = new CoursesScheduler();
			}
			mScheduler.ScheduleChanged += new EventHandler(mScheduler_ScheduleChanged);
            mGrid = grid;
			mGrid.DataSource     = Scheduler.Schedule;
            mGrid.MouseDown     += new MouseEventHandler( this.OnScheduleGridMouseDown );
			mGrid.MouseMove     += new MouseEventHandler(OnScheduleGridMouseMove); 
            mEmptyCellMenuItem   = new ScheduleMenuItem( null, "  ", new EventHandler( NewUsersEvent ) );
            mFreeDayConstraint   = new EventHandler( FreeDayConstraint );
            mStartHourConstraint = new EventHandler( StartHourConstraint );
            mEndHourConstraint   = new EventHandler( EndHourConstraint );
            mHideDay			 = new EventHandler( HideDay );
        }

        /// <summary>
        /// Called after the ScheduleGrid's OnMouseDown event and add items to 
        /// the ScheduleGrid's ContextMenu
        /// </summary>
        /// <param name="sender">The ScheduleGrid that raised this event</param>
        /// <param name="args">MouseEventArgs of this event</param>
		public void OnScheduleGridMouseDown( object sender, MouseEventArgs args )
		{
			ScheduleMenuItem menuItem;
			mHitTestInfo = mGrid.HitTest( args.X, args.Y );
			if ( mHitTestInfo.Type == DataGrid.HitTestType.Cell ||
				mHitTestInfo.Type == DataGrid.HitTestType.ColumnHeader )
			{
				mDayData = 
					(ScheduleDayData)this.mScheduler.Schedule.
					DaysData[ mHitTestInfo.Day ];
			}

			if ( mHitTestInfo.Type == DataGrid.HitTestType.Cell ) 
			{ 
				IScheduleEntry entry = mHitTestInfo.Object;
				if ( entry.IsEmpty )
				{//Empty Cell
					mGrid.ContextMenu.MenuItems.Add( mEmptyCellMenuItem );                    
				}
				else
				{//IScheduleEntry entry is not empty
					if( entry.UserDefined )
					{
						mGrid.ContextMenu.MenuItems.Add( mEmptyCellMenuItem );
					}
					foreach ( System.Collections.DictionaryEntry e in entry )
					{
						IScheduleObject obj = (IScheduleObject) e.Value;
						foreach ( System.Collections.DictionaryEntry dicEntry in obj.Events )
						{			
							menuItem = new ScheduleMenuItem( 
								obj, 
								dicEntry.Key as string,
								dicEntry.Value as EventHandler );
							mGrid.ContextMenu.MenuItems.Add( menuItem );
						}
					}
				}
				if( mHitTestInfo.Day != DayOfWeek.Saturday) // FIXME arbitrary date
				{
					if( mHitTestInfo.Hour < mDayData.EndHourConstraint )
					{
						menuItem = new ScheduleMenuItem( 
							null, 
							string.Format("  {0}  ", mHitTestInfo.Hour +":30" ),
							mStartHourConstraint);
						mGrid.ContextMenu.MenuItems.Add( menuItem );                                
					}
					if( mHitTestInfo.Hour > mDayData.StartHourConstraint )
					{
						menuItem = new ScheduleMenuItem( 
							null, 
							string.Format("  {0}  ", mHitTestInfo.Hour +":30" ),
							mEndHourConstraint);
						mGrid.ContextMenu.MenuItems.Add( menuItem ); 
					}
				}
			}
			if ( mHitTestInfo.Type == DataGrid.HitTestType.Cell ||
				mHitTestInfo.Type == DataGrid.HitTestType.ColumnHeader )
			{
				if ( mHitTestInfo.Day != DayOfWeek.Saturday) // FIXME arbitrary
				{
					menuItem = new ScheduleMenuItem( 
						null, 
						string.Format("{0}  {1}  ",
						mDayData.FreeDayConstraint?"":"",
						mHitTestInfo.Day),
						mFreeDayConstraint);
					mGrid.ContextMenu.MenuItems.Add( menuItem ); 
				}
			}
				
			if ( mHitTestInfo.Type == DataGrid.HitTestType.ColumnHeader )
			{//Only on ColumnHeaders

				menuItem = new ScheduleMenuItem( 
					null, 
					string.Format("   {0}",
					mHitTestInfo.Day),
					mHideDay);
				mGrid.ContextMenu.MenuItems.Add( menuItem ); 
			}
		}
		/// <summary>
		/// Called after ScheduleGrid's OnMouseHover event and show the hover text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScheduleGridMouseMove(object sender, MouseEventArgs args)
		{
			mHitTestInfo = mGrid.HitTest( args.X, args.Y );
			if ( mHitTestInfo.Type == DataGrid.HitTestType.Cell ) 
			{
				mGrid.HoverString = mHitTestInfo.Object.ToString( VerbosityFlag.Full );
			}			
		}
        public void NewUsersEvent( object obj , EventArgs args )
        {
            string      eventName;
            DayOfWeek   day;
            int         hour;
            int         duration;

            if( mGrid.GetUsersEventParameters( out eventName, out day, out hour, out duration, true ) )
            {
				//TODO:
                this.mScheduler.AddUserEvent( eventName, day, hour, duration ); 
                this.mGrid.Refresh();
            }
            return;
        }
        public void FreeDayConstraint( object obj , EventArgs args )
        {
            mDayData.FreeDayConstraint = !mDayData.FreeDayConstraint;
            mGrid.Parent.Refresh();
        }
        public void StartHourConstraint( object obj , EventArgs args )
        {
            mDayData.StartHourConstraint = mHitTestInfo.Hour;
            mGrid.Parent.Refresh();
        }
        public void EndHourConstraint( object obj , EventArgs args )
        {
            mDayData.EndHourConstraint = mHitTestInfo.Hour;
            mGrid.Parent.Refresh();
        }
        public void HideDay( object obj , EventArgs args )
        {
            this.mGrid.HideColumn( mHitTestInfo.Day.ToString() );
        }
        public void Refresh()
        {
            this.mGrid.DataSource = mScheduler.Schedule;
        }
        public CoursesScheduler Scheduler
        {
            get { return mScheduler; }
		}

		private void mScheduler_ScheduleChanged(object sender, EventArgs e)
		{
			this.mGrid.Refresh();
		}
	}
}
