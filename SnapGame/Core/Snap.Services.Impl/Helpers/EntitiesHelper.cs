using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Snap.Entities;

namespace Snap.Services.Impl.Helpers
{
    internal static class EntitiesHelper
    {
        public static IIncludableQueryable<SnapGame, StackNode> IncludeAll(this DbSet<SnapGame> repo) =>
            repo
                .Include(g => g.CentralPile.Last)
                .ThenInclude(p => p.Previous)

                .Include(g => g.GameData)
                .ThenInclude(gd => gd.CurrentTurn)
                .ThenInclude(pt => pt.Next)
                .Include(p => p.GameData.CurrentTurn.Player)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.PlayerTurn)
                .ThenInclude(pt => pt.Player)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.PlayerTurn)
                .ThenInclude(pt => pt.Next)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.StackEntity.Last)
                .ThenInclude(n => n.Previous);
    }
}
