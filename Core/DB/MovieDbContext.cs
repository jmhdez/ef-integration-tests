using System.Data.Entity;
using Koalite.EFSample.Model;

namespace Koalite.EFSample.DB
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(string dbNameOrConnectionString)
            : base(dbNameOrConnectionString)
        {
            // No quiero que se cree mágicamente bases de datos
            Database.SetInitializer<MovieDbContext>(null);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }

	    protected override void OnModelCreating(DbModelBuilder modelBuilder)
	    {
		    base.OnModelCreating(modelBuilder);

		    modelBuilder.Entity<Movie>()
			    .HasMany(x => x.Countries)
				.WithMany()
				.Map(x =>
				{
					x.ToTable("MovieCountries");
					x.MapLeftKey("MovieId");
					x.MapRightKey("CountryId");
				});
	    }
    }
}
