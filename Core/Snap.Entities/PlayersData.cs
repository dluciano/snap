﻿using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using GameSharp.Entities;

namespace Snap.Entities
{
    public sealed class PlayersData : IEntity
    {
        public int Id { get; set; }

        public StackEntity StackEntity { get; set; }
        public ICollection<PlayerGameplay> PlayerGameplay { get; } = new HashSet<PlayerGameplay>();
        public PlayerTurn PlayerTurn { get; set; }
        public SnapGame SnapGame { get; set; }

        public void Snap(StackNode centralPileLast)
        {
            if (StackEntity == null)
                return;
            var first = StackEntity.Last;
            while (first.Previous != null) first = first.Previous;
            first.Previous = centralPileLast;
        }
    }
}