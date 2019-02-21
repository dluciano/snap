using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public sealed class StackNode : IEntity
    {
        public int Id { get; set; }

        public Card Card { get; set; }
        public StackNode Previous { get; set; }
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
        public ICollection<PlayersData> PlayersData { get; } = new HashSet<PlayersData>();

        public static StackNode Create(Card card, StackEntity stack) =>
            stack.Last = new StackNode
            {
                Card = card,
                Previous = stack.Last
            };
    }
}