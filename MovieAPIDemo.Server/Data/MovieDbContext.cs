using Microsoft.EntityFrameworkCore;
using MovieAPIDemo.Server.Entities;

namespace MovieAPIDemo.Server.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext( DbContextOptions<MovieDbContext> _Options ) : base( _Options )
        {
            
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Person> Person { get; set; }

        protected override void OnModelCreating( ModelBuilder _ModelBuilder )
        {
            base.OnModelCreating( _ModelBuilder );
        }
    }
}
