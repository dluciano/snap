using GameSharp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Snap.Entities;

namespace Snap.DataAccess
{
    public class SnapDbContext : GameSharpContext
    {
        public DbSet<PlayerGameplay> PlayerGamePlays { get; set; }
        public DbSet<PlayersData> PlayerPiles { get; set; }
        public DbSet<SnapGame> SnapGames { get; set; }
        public DbSet<StackNode> StackNodes { get; set; }

        public SnapDbContext()
        {
        }
        public SnapDbContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=Snap.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<SnapGame>()
                .OwnsOne(p => p.CentralPile);
            modelBuilder
                .Entity<PlayersData>()
                .OwnsOne(p => p.StackEntity);
            modelBuilder
                .Entity<PlayersData>()
                .HasOne(p => p.SnapGame)
                .WithMany(r => r.PlayersData);
        }
    }
}
