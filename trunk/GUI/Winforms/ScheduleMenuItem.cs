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
	/// ScheduleMenuItem for ScheduleGrid ContextMenu.
	/// </summary>
	public class ScheduleMenuItem : System.Windows.Forms.MenuItem 
	{
		private IScheduleObject mObject;
		private EventHandler mOnClick;

		#region Constructors
		public ScheduleMenuItem(IScheduleObject obj)
			:this( obj, string.Empty, null ){}

		public ScheduleMenuItem(IScheduleObject obj, string text)
			:this( obj, text, null ){}

		public  ScheduleMenuItem (IScheduleObject obj, string text, EventHandler onClick)
			:base( text )
		{
			mObject = obj;
			mOnClick = onClick;
		}
		#endregion Constructors
		#region Properties
		public IScheduleObject ScheduleObject
		{
			get{ return mObject; }
		}
		#endregion Properties

		protected override void OnClick(EventArgs args)
		{
			if (mOnClick != null)
			{
				mOnClick(mObject, new ScheduleEventArgs(Text));
			}
		}
	}
}
