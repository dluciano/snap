using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class GameCreationTest
    {
        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            using (var module = await (await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var player = (await module.GetService<ISnapGameServices>().CreateAsync(CancellationToken.None))
                    .GameData.GameRoom.RoomPlayers.Select(r => r.Player).SingleOrDefault();
                player.ShouldNotBeNull();
                player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
            }
        }

        [Fact]
        public async Task When_create_game_its_first_player_should_not_be_a_viewer()
        {
            using (var module = await (await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var player = (await module.GetService<ISnapGameServices>().CreateAsync(CancellationToken.None))
                    .GameData.GameRoom.RoomPlayers.SingleOrDefault();
                player.ShouldNotBeNull();
                player.IsViewer.ShouldBeFalse();
            }
        }
    }
}
