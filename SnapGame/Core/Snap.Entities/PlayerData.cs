﻿using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using GameSharp.Entities;

namespace Snap.Entities
{
    public class PlayerData : IEntity
    {
        public StackEntity StackEntity { get; set; } = new StackEntity();
        public ICollection<PlayerGameplay> PlayerGameplay { get; } = new HashSet<PlayerGameplay>();
        public PlayerTurn PlayerTurn { get; set; }
        public SnapGame SnapGame { get; set; }
        public int Id { get; set; }

       
    }
}