using System;
using Koalite.EFSample.DB;

namespace Koalite.EFSample.Console
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				System.Console.Out.WriteLine("Uso: efcmd [--new|--clean|--sample] dbName");
				Environment.Exit(-1);
			}

			var command = args[0];
			var dbName = args[1];

			using (var context = new MovieDbContext(dbName))
			{
				switch (command)
				{
					case "--new":
						System.Console.Out.WriteLine("Creating database {0}...", dbName);
						DbTools.CreateNewDatabase(context);
						System.Console.Out.WriteLine("Database created.");
						break;
					
					case "--clean":
						System.Console.Out.WriteLine("Deleting all data in {0}", dbName);
						DbTools.CleanDatabase(context);
						System.Console.Out.WriteLine("Data deleted.");
						break;

					case "--sample":						
						System.Console.Out.WriteLine("Creating database {0}...", dbName);
						DbTools.CreateNewDatabase(context);
						System.Console.Out.WriteLine("Database created.");

						System.Console.Out.WriteLine("Loading sample data...");
						DbTools.LoadSampleData(context);
						System.Console.Out.WriteLine("Sample data loaded.");
						break;
				}
			}
		}
	}
}
