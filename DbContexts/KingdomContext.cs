using Microsoft.EntityFrameworkCore;
using EntityFramework.Exceptions.PostgreSQL;

using KingdomApi.Models;

namespace KingdomApi
{
    public class KingdomContext : DbContext
    {
        public DbSet<Kingdom> Kingdoms { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Noble> Nobles { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseNpgsql("Host=localhost;Database=kingdom;Username=postgres;Password=example")
                .UseExceptionProcessor()
                .UseSnakeCaseNamingConvention();
    }
}
