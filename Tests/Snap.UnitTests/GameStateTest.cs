using System.Threading;
using Shouldly;
using Xunit;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Snap.Services.Abstract;

namespace Snap.UnitTests
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
