using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public class GameRoomPlayer : IEntity
    {
        public Player Player { get; set; }
        public bool IsViewer { get; set; }
        public GameRoom GameRoom { get; set; }
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int RoomId { get; set; }
    }
}