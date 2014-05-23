using Koalite.EFSample.DB;

namespace Tests
{
	public class TestDbContext : MovieDbContext
	{
		public TestDbContext() : base("movietestdb")
		{
		}
	}
}