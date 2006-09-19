using UDonkey.DB;
using UDonkey.RepFile;

namespace UDonkey.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class DBTests
	{
/*		[Test]
		public void TestAutoUpdate()
		{
			CourseDB db = new CourseDB();
			db.AutoUpdate();
		}*/

		[Test]
		public void TestConversion()
		{
			RepToXML.Convert("REPY", CourseDB.DEFAULT_DB_FILE_NAME);
		}
	}
}
