using System.Collections.Generic;
using System.Linq;
using Dawlin.Abstract.Entities;

namespace Snap.Entities
{
    public class SnapGame : IEntity
    {
        public SnapGameData GameData { get; set; }
        public StackEntity CentralPile { get; set; }
        public ICollection<PlayerData> PlayersData { get; } = new HashSet<PlayerData>();

        public PlayerData CurrentTurn =>
            PlayersData.Single(p => p.PlayerTurn.Id == GameData.CurrentTurn.Id);

        public int Id { get; set; }
    }
}