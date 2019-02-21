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
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background
                var game = await module.CreateGameAsync();

                //When
                var roomPlayer = await module.SecondPlayerJoin(game);

                //Then
                roomPlayer.GameRoom.RoomPlayers.Count.ShouldBe(2);
                roomPlayer.GameRoom
                    .RoomPlayers.Select(p => p.IsViewer)
                    .ShouldAllBe(p => p == false);
            }
        }

        [Fact]
        public async Task When_player_2_join_as_viewer_then_two_player_should_exists_and_player_2_should_be_viewer()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background
                var game = await module.CreateGameAsync();
                await module.SeedAndLoginSecondPlayer();

                //When
                var gameRoomPlayer = await module.GetService<IGameRoomPlayerServices>()
                    .AddPlayersAsync(game.GameData.GameRoom, true, CancellationToken.None);

                //Then
                gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                    p.Player.Username == PlayerServiceSeedHelper.FirstPlayerUsername).IsViewer.ShouldBeFalse();
                gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                    p.Player.Username == PlayerServiceSeedHelper.SecondPlayerUsername).IsViewer.ShouldBeTrue();
            }
        }
    }
}
