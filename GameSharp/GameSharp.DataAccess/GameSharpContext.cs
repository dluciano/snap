using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.DataAccess
{
    public abstract class GameSharpContext : DbContext
    {
        protected GameSharpContext()
        {
        }

        protected GameSharpContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<GameData> GameDatas { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<GameRoomPlayer> GameRoomPlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerTurn> PlayerTurns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Player>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder
                .Entity<GameData>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameData>()
                .HasOne(p => p.FirstPlayer)
                .WithMany(r => r.FirstPlayers);
            modelBuilder
                .Entity<GameData>()
                .HasOne(p => p.CurrentTurn)
                .WithMany(r => r.CurrentTurns);
            modelBuilder
                .Entity<GameData>()
                .Ignore(p => p.Turns);

            modelBuilder
                .Entity<GameRoomPlayer>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.GameRoom)
                .WithMany(r => r.RoomPlayers)
                .HasForeignKey(p => p.RoomId);
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.Player)
                .WithMany(r => r.GameRoomPlayers)
                .HasForeignKey(r => r.PlayerId);
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasIndex(p => new { p.PlayerId, p.RoomId })
                .IsUnique();

            modelBuilder
                .Entity<PlayerTurn>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<PlayerTurn>()
                .HasOne(p => p.Player)
                .WithMany(r => r.PlayerTurns);

            modelBuilder
                .Entity<GameRoom>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameRoom>()
                .HasOne(p => p.CreatedBy)
                .WithMany(p=>p.CreatedRooms);
        }
    }
}