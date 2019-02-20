using System.Collections.Generic;
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
    }
}
