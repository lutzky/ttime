using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for CourseEventOccurrencesScheduleObject.
	/// </summary>
	public class CourseEventOccurrencesScheduleObject: AbstractScheduleObject
	{
		private CourseEventOccurrence mOccurrence;
		private int mnSeed;
		private string msKey;

		public CourseEventOccurrencesScheduleObject( CourseEventOccurrence occurrence )
		{
			if ( occurrence == null )
			{
				throw new ArgumentNullException( "occurrence" );
			}
			mOccurrence = occurrence;
			mnSeed = this.mOccurrence.CourseEvent.Course.Number.GetHashCode();
			this.UserDefined	= false;
			msKey = string.Concat(
				mOccurrence.CourseEvent.Course.Number,
				mOccurrence.CourseEvent.Type,
				mOccurrence.CourseEvent.EventNum);
		}
		#region IScheduleObject
		public override string Key
		{
			get
			{
				return msKey;
			}
		}

		public override DayOfWeek DayOfWeek
		{ 
			get{ return mOccurrence.Day;  }
			set{ mOccurrence.Day = value; }
		}
		public override int StartHour
		{ 
			get{ return mOccurrence.Hour;  }
			set{ mOccurrence.Hour = value; }
		}
		public override int Duration
		{ 
			get{ return mOccurrence.Duration;  }
			set{ mOccurrence.Duration = value; }
		}
		/*public override System.Drawing.Color ForeColor
		{
			get
			{   
				if( base.ForeColor.IsEmpty )
				{
					this.ForeColor = System.Drawing.ColorMixer.ForeGroundColor( mnSeed );
				}
				return base.ForeColor; 
			}
		}
		public override System.Drawing.Color           BackColor 
		{ 
			get
			{   
				if( base.BackColor.IsEmpty )
				{
					this.BackColor = System.Drawing.ColorMixer.BackGroundColor( mnSeed );
				}
				return base.BackColor; 
			}
		}*/
		public override string ToString( VerbosityFlag flag )
		{
			string ret = string.Empty;
			switch (flag)
			{
				case VerbosityFlag.Full:
				{
					return string.Format( "{1} {2}\n{3} {0}{4}{5}\n{6} {7} {8}",
						this.mOccurrence.CourseEvent.EventNum,
						this.mOccurrence.CourseEvent.Course.Number,
						this.mOccurrence.CourseEvent.Course.NickName,
						this.mOccurrence.CourseEvent.Type,
						(this.mOccurrence.CourseEvent.Giver.Length != 0)?":על ידי ":string.Empty,
						this.mOccurrence.CourseEvent.Giver,
						this.mOccurrence.Day,
						this.mOccurrence.Hour + ":30",
						this.mOccurrence.Location );
				}
				case VerbosityFlag.Middle:
				{
					return string.Format( "{1} {2}\n{3} {0}{4}{5}\n{6}",
						this.mOccurrence.CourseEvent.EventNum,
						this.mOccurrence.CourseEvent.Course.Number,
						this.mOccurrence.CourseEvent.Course.NickName,
						this.mOccurrence.CourseEvent.Type,
						(this.mOccurrence.CourseEvent.Giver.Length != 0)?":על ידי ":string.Empty,
						this.mOccurrence.CourseEvent.Giver,
						this.mOccurrence.Location );
				}
				case VerbosityFlag.Minimal:
				{
					return mOccurrence.CourseEvent.Course.NickName; 
				}
			}
			bool tmp = false;
			if( (flag & VerbosityFlag.CourseNumber)==  VerbosityFlag.CourseNumber)
			{
				ret += this.mOccurrence.CourseEvent.Course.Number + " ";
				tmp = true;
			}

			if( (flag & VerbosityFlag.CourseName) == VerbosityFlag.CourseName )
			{
				ret += this.mOccurrence.CourseEvent.Course.NickName;
				tmp = true;
			}
			if (tmp) ret += "\n";
			tmp = false;
			if( (flag & VerbosityFlag.EventType) == VerbosityFlag.EventType )
			{
				ret += this.mOccurrence.CourseEvent.Type + " ";
				tmp = true;
			}
			if( (flag & VerbosityFlag.EventNumber) == VerbosityFlag.EventNumber )
			{
				ret += this.mOccurrence.CourseEvent.EventNum;
				tmp = true;
			}
			if (tmp) ret += "\n";
			tmp = false;
			if( (flag & VerbosityFlag.EventGiver) == VerbosityFlag.EventGiver )
			{
				ret += (this.mOccurrence.CourseEvent.Giver.Length != 0)?":על ידי ":string.Empty;
				if (this.mOccurrence.CourseEvent.Giver.Length != 0) tmp = true;
				ret += this.mOccurrence.CourseEvent.Giver;
			}
			if (tmp) ret += "\n";
			if( (flag & VerbosityFlag.EventLocation) == VerbosityFlag.EventLocation )
			{
				ret += this.mOccurrence.Location + "\n";
			}
			return ret;

		}
		#endregion IScheduleObject
	}
}
