using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class GameCreationTest
    {
        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //When
                var player = (await module.CreateGameAsync())
                    .GameData
                    .GameRoom
                    .RoomPlayers
                    .Select(r => r.Player)
                    .SingleOrDefault();
                //Then
                player.ShouldNotBeNull();
                player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
            }
        }

        [Fact]
        public async Task When_create_game_its_first_player_should_not_be_a_viewer()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //When
                var player = (await module.CreateGameAsync())
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
}
