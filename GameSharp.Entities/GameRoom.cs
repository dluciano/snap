using System.Collections.Generic;
using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public class GameRoom : IEntity
    {
        public int Id { get; set; }

        public ICollection<GameRoomPlayer> RoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public ICollection<GameData> GameDatas { get; } = new HashSet<GameData>();
    }
}