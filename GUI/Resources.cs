using System;
using System.Resources;

namespace UDonkey.GUI
{
	/// <summary>
	/// Summary description for Resources.
	/// </summary>
	public class Resources
	{
		static private ResourceManager mResourceManager;
		static public string GeneralResource = "General";
		static Resources()
		{
			mResourceManager = new ResourceManager( typeof(Resources) );
			mResourceManager.IgnoreCase = true;
		}
		static public ResourceManager ResourceManager
		{
			get{ return mResourceManager; }
		}
		static public string String(string group, string name)
		{
			string resourceName = string.Format("{0}_{1}",group,name );
			return (string)mResourceManager.GetObject( resourceName );
		}
	}
	
}
