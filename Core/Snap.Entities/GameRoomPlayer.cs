using Dawlin.Abstract.Entities;

namespace Snap.Entities
{
    public class GameRoomPlayer : IEntity
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public bool IsViewer { get; set; }
        public GameRoom GameRoom { get; set; }
    }
}