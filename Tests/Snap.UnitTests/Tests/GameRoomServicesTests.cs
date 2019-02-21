using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Fakes;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public sealed class GameRoomServicesTests
    {
        [Fact]
        public async Task When_player_2_join_then_two_player_should_exists()
        {
            using (var module = await (await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var roomService = module.GetService<IGameRoomPlayerServices>();

                var game = await service.CreateAsync(CancellationToken.None);
                await module.SeedAndLoginSecondPlayer();

                var gameRoomPlayer = await roomService.AddPlayersAsync(game.GameData.GameRoom, false, CancellationToken.None);
                gameRoomPlayer.GameRoom.RoomPlayers.Count.ShouldBe(2);
                gameRoomPlayer.GameRoom
                    .RoomPlayers.Select(p => p.IsViewer)
                    .ShouldAllBe(p => p == false);
            }
        }

        [Fact]
        public async Task When_player_2_join_as_viewer_then_two_player_should_exists_and_player_2_should_be_viewer()
        {
            using (var module = await (await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var roomService = module.GetService<IGameRoomPlayerServices>();

                var game = await service.CreateAsync(CancellationToken.None);
                await module.SeedAndLoginSecondPlayer();

                var gameRoomPlayer = await roomService.AddPlayersAsync(game.GameData.GameRoom, true, CancellationToken.None);
                gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                    p.Player.Username == PlayerServiceSeedHelper.FirstPlayerUsername).IsViewer.ShouldBeFalse();
                gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                    p.Player.Username == PlayerServiceSeedHelper.SecondPlayerUsername).IsViewer.ShouldBeTrue();
            }
        }
    }
}
