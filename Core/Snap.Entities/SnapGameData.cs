using System.Collections.Generic;
using GameSharp.Entities;

namespace Snap.Entities
{
    public sealed class SnapGameData : GameData
    {
        public ICollection<SnapGame> SnapGames { get; } = new HashSet<SnapGame>();
    }
}
