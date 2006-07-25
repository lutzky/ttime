using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for HourColumnScheduleObject.
	/// </summary>
	public class HourColumnScheduleEntry: IScheduleEntry
	{
        private int mnHour;

        public HourColumnScheduleEntry( int hour )
        {
            if ( hour < 0 || hour > 23 )
            {
                throw new ArgumentOutOfRangeException( "hour", hour, "Hour must be between 0-23" );
            }
            mnHour = hour;
        }  
		#region IScheduleEntry
		public void Add( string key, IScheduleObject obj )
		{
			throw new MethodAccessException( "Cannot add IScheduleObject to HourColumnScheduleEntry" );
		}

		public			bool   ContainsKey( string key )
		{
			return false;
		}

		public IScheduleObject this[ string key ] 
		{ 
			get { return null; }
		}
		public void Remove ( string key )
		{
			throw new MethodAccessException( "Cannot remove IScheduleObject from HourColumnScheduleEntry" );
		}
		
		public int  Count       
		{ 
			get{ return 1; }
		}
		public bool IsEmpty
		{
			get{ return false; }
		}

		public bool UserDefined
		{
			get{ return false; }
		}

		public System.Drawing.Color BackColor
		{
			get{ return System.Drawing.Color.Gray; }
		}

		public System.Drawing.Color ForeColor
		{
			get{ return System.Drawing.Color.White; }
		}

		public System.Collections.IDictionary Events
		{
			get { return new System.Collections.Hashtable(); }
		}
        public override string ToString()
        {
            return mnHour + ":30" ;
        }
        public string ToString( VerbosityFlag flag)
        {
            return this.ToString();
        }

		public System.Collections.IEnumerator GetEnumerator()
		{
			return new System.Collections.Hashtable().GetEnumerator();
		}
		#endregion IScheduleEntry
	}
}
