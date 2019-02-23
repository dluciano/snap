using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;
using Snap.Fakes;

namespace Snap.ConsoleApplication
{
    internal static class Helpers
    {
        public static async Task<Player> SetCurrentPlayer(this IFakePlayerProvider provider, SnapGame game) =>
            await provider
                .SetCurrentPlayer(dbPlayers => Task.FromResult(game.CurrentTurn.PlayerTurn.Player));
    }
}
