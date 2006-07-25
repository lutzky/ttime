using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for UsersScheduleEvent.
	/// </summary>
	public class UsersEventScheduleObject: AbstractScheduleObject
	{
        private string msEvent;

        public UsersEventScheduleObject( string sEvent, DayOfWeek day, int startHour,  int duration )
        {
            if ( sEvent == null )
            {
                throw new ArgumentNullException( "sEvent" );
            }
            msEvent = sEvent;
            this.StartHour		= startHour;
            this.Duration		= duration;
            this.DayOfWeek		= day;

            this.BackColor		= System.Drawing.Color.BlueViolet;
            this.ForeColor		= System.Drawing.Color.White;
			this.UserDefined	= true;
        }
   
		public override string Key
		{
			get
			{
				return string.Format("{0}{1}{2}",
					this.msEvent,
					this.DayOfWeek,
					this.StartHour);
			}
		}

        public override string ToString( VerbosityFlag flag )
        {
            return msEvent;
        }

		public string Event
		{
			get { return this.msEvent; }
			set { this.msEvent = value; }
		}

	}
}
