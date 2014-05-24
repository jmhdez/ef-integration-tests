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
				spain = new Country("Espa�a");
				usa = new Country("USA");
				lucas = new Person("George Lucas");

				context.Countries.Add(spain);
				context.Countries.Add(usa);
				context.People.Add(lucas);

				context.SaveChanges();
			});
		}

		public override void BeforeEach()
		{
			// Esto no es lo m�s eficiente porque carga las entidades antes de borrarlas
			// pero hace el test m�s resistente ante cambios de esquema o relaciones entre
			// entidades. Si te preocupa *mucho* el rendimiento, puedes borrar a mano las
			// tablas necesarias
			Execute(context =>
			{
				context.Movies.RemoveRange(context.Movies.ToList());
				context.SaveChanges();
			});
		}

		private GrossByCountryAndYearQuery.Result[] GetResults(Country country)
		{
			var query = new GrossByCountryAndYearQuery(country.Name);
			return Execute(context => query.Execute(context)).ToArray();
		}

		private void AddMovie(string title, int year, decimal gross, params Country[] countries)
		{
			// Esto no deber�a hacerse porque usamos una tx para guardar cada pel�cula, perdiendo
			// el UoW y eficiencia, pero a cambio los tests quedan m�s legibles. 
			// En este caso, prefiero legibilidad.
			Execute(context =>
			{
				context.Movies.Add(new Movie(title, year, lucas, countries) {Gross = gross});
				context.SaveChanges();
			});
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