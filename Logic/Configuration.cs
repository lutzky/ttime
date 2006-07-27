using System;
using System.Collections;
using System.Xml;
using System.IO;

namespace UDonkey.Logic
{
	/// <summary>
	/// Handler and Configuration change
	/// </summary>
	public delegate void ConfigurationChangeHandler( string path, string name, object newVal, object oldVal );
	/// <summary>
	/// Summary description for Configuration.
	/// </summary>
	public class Configuration
	{
		public static string GENERAL     =  "General";
		public static string CONSTRAINTS =  "Constraints";
		public static string DISPLAY     =  "Display";
		public static string SORT        =  "Pref";

		private static Hashtable mConfigDictionary;
		private static Hashtable mListenersDictionary;
		#region Constructors
		static Configuration()
		{
			mConfigDictionary  = new Hashtable();
			mListenersDictionary = new Hashtable();
		}
		/// <summary>
		/// Private constructor to avoid instances of this item
		/// </summary>
		private Configuration(){}
		#endregion Constructors
		#region Static Methods
		public static bool Load()
		{
			Stream strm;
			try
			{
				strm = File.OpenRead("config.xml");
			}
			catch(System.IO.FileNotFoundException)
			{
				return false;
			}
			XmlDataDocument configFile = new XmlDataDocument();
			configFile.Load(strm);
			XmlElement root = configFile.DocumentElement;
			XmlNode rootNode = root.SelectSingleNode("/Config");
			UDonkey.IO.IOManager.ImportConfigFromXml(rootNode);
			strm.Close();
			return true;
		}
		


		#region Get
		/// <summary>
		/// Returns an object from the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="def">Default value if not exists</param>
		/// <returns></returns>
		private static object Get( string path, string name, object def )
		{
			object o = mConfigDictionary[ GetKey(path,name) ];
			return ( o == null )? def: o;
		}
		/// <summary>
		/// Returns an object from the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="def">Default value if not exists</param>
		/// <returns></returns>
		public static int Get( string path, string name, int def )
		{
			return int.Parse( Get( path, name, def.ToString())  );
		}
		/// <summary>
		/// Returns an object from the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="def">Default value if not exists</param>
		/// <returns></returns>
		public static string Get( string path, string name, string def )
		{
			return Get( path, name, (object)def) as string;
		}
		/// <summary>
		/// Returns an object from the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="def">Default value if not exists</param>
		/// <returns></returns>
		public static bool Get( string path, string name, bool def )
		{
			return bool.Parse( Get( path, name, def.ToString()) );
		}
		#endregion Get
		#region Set
		/// <summary>
		/// Set an object to the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="val">The value of the property</param>
		/// <returns></returns>
		public static void Set( string path, string name, string val )
		{
			string oldVal = string.Empty;
			object o = mConfigDictionary[ GetKey(path,name) ];
			if( o != null )
			{
				oldVal = o.ToString();
			}
			Set( path, name, val, oldVal );
		}
		/// <summary>
		/// Set an object to the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="val">The value of the property</param>
		/// <returns></returns>
		public static void Set( string path, string name, bool val )
		{
			bool oldVal = false;
			object o = mConfigDictionary[ GetKey(path,name) ];
			if( o != null )
			{
				oldVal = bool.Parse(o.ToString());
			}
			Set( path, name, val, oldVal );
		}
		/// <summary>
		/// Set an object to the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="val">The value of the property</param>
		/// <returns></returns>
		public static void Set( string path, string name, int val )
		{
			int oldVal = 0;
			object o = mConfigDictionary[ GetKey(path,name) ];
			if( o != null )
			{
				oldVal = int.Parse(o.ToString());
			}
			Set( path, name, val, oldVal );
		}
		/// <summary>
		/// Set an object to the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="val">The value of the property</param>
		/// <returns></returns>
		public static void Set( string path, string name, object val )
		{
			object oldVal = mConfigDictionary[ GetKey(path,name) ];
			Set( path, name, val, oldVal );
		}
		/// <summary>
		/// Set an object to the configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="val">The value of the property</param>
		/// <returns></returns>
		private static void Set( string path, string name, object newVal, object oldVal )
		{
			//Call the key listeners
			CallEvents( (ArrayList)mListenersDictionary[ GetKey(path,name)],
				path,name,newVal,oldVal );
			//Call the path listeners
			CallEvents( (ArrayList)mListenersDictionary[ GetKey(path,"") ],
				path,name,newVal,oldVal );
			mConfigDictionary[ GetKey(path,name) ] = newVal.ToString();
		}
		#endregion Set
		/// <summary>
		/// Register a listener on a configuration object
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="handler">Handler for the change</param>
		public static void RegisterConfigurationChangeHandler( 
			string path, string name, ConfigurationChangeHandler handler )
		{
			ArrayList col = (ArrayList)mListenersDictionary[ GetKey(path,name) ];
			if( col == null )
			{
				col = new ArrayList();
				mListenersDictionary[ GetKey(path,name) ] = col;
			}
			col.Add( handler );
		}

		/// <summary>
		/// Unregister Configuration Cahnge handler from the Configuration
		/// </summary>
		/// <param name="path">Path of the configuration object</param>
		/// <param name="name">Name of the configuration object</param>
		/// <param name="handler">Handler for the change</param>
		public static void UnRegisterConfigurationChangeHandler( 
			string path, string name, ConfigurationChangeHandler handler )
		{
			ArrayList col = (ArrayList)mListenersDictionary[ GetKey(path,name) ];
			if( col == null )
			{
				return;
			}
			if( col.Contains( handler ) )
			{
				col.Remove( handler );
			}
			if( col.Count == 0 )
			{
				mListenersDictionary[ GetKey(path,name) ] = null;
			}
		}
		private static string GetKey( string path, string name )
		{
			return string.Format( @"{0}\{1}", path, name );
		}
		private static void CallEvents( ArrayList list, string path, string name, object newVal, object oldVal )
		{
			if ( list != null )
			{			
				foreach( ConfigurationChangeHandler handler in list )
				{
					handler( path, name, newVal, oldVal );
				}
			}
		}

		public static void SaveToXml()
		{
			XmlTextWriter writer = new XmlTextWriter ("config.xml", null);
			writer.WriteStartDocument();
			UDonkey.IO.IOManager.ExportConfigBodyToXml(writer);
			writer.Flush();
			writer.Close();
		}
		public static Hashtable ConfigDictionary
		{
			get { return mConfigDictionary; }
		}

		#endregion Static Methods
	}
}
