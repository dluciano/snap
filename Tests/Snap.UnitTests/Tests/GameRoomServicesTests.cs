using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Services.Abstract;
using Shouldly;
using Snap.Tests.Helpers;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public sealed class GameRoomServicesTests
    {
        public GameRoomServicesTests()
        {
        }

        public GameRoomServicesTests(IBackgroundHelper backgroundHelper,
            IPlayerServiceSeedHelper playerHelperService,
            IGameRoomPlayerServices gameRoomPlayerServices)
        {
            _backgroundHelper = backgroundHelper;
            _playerHelperService = playerHelperService;
            _gameRoomPlayerServices = gameRoomPlayerServices;
        }

        private readonly IBackgroundHelper _backgroundHelper;
        private readonly IPlayerServiceSeedHelper _playerHelperService;
        private readonly IGameRoomPlayerServices _gameRoomPlayerServices;

        [Fact]
        public async Task When_player_2_join_as_viewer_then_two_player_should_exists_and_player_2_should_be_viewer()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();
            await _playerHelperService.SeedAndLoginAsync(PlayerServiceSeedHelper.SecondPlayerUsername);

            //When
            var gameRoomPlayer = await _gameRoomPlayerServices
                .AddPlayersAsync(room, true, CancellationToken.None);

            //Then
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.FirstPlayerUsername).IsViewer.ShouldBeFalse();
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.SecondPlayerUsername).IsViewer.ShouldBeTrue();
        }

        [Fact]
        public async Task When_player_2_join_then_two_player_should_exists()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();

            //When
            var roomPlayer = await _backgroundHelper.PlayerJoinAsync(room);

            //Then
            roomPlayer.GameRoom.RoomPlayers.Count.ShouldBe(2);
            roomPlayer.GameRoom
                .RoomPlayers.Select(p => p.IsViewer)
                .ShouldAllBe(p => p == false);
        }
    }
}