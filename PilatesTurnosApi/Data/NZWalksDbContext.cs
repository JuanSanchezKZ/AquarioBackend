using Microsoft.EntityFrameworkCore;
using PilatesTurnosApi.Models.Domain;

namespace PilatesTurnosApi.Data
{
    public class NZWalksDbContext: DbContext
    {

        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

            
        }

        public DbSet<Difficulty> Difficulty { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
