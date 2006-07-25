using System;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for ImportedScheduleObject.
	/// </summary>
	public class ImportedScheduleObject: AbstractScheduleObject
	{
		private string msKey;
		private System.Collections.Hashtable mVerbosityStrings;
		public ImportedScheduleObject( string key )
		{
			this.Duration = 1;
			msKey = key;
			mVerbosityStrings = new System.Collections.Hashtable();
		}
		#region AbstractScheduleObject
		public override string Key
		{
			get { return msKey; }
		}
		public override string ToString( VerbosityFlag flag )
		{
			string ret = mVerbosityStrings[ flag ] as string;
			return ( ret == null )? string.Empty: ret;
		}
		#endregion AbstractScheduleObject
		public System.Collections.IDictionary VerbosityStrings
		{
			get{ return mVerbosityStrings; }
		}
	}
}
