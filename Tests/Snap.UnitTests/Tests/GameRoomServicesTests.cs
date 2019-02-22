using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Services;
using GameSharp.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Fakes;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public sealed class GameRoomServicesTests
    {
        private readonly BackgroundHelper _backgroundHelper;
        private readonly IFakePlayerService _playerService;
        private readonly PlayerServiceSeedHelper _playerHelperService;
        private readonly IDealer _dealer;
        private readonly IGameRoomPlayerServices _gameRoomPlayerServices;

        public GameRoomServicesTests()
        {

        }

        public GameRoomServicesTests(BackgroundHelper backgroundHelper,
            IFakePlayerService playerService,
            PlayerServiceSeedHelper playerHelperService,
            IDealer dealer,
            IGameRoomPlayerServices gameRoomPlayerServices)
        {
            _backgroundHelper = backgroundHelper;
            _playerService = playerService;
            _playerHelperService = playerHelperService;
            _dealer = dealer;
            _gameRoomPlayerServices = gameRoomPlayerServices;
        }

        [Fact]
        public async Task When_player_2_join_then_two_player_should_exists()
        {
            //Background
            var game = await _backgroundHelper.CreateGameAsync();

            //When
            var roomPlayer = await _backgroundHelper.PlayerJoinAsync(game);

            //Then
            roomPlayer.GameRoom.RoomPlayers.Count.ShouldBe(2);
            roomPlayer.GameRoom
                .RoomPlayers.Select(p => p.IsViewer)
                .ShouldAllBe(p => p == false);
        }

        [Fact]
        public async Task When_player_2_join_as_viewer_then_two_player_should_exists_and_player_2_should_be_viewer()
        {
            //Background
            var game = await _backgroundHelper.CreateGameAsync();
            await _playerHelperService.SeedAndLoginAsync(PlayerServiceSeedHelper.SecondPlayerUsername);

            //When
            var gameRoomPlayer = await _gameRoomPlayerServices
                .AddPlayersAsync(game.GameData.GameRoom, true, CancellationToken.None);

            //Then
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.FirstPlayerUsername).IsViewer.ShouldBeFalse();
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.SecondPlayerUsername).IsViewer.ShouldBeTrue();
        }
    }
}
