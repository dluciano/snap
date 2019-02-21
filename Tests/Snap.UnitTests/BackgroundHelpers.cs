using System;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DI;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Tests.Module;

namespace Snap.Tests
{
    internal static class BackgroundHelpers
    {
        public static async Task<SnapGame> CreateGameAsync(this SnapModuleManager module)
        {
            var service = module.GetService<ISnapGameServices>();
            await module.SeedAndLoginFirstAsync();
            return await service.CreateAsync(CancellationToken.None);
        }

        public static async Task<GameRoomPlayer> SecondPlayerJoin(
            this SnapModuleManager module, SnapGame game)
        {
            await module.SeedAndLoginSecondPlayer();
            var roomService = module.GetService<IGameRoomPlayerServices>();
            return await roomService.AddPlayersAsync(game.GameData.GameRoom, false, CancellationToken.None);
        }

        public static async Task<SnapGame> GameStart(
            this SnapModuleManager module, SnapGame game)
        {
            var service = module.GetService<ISnapGameServices>();
            await module.LoginFirstPlayer();

            return await service.StarGameAsync(game, CancellationToken.None);
        }
    }
}
