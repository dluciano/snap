using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;
using Snap.DI;
using Snap.Fakes;

namespace Snap.Tests.Module
{
    internal static class PlayerServiceSeedHelper
    {
        public const string FirstPlayerUsername = "First Player";
        public const string SecondPlayerUsername = "Second Player";

        private static async Task<Player> SeedWithFirstPlayerAsync(this SnapModuleManager module)
        {
            var player = (await module.GetService<IFakePlayerService>()
                    .AddRangeAsync(FirstPlayerUsername))
                .Single();
            await module.GetService<SnapDbContext>().SaveChangesAsync();
            return player;
        }

        private static async Task<Player> SeedSecondPlayer(this SnapModuleManager module)
        {
            var player = (await module.GetService<IFakePlayerService>()
                    .AddRangeAsync(SecondPlayerUsername))
                .Single();
            await module.GetService<SnapDbContext>().SaveChangesAsync();
            return player;
        }

        public static async Task<SnapModuleManager> SeedAndLoginFirstAsync(this SnapModuleManager module)
        {
            await module.SeedWithFirstPlayerAsync();
            await LoginFirstPlayer(module);
            return module;
        }

        public static async Task<SnapModuleManager> LoginFirstPlayer(this SnapModuleManager module)
        {
            await module.GetService<IFakePlayerService>()
                .SetCurrentPlayer(async players => await players.SingleAsync(p => p.Username == FirstPlayerUsername));
            return module;
        }

        public static async Task<SnapModuleManager> SeedAndLoginSecondPlayer(this SnapModuleManager module)
        {
            await (module.SeedSecondPlayer());
            await module.GetService<IFakePlayerService>()
                .SetCurrentPlayer(async players => await players.SingleAsync(p => p.Username == SecondPlayerUsername));
            return module;
        }
    }
}
