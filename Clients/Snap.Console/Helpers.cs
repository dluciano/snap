using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Snap.DI;
using Snap.Entities;
using Snap.Fakes;

namespace Snap.ConsoleApplication
{
    internal static class Helpers
    {
        public static async Task<Player> SetCurrentPlayer(this SnapModuleManager module, SnapGame game) =>
            await module.GetService<IFakePlayerService>()
                .SetCurrentPlayer(dbPlayers => Task.FromResult(game.CurrentTurn.PlayerTurn.Player));
    }
}
