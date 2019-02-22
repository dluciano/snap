using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Snap.Tests.Module;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class GameCreationTests
    {
        public GameCreationTests()
        {
        }

        public GameCreationTests(BackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly BackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            //When
            var player = (await _backgroundHelper.CreateGameAsync())
                .GameData
                .GameRoom
                .RoomPlayers
                .Select(r => r.Player)
                .SingleOrDefault();
            //Then
            player.ShouldNotBeNull();
            player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
        }

        [Fact]
        public async Task When_create_game_its_first_player_should_not_be_a_viewer()
        {
            //When
            var player = (await _backgroundHelper.CreateGameAsync())
                .GameData
                .GameRoom
                .RoomPlayers
                .SingleOrDefault();

            //Then
            player.ShouldNotBeNull();
            player.IsViewer.ShouldBeFalse();
        }
    }
}