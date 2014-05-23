using Koalite.EFSample.DB;
using NUnit.Framework;

namespace Tests
{
	[SetUpFixture]
    public class DBTestSetup
    {
		[SetUp]
		public void Setup()
		{
			using (var context = new TestDbContext())
				DbTools.CreateNewDatabase(context);
		}
    }
}
