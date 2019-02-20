using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class CardPileNode : IEntity
    {
        public int Id { get; set; }

        public Card Card { get; set; }
        public CardPileNode Previous { get; set; }
        public ICollection<GameRoom> CentralPiles { get; } = new HashSet<GameRoom>();
    }
}