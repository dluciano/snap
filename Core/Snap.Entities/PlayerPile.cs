using System;
using System.Collections.Generic;
using System.Text;
using GameSharp.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public sealed class PlayerPile
    {
        public StackEntity StackEntity { get; set; }
        public ICollection<PlayerGameplay> PlayerGameplay { get; } = new HashSet<PlayerGameplay>();
        public PlayerTurn PlayerTurn { get; set; }

        public void Snap(StackNode centralPileLast)
        {
            if (StackEntity == null)
                return;
            var first = StackEntity.LastNode;
            while (first.Previous != null) first = first.Previous;
            first.Previous = centralPileLast;
        }
    }
}
