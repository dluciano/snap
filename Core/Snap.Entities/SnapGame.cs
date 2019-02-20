using System;
using System.Collections.Generic;
using System.Linq;
using Dawlin.Abstract.Entities;
using GameSharp.Entities;

namespace Snap.Entities
{
    public class SnapGame : IEntity
    {
        public int Id { get; set; }
        public GameData GameData { get; set; }
        public StackEntity CentralPile { get; set; }
        public ICollection<PlayersData> PlayersData { get; } = new HashSet<PlayersData>();

        public PlayersData CurrentTurn =>
            PlayersData.Single(p => p.PlayerTurn.Id == GameData.CurrentTurn.Id);
    }
}
