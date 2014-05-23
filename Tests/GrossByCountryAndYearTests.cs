using System.Linq;
using Koalite.EFSample.Model;
using Koalite.EFSample.Query;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class GrossByCountryAndYearTests : DBTest
	{
		private Country spain;
		private Country usa;
		private Person lucas;

		protected override void BeforeAll()
		{
			Execute(context =>
			{
				spain = new Country("España");
				usa = new Country("USA");
				lucas = new Person("George Lucas");

				context.Countries.Add(spain);
				context.Countries.Add(usa);
				context.People.Add(lucas);
			});
		}

		protected override void BeforeEach()
		{
			// Esto no es lo más eficiente porque carga las entidades antes de borrarlas
			// pero hace el test más resistente ante cambios de esquema o relaciones entre
			// entidades. Si te preocupa *mucho* el rendimiento, puedes borrar a mano las
			// tablas necesarias
			Execute(context => context.Movies.RemoveRange(context.Movies.ToList()));
		}

		private GrossByCountryAndYearQuery.Result[] GetResults(Country country)
		{
			return Query(context => new GrossByCountryAndYearQuery(country.Name).Execute(context).ToArray());
		}

		private void AddMovie(string title, int year, decimal gross, params Country[] countries)
		{
			Execute(context => context.Movies.Add(new Movie(title, year, lucas, countries) { Gross = gross }));
		}

		[Test]
		public void Returns_Empty_Results_If_There_Are_No_Movies()
		{
			var result = GetResults(spain);

			Assert.That(result, Is.Empty);
		}

		[Test]
		public void Returns_Only_Movies_From_Selected_Country()
		{
			AddMovie("Titanic", 1997, 1000, usa);
			AddMovie("Airbag", 1997, 25, spain);

			var result = GetResults(spain);

			Assert.That(result.Length, Is.EqualTo(1));
			Assert.That(result[0].Gross, Is.EqualTo(25));
		}

		[Test]
		public void Returns_Coproductions_From_Selected_Country()
		{
			AddMovie("El Laberinto del Fauno", 2003, 75, spain, usa);

			var result = GetResults(spain);

			Assert.That(result.Length, Is.EqualTo(1));
		}

		[Test]
		public void Returns_Movies_From_Multiple_Years()
		{
			AddMovie("Airbag", 1997, 25, spain);
			AddMovie("El Laberinto del Fauno", 2003, 75, spain, usa);

			var result = GetResults(spain);

			Assert.That(result.Length, Is.EqualTo(2));
			Assert.That(result[0].Year, Is.EqualTo(2003));
			Assert.That(result[1].Year, Is.EqualTo(1997));
		}

		[Test]
		public void Returns_The_Total_Gross_By_Year()
		{
			AddMovie("Airbag", 1997, 25, spain);
			AddMovie("Abre los ojos", 1997, 100, spain);

			var result = GetResults(spain);

			Assert.That(result.Length, Is.EqualTo(1));
			Assert.That(result[0].Gross, Is.EqualTo(125));
		}
	}
}