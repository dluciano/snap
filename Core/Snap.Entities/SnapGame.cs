using System;
using System.Text;
using Dawlin.Abstract.Entities;
using Dawlin.Util;
using GameSharp.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class SnapGame : IEntity, ITransitable<GameState>
    {
        public int Id { get; set; }
        public GameData GameData { get; set; }

        public GameState From { get; set; } = GameState.NONE;
        public StackEntity CentralPile { get; set; }
    }
}
