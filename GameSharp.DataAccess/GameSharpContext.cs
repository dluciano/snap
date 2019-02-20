using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.DataAccess
{
    public abstract class GameSharpContext : DbContext
    {
        public DbSet<GameData> GameDatas { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<GameRoomPlayer> GameRoomPlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerTurn> PlayerTurns { get; set; }

        public GameSharpContext()
        {
        }
        public GameSharpContext(DbContextOptions options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
                .HasOne(p => p.GameRoom)
                .WithMany(r => r.GameDatas);
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.GameRoom)
                .WithMany(r => r.RoomPlayers);
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.Player)
                .WithMany(r => r.GameRoomPlayers);
            modelBuilder
                .Entity<PlayerTurn>()
                .HasOne(p => p.Player)
                .WithMany(r => r.PlayerTurns);
        }
    }
}
