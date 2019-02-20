using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Services.Abstract;
using Xunit;

namespace Snap.Tests
{
    public class GameCreationTest
    {
        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var testPlayer = new Player { Username = "test" };
                var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                    .GameData.GameRoom.RoomPlayers.Select(r => r.Player).SingleOrDefault();
                player.ShouldNotBeNull();
                player.ShouldBe(testPlayer);
            }
        }

        [Fact]
        public async Task When_create_game_it_players_should_be_player_not_viewers()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var testPlayer = new Player { Username = "test" };
                var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                    .GameData.GameRoom.RoomPlayers.SingleOrDefault();
                player.ShouldNotBeNull();
                player.IsViewer.ShouldBeFalse();
            }
        }
    }
}
