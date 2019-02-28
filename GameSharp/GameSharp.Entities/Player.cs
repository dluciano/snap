using System.Collections.Generic;
using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public class Player : IEntity
    {
        public string Username { get; set; }
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public ICollection<PlayerTurn> PlayerTurns { get; } = new HashSet<PlayerTurn>();
        public int Id { get; set; }
        public ICollection<GameRoom> CreatedRooms { get; } = new HashSet<GameRoom>();
    }
}