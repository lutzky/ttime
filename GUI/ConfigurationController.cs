using System;
using System.Xml;
using System.IO;
using UDonkey.Logic;
using System.Collections;
using System.Collections.Specialized;

namespace UDonkey.GUI
{
	public delegate void ConfigurationChangedHandler( ConfigurationController sender, StringCollection changes );
	/// <summary>
	/// Summary description for ConfigurationController.
	/// </summary>
	public class ConfigurationController
	{
		private ConfigControl mConfigControl;
		private event ConfigurationChangedHandler mConfigurationChanged;

		public ConfigurationController(ConfigControl configControl)
		{
			mConfigControl=configControl;		
			this.UpdateHash();
			configControl.Save.Click += new System.EventHandler(this.btSaveChanges_Click);
			configControl.VisibleChanged += new EventHandler(configControl_VisibleChanged);
		}

		public bool Load()
		{
            if (Configuration.Load())
            {
                this.UpdateGUI();
                this.UpdateHash();
                return true;
            }
            return false;
		}
		private void btSaveChanges_Click(object sender, System.EventArgs e)
		{
			this.UpdateHash();
			Configuration.SaveToXml();
			mConfigControl.SavedLabel.Visible = true;
			new System.Threading.Thread( new System.Threading.ThreadStart( this.HideLabel )).Start();
		}

		private void UpdateHash()
		{
			Configuration.Set( "General","AllowCollisions", mConfigControl.AllowCollisions );
			Configuration.Set( "General","MaxCollisions", mConfigControl.MaxCollisions);
			Configuration.Set( "General","AllowRegSplit", mConfigControl.AllowRegSplit);
			Configuration.Set( "General","TestAlert", mConfigControl.TestAlert);
			Configuration.Set( "General","TestInterval", mConfigControl.TestInterval);
			Configuration.Set( "Display","Sunday", mConfigControl.Sunday);
			Configuration.Set( "Display","Monday", mConfigControl.Monday);
			Configuration.Set( "Display","Tuesday", mConfigControl.Tuesday);
			Configuration.Set( "Display","Wednesday", mConfigControl.Wednesday);
			Configuration.Set( "Display","Thursday", mConfigControl.Thursday);
			Configuration.Set( "Display","Friday", mConfigControl.Friday);
			Configuration.Set( "Display","Saturday", mConfigControl.Saturday);
			Configuration.Set( "Display","StartHour", mConfigControl.StartHour);
			Configuration.Set( "Display","EndHour", mConfigControl.EndHour);
			Configuration.Set( "Pref","PrefStartHour", mConfigControl.PrefStartHour);
			Configuration.Set( "Pref","PrefEndHour", mConfigControl.PrefEndHour);
			Configuration.Set( "Pref","PrefFreeDays", mConfigControl.PrefFreeDays);
			Configuration.Set( "Pref","PrefHoles", mConfigControl.PrefHoles);
			Configuration.Set( "Export","ExportName", mConfigControl.ExportName);
			Configuration.Set( "Export","ExportNumber", mConfigControl.ExportNumber);
			Configuration.Set( "Export","ExportType", mConfigControl.ExportType);
			Configuration.Set( "Export","ExportRegNum", mConfigControl.ExportRegNum);
			Configuration.Set( "Export","ExportLocation", mConfigControl.ExportLocation);
			Configuration.Set( "Export","ExportTeacher", mConfigControl.ExportTeacher);
			Configuration.Set( @"Schedule\Sunday","Free", mConfigControl.FreeSunday);
			Configuration.Set( @"Schedule\Sunday","StartHour", mConfigControl.SundayStartHour);
			Configuration.Set( @"Schedule\Sunday","EndHour", mConfigControl.SundayEndHour);
			Configuration.Set( @"Schedule\Monday","Free", mConfigControl.FreeMonday);
			Configuration.Set( @"Schedule\Monday","StartHour", mConfigControl.MondayStartHour);
			Configuration.Set( @"Schedule\Monday","EndHour", mConfigControl.MondayEndHour);
			Configuration.Set( @"Schedule\Tuesday","Free", mConfigControl.FreeTuesday);
			Configuration.Set( @"Schedule\Tuesday","StartHour", mConfigControl.TuesdayStartHour);
			Configuration.Set( @"Schedule\Tuesday","EndHour", mConfigControl.TuesdayEndHour);
			Configuration.Set( @"Schedule\Wednesday","Free", mConfigControl.FreeWednesday);
			Configuration.Set( @"Schedule\Wednesday","StartHour", mConfigControl.WednesdayStartHour);
			Configuration.Set( @"Schedule\Wednesday","EndHour", mConfigControl.WednesdayEndHour);
			Configuration.Set( @"Schedule\Thursday","Free", mConfigControl.FreeThursday);
			Configuration.Set( @"Schedule\Thursday","StartHour", mConfigControl.ThursdayStartHour);
			Configuration.Set( @"Schedule\Thursday","EndHour", mConfigControl.ThursdayEndHour);
			Configuration.Set( "Constraints","MinFreeDays", mConfigControl.MinFreeDays);
			Configuration.Set( "Constraints","MinDailyHours", mConfigControl.MinDailyHours);
			Configuration.Set( "Constraints","MaxDailyHours", mConfigControl.MaxDailyHours);
		}
		private void UpdateGUI()
		{
			mConfigControl.AllowCollisions = Configuration.Get( "General","AllowCollisions", mConfigControl.AllowCollisions );
			mConfigControl.MaxCollisions = Configuration.Get( "General","MaxCollisions", mConfigControl.MaxCollisions);
			mConfigControl.AllowRegSplit = Configuration.Get( "General","AllowRegSplit", mConfigControl.AllowRegSplit);
			mConfigControl.TestAlert = Configuration.Get( "General","TestAlert", mConfigControl.TestAlert);
			mConfigControl.TestInterval = Configuration.Get( "General","TestInterval", mConfigControl.TestInterval);
			mConfigControl.Sunday = Configuration.Get( "Display","Sunday", mConfigControl.Sunday);
			mConfigControl.Monday = Configuration.Get( "Display","Monday", mConfigControl.Monday);
			mConfigControl.Tuesday = Configuration.Get( "Display","Tuesday", mConfigControl.Tuesday);
			mConfigControl.Wednesday = Configuration.Get( "Display","Wednesday", mConfigControl.Wednesday);
			mConfigControl.Thursday = Configuration.Get( "Display","Thursday", mConfigControl.Thursday);
			mConfigControl.Friday = Configuration.Get( "Display","Friday", mConfigControl.Friday);
			mConfigControl.Saturday = Configuration.Get( "Display","Saturday", mConfigControl.Saturday);
			mConfigControl.StartHour = Configuration.Get( "Display","StartHour", mConfigControl.StartHour);
			mConfigControl.EndHour = Configuration.Get( "Display","EndHour", mConfigControl.EndHour);
			mConfigControl.PrefStartHour = Configuration.Get( "Pref","PrefStartHour", mConfigControl.PrefStartHour);
			mConfigControl.PrefEndHour = Configuration.Get( "Pref","PrefEndHour", mConfigControl.PrefEndHour);
			mConfigControl.PrefFreeDays = Configuration.Get( "Pref","PrefFreeDays", mConfigControl.PrefFreeDays);
			mConfigControl.PrefHoles = Configuration.Get( "Pref","PrefHoles", mConfigControl.PrefHoles);
			mConfigControl.ExportName = Configuration.Get( "Export","ExportName", mConfigControl.ExportName);
			mConfigControl.ExportNumber = Configuration.Get( "Export","ExportNumber", mConfigControl.ExportNumber);
			mConfigControl.ExportType = Configuration.Get( "Export","ExportType", mConfigControl.ExportType);
			mConfigControl.ExportRegNum = Configuration.Get( "Export","ExportRegNum", mConfigControl.ExportRegNum);
			mConfigControl.ExportLocation = Configuration.Get( "Export","ExportLocation", mConfigControl.ExportLocation);
			mConfigControl.ExportTeacher = Configuration.Get( "Export","ExportTeacher", mConfigControl.ExportTeacher);
			mConfigControl.FreeSunday = Configuration.Get( @"Schedule\Sunday","Free", mConfigControl.FreeSunday);
			mConfigControl.SundayStartHour = Configuration.Get( @"Schedule\Sunday","StartHour", mConfigControl.SundayStartHour);
			mConfigControl.SundayEndHour = Configuration.Get( @"Schedule\Sunday","EndHour", mConfigControl.SundayEndHour);
			mConfigControl.FreeMonday = Configuration.Get( @"Schedule\Monday","Free", mConfigControl.FreeMonday);
			mConfigControl.MondayStartHour = Configuration.Get( @"Schedule\Monday","StartHour", mConfigControl.MondayStartHour);
			mConfigControl.MondayEndHour = Configuration.Get( @"Schedule\Monday","EndHour", mConfigControl.MondayEndHour);
			mConfigControl.FreeTuesday = Configuration.Get( @"Schedule\Tuesday","Free", mConfigControl.FreeTuesday);
			mConfigControl.TuesdayStartHour = Configuration.Get( @"Schedule\Tuesday","StartHour", mConfigControl.TuesdayStartHour);
			mConfigControl.TuesdayEndHour = Configuration.Get( @"Schedule\Tuesday","EndHour", mConfigControl.TuesdayEndHour);
			mConfigControl.FreeWednesday = Configuration.Get( @"Schedule\Wednesday","Free", mConfigControl.FreeWednesday);
			mConfigControl.WednesdayStartHour = Configuration.Get( @"Schedule\Wednesday","StartHour", mConfigControl.WednesdayStartHour);
			mConfigControl.WednesdayEndHour = Configuration.Get( @"Schedule\Wednesday","EndHour", mConfigControl.WednesdayEndHour);
			mConfigControl.FreeThursday = Configuration.Get( @"Schedule\Thursday","Free", mConfigControl.FreeThursday);
			mConfigControl.ThursdayStartHour = Configuration.Get( @"Schedule\Thursday","StartHour", mConfigControl.ThursdayStartHour);
			mConfigControl.ThursdayEndHour = Configuration.Get( @"Schedule\Thursday","EndHour", mConfigControl.ThursdayEndHour);
			mConfigControl.MinFreeDays = Configuration.Get( "Constraints","MinFreeDays", mConfigControl.MinFreeDays);
			mConfigControl.MinDailyHours = Configuration.Get( "Constraints","MinDailyHours", mConfigControl.MinDailyHours);
			mConfigControl.MaxDailyHours = Configuration.Get( "Constraints","MaxDailyHours", mConfigControl.MaxDailyHours);
		}

		private void HideLabel()
		{
			System.Threading.Thread.Sleep( 3000 );
			mConfigControl.SavedLabel.Visible = false;
		}
		/*public object this[ string key ]
		{
			get
			{
				return mConfigDictionary[ key ];
				/*switch(key)
				{
					case "AllowCollisions": { return mConfigControl.AllowCollisions; }
					case "MaxCollisions": { return mConfigControl.MaxCollisions; }
					case "AllowRegSplit": { return mConfigControl.AllowRegSplit; }
					case "TestAlert": { return mConfigControl.TestAlert; }
					case "TestInterval": { return mConfigControl.TestInterval; }
					case "Sunday": { return mConfigControl.Sunday; }
					case "Monday": { return mConfigControl.Monday; }
					case "Tuesday": { return mConfigControl.Tuesday; }
					case "Wednesday": { return mConfigControl.Wednesday; }
					case "Thursday": { return mConfigControl.Thursday; }
					case "Friday": { return mConfigControl.Friday; }
					case "Saturday": { return mConfigControl.Saturday; }
					case "StartHour": { return mConfigControl.StartHour; }
					case "EndHour": { return mConfigControl.EndHour; }
					case "PrefStartHour": { return mConfigControl.PrefStartHour; }
					case "PrefEndHour": { return mConfigControl.PrefEndHour; }
					case "PrefFreeDays": { return mConfigControl.PrefFreeDays; }
					case "PrefHoles": { return mConfigControl.PrefHoles; }
					default: { return null; }
				}
			}
		}*/
		public event ConfigurationChangedHandler ConfigurationChanged
		{
			add{ mConfigurationChanged += value;  }
			remove{ mConfigurationChanged -= value;  }
		}
		public static int GetExportVerbosityFlag()
		{
			int num = 0;
			if (Configuration.Get( "Export","ExportRegNum", true)) num += 1;
			if (Configuration.Get( "Export","ExportNumber", true)) num +=2;
			if (Configuration.Get( "Export","ExportName", true)) num +=4;
			if (Configuration.Get( "Export","ExportType", true)) num +=8;
			if (Configuration.Get( "Export","ExportTeacher", true)) num +=16;
			if (Configuration.Get( "Export","ExportLocation", false)) num +=32;
			return num;
		}

		private void configControl_VisibleChanged(object sender, EventArgs e)
		{
			this.UpdateGUI();
		}
	}
}
