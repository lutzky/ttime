using System;
// TODO don't depend on windows forms
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
            mGrid.MouseDown     += new GridMouseEventHandler( this.OnScheduleGridMouseDown );
			mGrid.MouseMove     += new GridMouseEventHandler(OnScheduleGridMouseMove); 
            mEmptyCellMenuItem   = new ScheduleMenuItem( null, "צור אירוע משתמש", new EventHandler( NewUsersEvent ) );
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
		public void OnScheduleGridMouseDown( object sender, GridMouseEventArgs args )
		{
			ScheduleMenuItem menuItem;
			mHitTestInfo = args.HitTestInfo;
			if ( mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.Cell ||
				mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.ColumnHeader )
			{
				mDayData = 
					(ScheduleDayData)this.mScheduler.Schedule.
					DaysData[ mHitTestInfo.Day ];
			}

			if ( mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.Cell ) 
			{ 
				IScheduleEntry entry = mHitTestInfo.Object;
				if ( entry.IsEmpty )
				{//Empty Cell
					mGrid.AddContextMenuItem( mEmptyCellMenuItem );                    
				}
				else
				{//IScheduleEntry entry is not empty
					if( entry.UserDefined )
					{
						mGrid.AddContextMenuItem( mEmptyCellMenuItem );
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
							mGrid.AddContextMenuItem( menuItem );
						}
					}
				}
				if( mHitTestInfo.Day != DayOfWeek.שעות )
				{
					if( mHitTestInfo.Hour < mDayData.EndHourConstraint )
					{
						menuItem = new ScheduleMenuItem( 
							null, 
							string.Format("הגדר את {0} כשעת התחלה", mHitTestInfo.Hour +":30" ),
							mStartHourConstraint);
						mGrid.AddContextMenuItem( menuItem );                                
					}
					if( mHitTestInfo.Hour > mDayData.StartHourConstraint )
					{
						menuItem = new ScheduleMenuItem( 
							null, 
							string.Format("הגדר את {0} כשעת סיום", mHitTestInfo.Hour +":30" ),
							mEndHourConstraint);
						mGrid.AddContextMenuItem( menuItem ); 
					}
				}
			}
			if ( mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.Cell ||
				mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.ColumnHeader )
			{
				if ( mHitTestInfo.Day != DayOfWeek.שעות )
				{
					menuItem = new ScheduleMenuItem( 
						null, 
						string.Format("{0} יום {1} כיום חופשי",
						mDayData.FreeDayConstraint?"הסר":"הגדר",
						mHitTestInfo.Day),
						mFreeDayConstraint);
					mGrid.AddContextMenuItem( menuItem ); 
				}
			}
				
			if ( mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.ColumnHeader )
			{//Only on ColumnHeaders

				menuItem = new ScheduleMenuItem( 
					null, 
					string.Format("הסתר את יום {0}",
					mHitTestInfo.Day),
					mHideDay);
				mGrid.AddContextMenuItem( menuItem ); 
			}
		}
		/// <summary>
		/// Called after ScheduleGrid's OnMouseHover event and show the hover text
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScheduleGridMouseMove(object sender, GridMouseEventArgs args)
		{
			mHitTestInfo = args.HitTestInfo; 
			if ( mHitTestInfo.Type == ScheduleDataGrid.HitTestInfo.HitTestType.Cell ) 
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
            mGrid.ParentRefresh();
        }
        public void StartHourConstraint( object obj , EventArgs args )
        {
            mDayData.StartHourConstraint = mHitTestInfo.Hour;
            mGrid.ParentRefresh();
        }
        public void EndHourConstraint( object obj , EventArgs args )
        {
            mDayData.EndHourConstraint = mHitTestInfo.Hour;
            mGrid.ParentRefresh();
        }
        public void HideDay( object obj , EventArgs args )
        {
            this.mGrid.HideColumn( mHitTestInfo.Day.ToString() );
        }
        public void Refresh()
        {
            Console.WriteLine("ScheduleGridLogic.Refresh");
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
