using System.Collections.Generic;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public sealed class StackEntity
    {
        public StackNode LastNode { get; set; }
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
        public void Push(Card value) =>
            LastNode = new StackNode
            {
                Previous = LastNode,
                Value = value
            };

        public Card? PopCard()
        {
            if (LastNode == null)
                return null;
            var aux = LastNode;
            LastNode = LastNode.Previous;
            return aux.Value;
        }
    }
}
