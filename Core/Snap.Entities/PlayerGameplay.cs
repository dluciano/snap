using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class PlayerGameplay : IEntity
    {
        public Card Card { get; set; }
        public PlayerData PlayerTurn { get; set; }
        public int Id { get; set; }
    }
}