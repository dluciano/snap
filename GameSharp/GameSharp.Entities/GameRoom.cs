using System;
using System.Collections.Generic;
using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public class GameRoom : IEntity
    {
        public ICollection<GameRoomPlayer> RoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public int Id { get; set; }
        public bool CanJoin { get; set; }
        public Guid GameIdentifier { get; set; }
        public GameData GamesData { get; set; }
        public Player CreatedBy { get; set; }
    }
}