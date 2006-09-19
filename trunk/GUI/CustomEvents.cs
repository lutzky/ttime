using System;

namespace UDonkey.GUI
{
	
	public delegate void SearchEventHandler(object sender, SearchEventArgs args);
		
	public class SearchEventArgs: EventArgs
	{
		public string Name   = null;
		public string Number = null;
		public string Points = null;
		public string Faculty      = null;
		public string Lecturer     = null;
		public string[] Days = new string[6];
	}

	public delegate void GridMouseEventHandler(object sender, GridMouseEventArgs args);

	public class GridMouseEventArgs : EventArgs
	{
		public ScheduleDataGrid.HitTestInfo HitTestInfo;
	}

}
