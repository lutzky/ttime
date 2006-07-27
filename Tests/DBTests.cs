using UDonkey.DB;

namespace UDonkey.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class DBTests
	{
		[Test]
		public void TestAutoUpdate()
		{
			CourseDB db = new CourseDB();
			db.AutoUpdate();
		}
	}
}
