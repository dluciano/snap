using System.Collections.Generic;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public sealed class StackEntity
    {
        public StackNode Last { get; set; }

        public void Push(Card value) =>
            Last = new StackNode
            {
                Previous = Last,
                Card = value
            };

        public Card? PopCard()
        {
            if (Last == null)
                return null;
            var aux = Last;
            Last = Last.Previous;
            return aux.Card;
        }
    }
}
