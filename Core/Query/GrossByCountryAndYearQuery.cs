using System.Collections.Generic;
using System.Linq;
using Koalite.EFSample.DB;

namespace Koalite.EFSample.Query
{
	public class GrossByCountryAndYearQuery
	{
		public class Result
		{
			public int Year { get; private set; }
			public decimal Gross { get; private set; }
		}

		private readonly string country;

		public GrossByCountryAndYearQuery(string country)
		{
			this.country = country;
		}

		public IEnumerable<Result> Execute(MovieDbContext context)
		{
			const string sql = @"
				select 
					[Year] as [Year], 
					sum(Gross) as [Gross]
				from 
					Movies m
					inner join MovieCountries mc on m.Id = mc.MovieId
					inner join Countries c on mc.CountryId = c.Id
				where 
					c.[Name] = @p0
				group by
					[Year]
				order by
					[Year] desc";

			return context.Database.SqlQuery<Result>(sql, country).ToArray();
		}
	}
}
