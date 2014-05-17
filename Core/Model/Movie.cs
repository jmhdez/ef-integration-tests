using System.Collections.Generic;
using System.Linq;

namespace Koalite.EFSample.Model
{
    public class Movie
    {
        private decimal gross;

        protected Movie() { /*EF*/ }

        public Movie(string title, int year, Person director, params Country[] countries)
        {
            Check.Require(!string.IsNullOrWhiteSpace(title), "Title is required");
            Check.Require(year > 1850, "No film was recorded prior to 1850");
            Check.Require(director != null, "Someone had to direct the film");
            Check.Require(countries != null && countries.Length > 0, "Al least one country is required");

            Title = title;
            Year = year;
	        Director = director;
            Countries = countries.ToList();
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public int Year { get; private set; }
        public virtual Person Director { get; private set; }
        public virtual ICollection<Country> Countries { get; private set; }

        public decimal Gross
        {
            get { return gross; }
            set
            {
                Check.Require(value >= 0, "Gross must be a positive number");
                gross = value;
            }
        }
    }
}