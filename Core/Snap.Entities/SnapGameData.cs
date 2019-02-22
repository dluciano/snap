using System.Collections.Generic;
using GameSharp.Entities;

namespace Snap.Entities
{
    public class SnapGameData : GameData
    {
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
    }
}