using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;
using Snap.Fakes;

namespace Snap.ConsoleApplication
{
    internal static class Helpers
    {
        public static async Task<Player> SetCurrentPlayer(this IFakePlayerService service, SnapGame game) =>
            await service
                .SetCurrentPlayer(dbPlayers => Task.FromResult(game.CurrentTurn.PlayerTurn.Player));
    }
}
