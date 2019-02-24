using System.Collections.Generic;
using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public class PlayerTurn : IEntity
    {
        public Player Player { get; set; }
        public PlayerTurn Next { get; set; }

        public ICollection<GameData> FirstPlayers { get; } = new HashSet<GameData>();
        public ICollection<GameData> CurrentTurns { get; } = new HashSet<GameData>();
        public int Id { get; set; }
    }
}