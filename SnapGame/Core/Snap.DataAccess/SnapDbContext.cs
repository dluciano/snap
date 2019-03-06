using GameSharp.DataAccess;
using Microsoft.EntityFrameworkCore;
using Snap.Entities;

namespace Snap.DataAccess
{
    public sealed class SnapDbContext : GameSharpContext
    {
        public SnapDbContext()
        {
        }

        public SnapDbContext(DbContextOptions<SnapDbContext> options) :
            base(options)
        {
        }

        public DbSet<PlayerGameplay> PlayerGamePlays { get; set; }
        public DbSet<PlayerData> PlayersData { get; set; }
        public DbSet<SnapGame> SnapGames { get; set; }
        public DbSet<StackNode> StackNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<SnapGame>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<SnapGame>()
                .OwnsOne(p => p.CentralPile);
            modelBuilder
                .Entity<SnapGame>()
                .Ignore(p => p.CurrentTurn);
            modelBuilder
                .Entity<SnapGame>()
                .HasOne(p => p.GameData)
                .WithMany(p => p.SnapGames);

            modelBuilder
                .Entity<SnapGameData>()
                .HasOne(p => p.Room)
                .WithOne(p => (SnapGameData)p.GamesData)
                .HasForeignKey<SnapGameData>(p => p.GameRoomId);

            modelBuilder
                .Entity<PlayerData>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<PlayerData>()
                .OwnsOne(p => p.StackEntity);
            modelBuilder
                .Entity<PlayerData>()
                .HasOne(p => p.SnapGame)
                .WithMany(r => r.PlayersData);

            modelBuilder
                .Entity<PlayerGameplay>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<PlayerGameplay>()
                .HasOne(p => p.GameData)
                .WithMany(p => p.PlayerGamePlays);

            modelBuilder
                .Entity<StackNode>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}