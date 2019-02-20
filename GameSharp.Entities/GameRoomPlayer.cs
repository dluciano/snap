using Dawlin.Abstract.Entities;

namespace GameSharp.Entities
{
    public sealed class GameRoomPlayer : IEntity
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public bool IsViewer { get; set; }
        public GameRoom GameRoom { get; set; }
    }
}