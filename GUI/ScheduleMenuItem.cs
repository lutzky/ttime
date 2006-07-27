using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace UDonkey.GUI
{
	/// <summary>
	/// ScheduleMenuItem for ScheduleGrid ContextMenu.
	/// </summary>
	public class ScheduleMenuItem : System.Windows.Forms.MenuItem 
	{
		private IScheduleObject mObject;

		#region Constructros
		public ScheduleMenuItem(IScheduleObject obj)
			:this( obj, string.Empty, null ){}

		public ScheduleMenuItem(IScheduleObject obj, string text)
			:this( obj, text, null ){}

		public  ScheduleMenuItem (IScheduleObject obj, string text, EventHandler onClick)
			:base( text, onClick )
		{
			mObject = obj;
		}
		#endregion Constructros
		#region Properties
		public IScheduleObject ScheduleObject
		{
			get{ return mObject; }
		}
		#endregion Properties
	}
}
