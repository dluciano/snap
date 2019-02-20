using Microsoft.EntityFrameworkCore;
using Snap.Entities;

namespace Snap.DataAccess
{
    public class SnapDbContext : DbContext
    {
        public DbSet<CardPileNode> CardPileNodes { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<GameRoomPlayer> GameRoomPlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerGameplay> PlayerGamePlays { get; set; }
        public DbSet<PlayerTurn> PlayerTurns { get; set; }

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
                .Entity<GameRoom>()
                .HasOne(p => p.FirstPlayer)
                .WithMany(r => r.FirstPlayers);
            modelBuilder
                .Entity<GameRoom>()
                .HasOne(p => p.CurrentTurn)
                .WithMany(r => r.CurrentTurns);
            modelBuilder
                .Entity<GameRoom>()
                .HasOne(p => p.CentralPileLast)
                .WithMany(r => r.CentralPiles);
        }
    }
}
