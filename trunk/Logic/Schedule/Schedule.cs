using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
//using UDonkey.GUI;

namespace UDonkey.Logic
{
	/// <summary>
	/// TODO: Summary description for Schedule.
	/// </summary>
	public class Schedule
	{
		public  const  string   DATA_TABLE_NAME   = "Schedule";
		public  const  string   DISPLAY_CONF_PATH = "Display";
		public  const  string   GENERAL_CONF_PATH = "General";
		private System.Collections.Hashtable	  mDays;
		private DataTable					      mDataTable;
		private int                               mMaxCollisions;
		private event EventHandler				  mChanged;
		#region Constructors
		/// <summary>
		/// Create new Schedule
		/// </summary>
		public Schedule()
		{
			mDays      = CreateScheduleDayDataCollection();
			mDataTable = CreateScheduleDataTable();
			if( Configuration.Get(Configuration.GENERAL,"AllowCollisions",false) )
			{
				mMaxCollisions =  Configuration.Get(Configuration.GENERAL,"MaxCollisions",3);
			}
			else
			{
				mMaxCollisions = 1;
			}
			Configuration.RegisterConfigurationChangeHandler( Configuration.GENERAL,
				"AllowCollisions", new ConfigurationChangeHandler( this.ConfigurationChange ) );
			Configuration.RegisterConfigurationChangeHandler( Configuration.GENERAL,
				"MaxCollisions", new ConfigurationChangeHandler( this.ConfigurationChange ) );
		}
		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="from">Original Schedule</param>
		public Schedule( Schedule from )
		{
			mDataTable = from.DataTable.Copy();
			mDays      = (System.Collections.Hashtable)from.mDays.Clone();
		}
		#endregion Constructors
		#region Methods
		/// <summary>
		/// Clone this Schedule
		/// </summary>
		/// <returns>The cloned Schedule</returns>
		public Schedule Clone()
		{
			return new Schedule( this );
		}
		/// <summary>
		/// Add ScheduleObject to this Schedule
		/// </summary>
		/// <param name="obj">ScheduleObject to add</param>
		public bool AddScheduleObject( IScheduleObject obj )
		{

			if( this.ScheduleDayData( obj.DayOfWeek ).AddScheduleObject( obj ) )
			{
				obj.Schedule = this;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Get ScheduleObject at day and Start Time 
		/// </summary>
		/// <param name="day">Day</param>
		/// <param name="time">Start Time</param>
		/// <returns>The ScheduleObject at this location</returns>
		public IScheduleObject GetScheduleObject( DayOfWeek day, int time )
		{
			return this.ScheduleDayData( day ).GetScheduleObject( time ) as IScheduleObject;
		}
		/// <summary>
		/// Remove the ScheduleObject from the Schedule
		/// </summary>
		/// <param name="obj">ScheduleObject to remove</param>
		public void RemoveScheduleObject( IScheduleObject obj )
		{
			this.ScheduleDayData( obj.DayOfWeek ).RemoveScheduleObject( obj );
		}
		public bool ContainsScheduleObject( IScheduleObject obj )
		{
			IScheduleEntry entry = mDataTable.Rows[ obj.StartHour ][ obj.DayOfWeek.ToString() ] as IScheduleEntry;
			return entry.ContainsKey( obj.Key );
		}
		public  ScheduleDayData ScheduleDayData( DayOfWeek day )
		{
			return (ScheduleDayData)mDays[ day ];
		}
		public int RowToHour( int rowIndex )
		{
			return this.StartHour + rowIndex;
		}
		public int HourToRow( int hour )
		{
			return hour - this.StartHour;
		}
		
		public void CallChanged()
		{
			if( mChanged != null )
			{
				mChanged( this, new EventArgs() );
			}
		}
		#region Import/Export
		private const string ROOT_NODE            = "ScheduleDB";
		private const string SCHEDULE_OBJECT_NODE = "ScheduleObject";
		private const string KEY_ATTRIBUTE        = "Key";
		private const string COLUMN_ATTRIBUTE     = "ColumnName";
		private const string ROW_ATTRIBUTE        = "RowNumber";
		private const string BG_COLOR_ATTRIBUTE   = "BGColor";
		private const string FG_COLOR_ATTRIBUTE   = "FGColor";
		public void ExportToXml ( string filename )
		{			
			if( filename.Length == 0  )
			{
				return;
			}
			XmlTextWriter ScheduleWriter;
			ScheduleWriter = new XmlTextWriter (filename, null);
			ScheduleWriter.Formatting = Formatting.Indented;
			ScheduleWriter.Indentation= 6;
			ScheduleWriter.Namespaces = false;
			int row=0;

			try
			{
				ScheduleWriter.WriteStartDocument();
				ScheduleWriter.WriteStartElement(ROOT_NODE);
			}
			catch(Exception e)
			{
				Console.WriteLine("Exception: {0}", e.ToString());
			}
			//going throughout the columns (days)
			foreach (DataColumn myCol in mDataTable.Columns)
			{			
				//going throughout the rows (hour)
				foreach(DataRow myRow in mDataTable.Rows)
				{			
					++row;
						IScheduleEntry entry =  myRow[myCol] as IScheduleEntry;
					if ( !entry.IsEmpty )
					{

						foreach( System.Collections.DictionaryEntry de in  entry )
						{
							IScheduleObject obj = (IScheduleObject) de.Value;
							//start ScheduleObject
							ScheduleWriter.WriteStartElement   ( SCHEDULE_OBJECT_NODE);
							ScheduleWriter.WriteAttributeString( COLUMN_ATTRIBUTE, myCol.ColumnName );
							ScheduleWriter.WriteAttributeString( ROW_ATTRIBUTE   , row.ToString());
							ScheduleWriter.WriteAttributeString( KEY_ATTRIBUTE, obj.Key);
							//ScheduleWriter.WriteAttributeString( BG_COLOR_ATTRIBUTE , obj.BackColor.ToArgb().ToString() );
							//ScheduleWriter.WriteAttributeString( FG_COLOR_ATTRIBUTE , obj.ForeColor.ToArgb().ToString() );
							foreach( VerbosityFlag flag in Enum.GetValues( typeof(VerbosityFlag) ) )
							{
								ScheduleWriter.WriteAttributeString (flag.ToString(), obj.ToString( flag ) );
							}
							// End ScheduleObject
							ScheduleWriter.WriteEndElement();
						}												
					}
				}
				row=0;
			}

			ScheduleWriter.WriteEndElement(); // End ScheduleDB element	
			
			ScheduleWriter.Flush();
			ScheduleWriter.Close();
			return;
		}

		static public Schedule CreateFromXml( string filename )
		{	
			Schedule schedule = new Schedule( );
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(filename);
			
				//dataTable mDataTable = new DataTable();
				XmlNode rootNode = doc[ROOT_NODE];
				foreach( XmlNode node in rootNode.ChildNodes )
				{
					if( node.Name == SCHEDULE_OBJECT_NODE )
					{
						ImportedScheduleObject o = 
							new ImportedScheduleObject( node.Attributes[KEY_ATTRIBUTE].Value );

						o.DayOfWeek = (DayOfWeek)Enum.Parse( typeof(DayOfWeek),
							node.Attributes[ COLUMN_ATTRIBUTE ].Value, true );
						o.StartHour = int.Parse( node.Attributes[ ROW_ATTRIBUTE ].Value );
						//o.BackColor = System.Drawing.Color.FromArgb(
						//	int.Parse( node.Attributes[ BG_COLOR_ATTRIBUTE ].Value ) );
						//o.ForeColor = System.Drawing.Color.FromArgb(
						//	int.Parse( node.Attributes[ FG_COLOR_ATTRIBUTE ].Value ) );
						foreach( VerbosityFlag flag in Enum.GetValues( typeof(VerbosityFlag) ) )
						{
							o.VerbosityStrings.Add( flag, node.Attributes[ flag.ToString() ].Value );
						}	
						schedule.AddScheduleObject( o );
					}
				}
			}
			catch
			{
				//TODO: Handle this exception
				return null;
			}
			return schedule;
		}
		#endregion Import/Export
		private System.Collections.Hashtable CreateScheduleDayDataCollection()
		{
			System.Collections.Hashtable daysCollection = new System.Collections.Hashtable();
			foreach ( DayOfWeek day in Enum.GetValues( typeof(DayOfWeek) ) )
			{
				daysCollection.Add( day, new ScheduleDayData( this, day ) ); 
			}
			return daysCollection;
		}
		private DataTable CreateScheduleDataTable()
		{
			DataTable returnDataTable = new DataTable( DATA_TABLE_NAME );
			DataColumn column;
			DataRow row;

			//Creating the hours column
			/*column = new DataColumn( "שעות", typeof(string) );
			returnDataTable.Columns.Add(column);*/

			foreach ( string day in Enum.GetNames( typeof(DayOfWeek) ) )
			{
				column = new DataColumn( day, typeof(IScheduleObject) );
				returnDataTable.Columns.Add(column);
			}

			//Creating the days columns
			for ( int i = 0 ; i < 24 ; ++i )
			{
				row = returnDataTable.NewRow();
				foreach ( string day in Enum.GetNames( typeof(DayOfWeek) ) )
				{
					row[ day.ToString() ] = new ScheduleEntryBucket( this );
				}
				row[ "שעות" ] =  new HourColumnScheduleEntry( i );
				returnDataTable.Rows.Add( row );
			}
			return returnDataTable;
		}

		private void ConfigurationChange( string path, string name, object newVal, object oldVal )
		{
			if( Configuration.Get(Configuration.GENERAL,"AllowCollisions",false) )
			{
				mMaxCollisions =  Configuration.Get(Configuration.GENERAL,"MaxCollisions",3);
			}
			else
			{
				mMaxCollisions = 1;
			}
			return;
		}
		#endregion Methods
		#region Properties
		/// <summary>
		/// Return DataTable representation of this Schedule
		/// </summary>
		public DataTable DataTable
		{
			get
			{
				DataTable retTable = mDataTable.Copy();
				for ( int i = 0 ; i < this.StartHour ; ++i )
				{//The new first index is always 0
					retTable.Rows[0].Delete();
				}

				for ( int i = this.EndHour + 1; i < 24 ; ++i )
				{//The new last index will be EndHour - StartHour
					retTable.Rows[this.EndHour - this.StartHour + 1 ].Delete();
				}
				return retTable;
			}
                   
		}
		public DataTable FullDataTable
		{
			get
			{
				return mDataTable;
			}
                   
		}
		/// <summary>
		/// Return the collection of ScheduleDayData objects
		/// </summary>
		public System.Collections.Hashtable DaysData
		{
			get { return mDays; }
		}
		public int StartHour
		{
			get { return Configuration.Get(DISPLAY_CONF_PATH,"StartHour",8);  }
			set { Configuration.Set(DISPLAY_CONF_PATH,"StartHour",value);     }
		}
		public int EndHour
		{
			get { return Configuration.Get(DISPLAY_CONF_PATH,"EndHour",20);  }
			set { Configuration.Set(DISPLAY_CONF_PATH,"EndHour",value);     }
		}

		public int AllowedOverlaps
		{
			get 
			{ 
				return mMaxCollisions;
			}
			set 
			{ 
				Configuration.Set( DISPLAY_CONF_PATH,"AllowCollisions", value > 1 );
				Configuration.Set( DISPLAY_CONF_PATH,"MaxCollisions", ( value > 1 )?value:1 );
			}
		}
		public event EventHandler Changed
		{
			add   { mChanged += value; }
			remove{ mChanged -= value; }
		}
		#endregion Properties
	}
}
