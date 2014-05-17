using System.Data.Entity;

namespace Koalite.EFSample.DB
{
	public static class DbTools
	{
		public static void CreateNewDatabase(MovieDbContext context)
		{
			new DropCreateDatabaseAlways<MovieDbContext>()
				.InitializeDatabase(context);
		}

		public static void CleanDatabase(MovieDbContext context)
		{
			new DbCleaner(context).DeleteAllData();
		}

		public static void LoadSampleData(MovieDbContext context)
		{
			new SampleDataLoader(context).LoadData();
		}
	}
}