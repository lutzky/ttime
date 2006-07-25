using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ScheduleObjectCollection.
	/// </summary>
	public class ScheduleEntryBucket: IScheduleEntry	
	{
		private Schedule            mSchedule;
		private System.Collections.SortedList mcList;
		private bool 				mbUserDefined;

		public ScheduleEntryBucket( Schedule schedule )
		{
			mSchedule = schedule;
			mcList	  = new System.Collections.SortedList();
		}

		#region IScheduleObjectsBucket
		
		public			void   Add( string key, IScheduleObject obj )
		{
			if( this.IsEmpty  )
			{
				mbUserDefined = obj.UserDefined;
			}
			mcList.Add( key, obj );
		}

		public			bool   ContainsKey( string key )
		{
			return mcList.ContainsKey( key );
		}

		public			IScheduleObject this [string key]
		{
			get
			{
				return (IScheduleObject)mcList[ key ];
			}
		}
		public			void   Remove( string key )
		{
			mcList.Remove( key );
		}

		public override string ToString()
		{
			string ret = string.Empty;
			int count  = this.Count;
			VerbosityFlag flag = ( count > 1 )? 
				VerbosityFlag.Minimal: VerbosityFlag.Middle;

			foreach ( IScheduleObject obj in mcList.Values )
			{
				if( ret.Length == 0 )
				{
					ret = obj.ToString( flag );
				}
				else
				{
					ret = string.Concat( ret, "\n", obj.ToString( flag ) );
				}
			}
			return ret;
		}

        public string ToString( VerbosityFlag flag )
        {
            string ret = string.Empty;
            foreach ( IScheduleObject obj in mcList.Values )
            {
                if( ret.Length == 0 )
                {
                    ret = obj.ToString( flag );
                }
                else
                {
                    ret = string.Concat( ret, "\n==============\n", obj.ToString( flag ) );
                }
            }
            return ret;
        }

		public System.Collections.IEnumerator GetEnumerator()
		{
			return mcList.GetEnumerator();
		}
		public bool     IsEmpty
		{
			get
			{
				return mcList.Count == 0;
			}
		}

		public int  Count      
		{ 
			get{ return mcList.Count; }
		}
		public bool UserDefined 
		{ 
			get{ return mbUserDefined; }
		}
		public Schedule Schedule
		{ 
			get{ return mSchedule;  }
		}

		public System.Collections.IDictionary Events
		{ 
			get
			{ 
				System.Collections.Hashtable events = new System.Collections.Hashtable();
				foreach ( IScheduleObject obj in mcList.Values )
				{
					foreach ( System.Collections.DictionaryEntry entry in obj.Events )
					{
						events.Add( entry.Key, entry.Value );
					}
				}
				return events; 
			} 
		}
		public System.Drawing.Color           ForeColor
		{
			get
			{
				switch ( mcList.Count )
				{
					case 0: return System.Drawing.Color.Black;
					case 1: 
					{
						IScheduleObject obj = mcList.GetValueList()[0] as IScheduleObject;
						return obj.ForeColor;
					}
					default: return System.Drawing.Color.White;
				}
			}		
		}
		public System.Drawing.Color           BackColor 
		{
			get
			{
				switch ( mcList.Count )
				{
					case 0: return System.Drawing.Color.White;
					case 1: 
					{
						IScheduleObject obj = mcList.GetValueList()[0] as IScheduleObject;
						return obj.BackColor;
					}
					default: return System.Drawing.Color.Red;
				}
			}		
		}


		#endregion IScheduleObject
	}
}