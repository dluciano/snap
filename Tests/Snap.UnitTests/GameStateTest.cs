using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Services.Abstract;
using Xunit;

namespace Snap.Tests
{
    public class GameStateTest
    {
        [Fact]
        public async Task When_create_game_state_should_be_awaiting_player()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }))
                    .GameData.From
                    .ShouldBe(GameState.AWAITING_PLAYERS);
            }
        }
    }
}
