using Koalite.EFSample.Model;

namespace Koalite.EFSample.DB
{
	public class SampleDataLoader
	{
		private readonly MovieDbContext dbContext;

		public SampleDataLoader(MovieDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public void LoadData()
		{
			var usa = new Country("USA");
			var spain = new Country("España");
			var france = new Country("Francia");

			dbContext.Countries.Add(usa);
			dbContext.Countries.Add(spain);
			dbContext.Countries.Add(france);

			var juanma = new Person("Juanma Bajo Ulloa");
			var james = new Person("James Cameron");

			dbContext.People.Add(juanma);
			dbContext.People.Add(james);

			var airbag = new Movie("Airbag", 1997, juanma, spain);
			var terminator = new Movie("Terminator", 1984, james, usa);

			dbContext.Movies.Add(airbag);
			dbContext.Movies.Add(terminator);

			dbContext.SaveChanges();
		}
	}
}