using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class StackEntity
        : IEnumerable<StackNode>
    {
        public StackNode Last { get; set; }

        public IEnumerator<StackNode> GetEnumerator()
        {
            var last = Last;
            while (last != null)
            {
                yield return last;
                last = last.Previous;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Push(Card value)
        {
            Last = new StackNode
            {
                Previous = Last,
                Card = value
            };
        }

        public Card? PopCard()
        {
            if (Last == null)
                return null;
            var aux = Last;
            Last = Last.Previous;
            return aux.Card;
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(s => Enum.GetName(typeof(Card), s.Card)));
        }
    }
}