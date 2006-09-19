//
using System;
using Gtk;
using GtkSharp;
using Glade;

namespace UDonkey.GUI
{
	public class ConfigControl 
	{
#region Glade Widgets
		[Widget] CheckButton chbCollisions;
		[Widget] SpinButton tbMaxCollisions;
		[Widget] CheckButton chbRegGroups;
		[Widget] CheckButton chbTestAlert;
		[Widget] SpinButton tbMinTestDays;

		[Widget] CheckButton chbSunday;
		[Widget] CheckButton chbMonday;
		[Widget] CheckButton chbTuesday;
		[Widget] CheckButton chbWednesday;
		[Widget] CheckButton chbThursday;
		[Widget] CheckButton chbFriday;
		[Widget] CheckButton chbSaturday;
		[Widget] ComboBoxEntry cbStartHour;
		[Widget] ComboBoxEntry cbEndHour;
		
		[Widget] CheckButton chbExportName;
		[Widget] CheckButton chbExportRegNum;
		[Widget] CheckButton chbExportType;
		[Widget] CheckButton chbExportTeacher;
		[Widget] CheckButton chbExportLocation;
		[Widget] CheckButton chbExportNumber;

		[Widget] VScale tbStartDay;
		[Widget] VScale tbEndDay;
		[Widget] VScale tbFreeDays;
		[Widget] VScale tbHoles;

		[Widget] Button btAnt;
		[Widget] Button btDontCare;
		[Widget] Button btPadlaa;

		[Widget] SpinButton tbMinFreeDays;
		[Widget] SpinButton tbMinDailyHours;
		[Widget] SpinButton tbMaxDailyHours;

		[Widget] CheckButton chbFreeSunday;
		[Widget] CheckButton chbFreeMonday;
		[Widget] CheckButton chbFreeTuesday;
		[Widget] CheckButton chbFreeWednesday;
		[Widget] CheckButton chbFreeThursday;

		[Widget] ComboBoxEntry cbSundayStart;
		[Widget] ComboBoxEntry cbSundayEnd;
		[Widget] ComboBoxEntry cbMondayStart;
		[Widget] ComboBoxEntry cbMondayEnd;
		[Widget] ComboBoxEntry cbTuesdayStart;
		[Widget] ComboBoxEntry cbTuesdayEnd;
		[Widget] ComboBoxEntry cbWednesdayStart;
		[Widget] ComboBoxEntry cbWednesdayEnd;
		[Widget] ComboBoxEntry cbThursdayStart;
		[Widget] ComboBoxEntry cbThursdayEnd;

		[Widget] Button btDefaults;
		[Widget] Button btCancel;
		[Widget] Button btSaveChanges;
#endregion

		public ConfigControl()
		{
			Glade.XML gxml = new Glade.XML(null, "udonkey.glade", "ConfigControl", null);
			//Glade.XML gxml = Glade.XML.FromAssembly("udonkey.glade", "ConfigControl", null);
			gxml.Autoconnect (this);
		}

#region Events
		public void on_TimeCombo_changed(object o, EventArgs args)
		{
			Console.WriteLine("Test");
		}
    
		protected void on_btDefaults_clicked(object o, EventArgs args)
		{
			Console.WriteLine("Test:::" + StartHour);
		}

		protected void on_chbCollisions_toggled(object o, EventArgs args)
		{
			AllowCollisions = chbCollisions.Active;
		}
		
		protected void on_chbTestAlert_toggled(object o, EventArgs args)
		{
			TestAlert = chbTestAlert.Active;
		}
		
		protected void on_btAnt_clicked(object o, EventArgs args)
		{
			PrefStartHour = 
			PrefEndHour = 
			PrefFreeDays = 
			PrefHoles = -5;
		}
		
		protected void on_btPadlaa_clicked(object o, EventArgs args)
		{
			PrefStartHour = 
			PrefEndHour = 
			PrefFreeDays = 
			PrefHoles = 5;
		}

		protected void on_btDontCare_clicked(object o, EventArgs args)
		{
			PrefStartHour = 
			PrefEndHour = 
			PrefFreeDays = 
			PrefHoles = 0;
		}
		
		protected void on_chbFreeSunday_toggled(object o, EventArgs args)
		{
			FreeSunday = chbFreeSunday.Active;
		}

		protected void on_chbFreeMonday_toggled(object o, EventArgs args)
		{
			FreeMonday = chbFreeMonday.Active;
		}

		protected void on_chbFreeTuesday_toggled(object o, EventArgs args)
		{
			FreeTuesday = chbFreeTuesday.Active;
		}

		protected void on_chbFreeWednesday_toggled(object o, EventArgs args)
		{
			FreeWednesday = chbFreeWednesday.Active;
		}

		protected void on_chbFreeThursday_toggled(object o, EventArgs args)
		{
			FreeThursday = chbFreeThursday.Active;
		}
		
		protected void on_scale_value_changed(object o, EventArgs args)
		{
			VScale scale = o as VScale;
			scale.Value = (int)scale.Value;
		}

		protected void on_focus(object o, FocusedArgs args)
		{
			if (mVisibilityNotifyEvent != null) 
				mVisibilityNotifyEvent(o, args);
		}
#endregion

#region Properties
		public bool AllowCollisions
		{
			get {return chbCollisions.Active;}
			set 
			{
				chbCollisions.Active = value;
				if (value==true)
				{
					tbMaxCollisions.Sensitive=true;
				}
				else
				{
					tbMaxCollisions.Sensitive=false;
				}
			}
		}
		public int MaxCollisions
		{
			get 
			{
				try
				{
					return int.Parse(tbMaxCollisions.Text);
				}
				catch
				{
					tbMaxCollisions.Text="0";
					return 0;
				}
			}
			set { tbMaxCollisions.Text = value.ToString();}
		}

		public bool AllowRegSplit
		{
			get {return chbRegGroups.Active;}
			set {chbRegGroups.Active = value;}
		}
		public bool TestAlert
		{
			get {return chbTestAlert.Active;}
			set 
			{
				chbTestAlert.Active = value;
				if (value==true)
				{
					tbMinTestDays.Sensitive=true;
				}
				else
				{
					tbMinTestDays.Sensitive=false;
				}
			}
		}
		public int TestInterval
		{
			get 
			{
				try
				{
					return int.Parse(tbMinTestDays.Text);
				}
				catch
				{
					tbMinTestDays.Text="1";
					return 1;
				}
		 	}
			set { tbMinTestDays.Text = value.ToString();}
		}


		public bool Sunday
		{
			get {return chbSunday.Active;}
			set 
			{
				chbSunday.Active = value;
			}
		}
		public bool Monday
		{
			get {return chbMonday.Active;}
			set {chbMonday.Active = value;}
		}
		public bool Tuesday
		{
			get {return chbTuesday.Active;}
			set {chbTuesday.Active = value;}
		}
		public bool Wednesday
		{
			get {return chbWednesday.Active;}
			set {chbWednesday.Active = value;}
		}
		public bool Thursday
		{
			get {return chbThursday.Active;}
			set {chbThursday.Active = value;}
		}
		public bool Friday
		{
			get {return chbFriday.Active;}
			set {chbFriday.Active = value;}
		}
		public bool Saturday
		{
			get {return chbSaturday.Active;}
			set {chbSaturday.Active = value;}
		}
		public string StartHour
		{
			get 
			{
				string hour = cbStartHour.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbStartHour.Entry.Text = "8:30";
					return;
				}

				if (cbEndHour.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbEndHour.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbEndHour.Entry.Text=value + ":30";
					}
				}
				cbStartHour.Entry.Text = value + ":30";
			}
		}
		public string EndHour
		{
			get 
			{
				string hour = cbEndHour.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbEndHour.Entry.Text="20:30";
					return;
				}

				if (cbStartHour.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbStartHour.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbStartHour.Entry.Text=value + ":30";
					}
				}
				cbEndHour.Entry.Text = value + ":30";
			}
		}


		public int PrefStartHour
		{
			get {return (int)tbStartDay.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbStartDay.Value=value;
			}
		}
		public int PrefEndHour
		{
			get {return (int)tbEndDay.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbEndDay.Value=value;
			}
		}
		public int PrefFreeDays
		{
			get {return (int)tbFreeDays.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbFreeDays.Value=value;
			}
		}
		public int PrefHoles
		{
			get {return (int)tbHoles.Value;}
			set 
			{
				if (value>=-5 && value<=5)
					tbHoles.Value=value;
			}
		}


		public bool ExportName
		{
			get {return chbExportName.Active;}
			set {chbExportName.Active = value;}
		}
		public bool ExportNumber
		{
			get {return chbExportNumber.Active;}
			set {chbExportNumber.Active = value;}
		}
		public bool ExportType
		{
			get {return chbExportType.Active;}
			set {chbExportType.Active = value;}
		}
		public bool ExportRegNum
		{
			get {return chbExportRegNum.Active;}
			set {chbExportRegNum.Active = value;}
		}
		public bool ExportLocation
		{
			get {return chbExportLocation.Active;}
			set {chbExportLocation.Active = value;}
		}
		public bool ExportTeacher
		{
			get {return chbExportTeacher.Active;}
			set {chbExportTeacher.Active = value;}
		}


		public bool FreeSunday
		{
			get {return chbFreeSunday.Active;}
			set 
			{
				chbFreeSunday.Active = value;
				if (value==true)
				{
					cbSundayStart.Sensitive=false;
					cbSundayEnd.Sensitive=false;
				}
				else
				{
					cbSundayStart.Sensitive=true;
					cbSundayEnd.Sensitive=true;
				}
			}
		}
		public bool FreeMonday
		{
			get {return chbFreeMonday.Active;}
			set 
			{
				chbFreeMonday.Active = value;
				if (value==true)
				{
					cbMondayStart.Sensitive=false;
					cbMondayEnd.Sensitive=false;
				}
				else
				{
					cbMondayStart.Sensitive=true;
					cbMondayEnd.Sensitive=true;
					
				}
			}
		}
		public bool FreeTuesday
		{
			get {return chbFreeTuesday.Active;}
			set 
			{
				chbFreeTuesday.Active = value;
				if (value==true)
				{
					cbTuesdayStart.Sensitive=false;
					cbTuesdayEnd.Sensitive=false;
				}
				else
				{
					cbTuesdayStart.Sensitive=true;
					cbTuesdayEnd.Sensitive=true;
				}
			}
		}
		public bool FreeWednesday
		{
			get {return chbFreeWednesday.Active;}
			set 
			{
				chbFreeWednesday.Active = value;
				if (value==true)
				{
					cbWednesdayStart.Sensitive=false;
					cbWednesdayEnd.Sensitive=false;
				}
				else
				{
					cbWednesdayStart.Sensitive=true;
					cbWednesdayEnd.Sensitive=true;
				}
			}
		}
		public bool FreeThursday
		{
			get {return chbFreeThursday.Active;}
			set 
			{
				chbFreeThursday.Active = value;
				if (value==true)
				{
					cbThursdayStart.Sensitive=false;
					cbThursdayEnd.Sensitive=false;
				}
				else
				{
					cbThursdayStart.Sensitive=true;
					cbThursdayEnd.Sensitive=true;
				}
			}
		}

		
		public string SundayStartHour
		{
			get 
			{
				string hour = cbSundayStart.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbSundayStart.Entry.Text="8:30";
					return;
				}

				if (cbSundayEnd.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbSundayEnd.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbSundayEnd.Entry.Text=value + ":30";
					}
				}
				cbSundayStart.Entry.Text = value + ":30";
			}
		}
		public string SundayEndHour
		{
			get 
			{
				string hour = cbSundayEnd.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbSundayEnd.Entry.Text="20:30";
					return;
				}

				if (cbSundayStart.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbSundayStart.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbSundayStart.Entry.Text=value + ":30";
					}
				}
				cbSundayEnd.Entry.Text = value + ":30";
			}
		}


		public string MondayStartHour
		{
			get 
			{
				string hour = cbMondayStart.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbMondayStart.Entry.Text="8:30";
					return;
				}

				if (cbMondayEnd.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbMondayEnd.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbMondayEnd.Entry.Text=value + ":30";
					}
				}
				cbMondayStart.Entry.Text = value + ":30";
			}
		}
		public string MondayEndHour
		{
			get 
			{
				string hour = cbMondayEnd.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbMondayEnd.Entry.Text="20:30";
					return;
				}

				if (cbMondayStart.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbMondayStart.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbMondayStart.Entry.Text=value + ":30";
					}
				}
				cbMondayEnd.Entry.Text = value + ":30";
			}
		}


		public string TuesdayStartHour
		{
			get 
			{
				string hour = cbTuesdayStart.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbTuesdayStart.Entry.Text="8:30";
					return;
				}

				if (cbTuesdayEnd.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbTuesdayEnd.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbTuesdayEnd.Entry.Text=value + ":30";
					}
				}
				cbTuesdayStart.Entry.Text = value + ":30";
			}
		}
		public string TuesdayEndHour
		{
			get 
			{
				string hour = cbTuesdayEnd.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbTuesdayEnd.Entry.Text="20:30";
					return;
				}

				if (cbTuesdayStart.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbTuesdayStart.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbTuesdayStart.Entry.Text=value + ":30";
					}
				}
				cbTuesdayEnd.Entry.Text = value + ":30";
			}
		}


		public string WednesdayStartHour
		{
			get 
			{
				string hour = cbWednesdayStart.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbWednesdayStart.Entry.Text="8:30";
					return;
				}

				if (cbWednesdayEnd.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbWednesdayEnd.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbWednesdayEnd.Entry.Text=value + ":30";
					}
				}
				cbWednesdayStart.Entry.Text = value + ":30";
			}
		}
		public string WednesdayEndHour
		{
			get 
			{
				string hour = cbWednesdayEnd.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbWednesdayEnd.Entry.Text="20:30";
					return;
				}

				if (cbWednesdayStart.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbWednesdayStart.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbWednesdayStart.Entry.Text=value + ":30";
					}
				}
				cbWednesdayEnd.Entry.Text = value + ":30";
			}
		}


		public string ThursdayStartHour
		{
			get 
			{
				string hour = cbThursdayStart.Entry.Text;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbThursdayStart.Entry.Text="8:30";
					return;
				}

				if (cbThursdayEnd.Entry.Text!=null)
				{
					int starthour = int.Parse(value);
					string[] hours = ((string)(cbThursdayEnd.Entry.Text)).Split(':');
					int endhour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbThursdayEnd.Entry.Text=value + ":30";
					}
				}
				cbThursdayStart.Entry.Text = value + ":30";
			}
		}
		public string ThursdayEndHour
		{
			get 
			{
				string hour = cbThursdayEnd.Entry.Text ;
				string[] results = hour.Split(':');
				return results[0];
			}
			set 
			{
				if (value!="0" &&
					value!="1" &&
					value!="2" &&
					value!="3" &&
					value!="4" &&
					value!="5" &&
					value!="6" &&
					value!="7" &&
					value!="8" &&
					value!="9" &&
					value!="10" &&
					value!="11" &&
					value!="12" &&
					value!="13" &&
					value!="14" &&
					value!="15" &&
					value!="16" &&
					value!="17" &&
					value!="18" &&
					value!="19" &&
					value!="20" &&
					value!="21" &&
					value!="22" &&
					value!="23")
				{
					cbThursdayEnd.Entry.Text="20:30";
					return;
				}

				if (cbThursdayStart.Entry.Text!=null)
				{
					int endhour = int.Parse(value);
					string[] hours = ((string)cbThursdayStart.Entry.Text).Split(':');
					int starthour = int.Parse(hours[0]);
					if (endhour<starthour) // Can't end day before it starts...
					{
						cbThursdayStart.Entry.Text=value + ":30";
					}
				}
				cbThursdayEnd.Entry.Text = value + ":30";
			}
		}

		
		public int MaxDailyHours
		{
			get 
			{
				try
				{
					return int.Parse(tbMaxDailyHours.Text);
				}
				catch
				{
					tbMaxDailyHours.Text="0";
					return 0;
				}
			}
			set { tbMaxDailyHours.Text = value.ToString();}
		}

		public int MinDailyHours
		{
			get 
			{
				try
				{
					return int.Parse(tbMinDailyHours.Text);
				}
				catch
				{
					tbMinDailyHours.Text="0";
					return 0;
				}
			}
			set { tbMinDailyHours.Text = value.ToString();}
		}

		public int MinFreeDays
		{
			get 
			{
				try
				{
					return int.Parse(tbMinFreeDays.Text);
				}
				catch
				{
					tbMinFreeDays.Text="0";
					return 0;
				}
			}
			set { tbMinFreeDays.Text = value.ToString();}
		}

		public bool SavedLabelVisible;

#endregion
		
#region Events
		public event System.EventHandler Save { 
			add { btSaveChanges.Clicked += value; } 
			remove { btSaveChanges.Clicked -= value; }
		}

		private System.EventHandler mVisibilityNotifyEvent;
		public event System.EventHandler VisibleChanged {
			add { mVisibilityNotifyEvent += value; }
			remove { mVisibilityNotifyEvent -= value; }
		}
#endregion

/*		public static void Main()
		{
			Application.Init();
			new ConfigControl();
			Application.Run();
		}*/
	}
}
