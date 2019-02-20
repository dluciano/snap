﻿using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public sealed class StackNode : IEntity
    {
        public int Id { get; set; }

        public Card Value { get; set; }
        public StackNode Previous { get; set; }
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
    }
}