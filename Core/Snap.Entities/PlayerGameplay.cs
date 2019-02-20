using Dawlin.Abstract.Entities;
using GameSharp.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class PlayerGameplay : IEntity
    {
        public int Id { get; set; }

        public Card Card { get; set; }
        public PlayerTurn PlayerTurn { get; set; }
    }
}