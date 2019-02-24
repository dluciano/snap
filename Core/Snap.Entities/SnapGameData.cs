using System.Collections.Generic;
using GameSharp.Entities;

namespace Snap.Entities
{
    public class SnapGameData : GameData
    {
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
        public GameRoom Room { get; set; }
        public int GameRoomId { get; set; }
        public ICollection<PlayerGameplay> PlayerGamePlays { get; } = new HashSet<PlayerGameplay>();
    }
}