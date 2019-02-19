namespace Snap.Entities
{
    public class GameRoomPlayer
    {
        public Player Player { get; internal set; }
        public bool IsViewer { get; internal set; }
        public GameSession GameSession { get; internal set; }
    }
}