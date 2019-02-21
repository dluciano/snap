using System;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class GameStateTest
    {
        [Fact]
        public async Task When_create_game_state_should_be_awaiting_player()
        {
            using (var module = await (await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                (await service.CreateAsync(CancellationToken.None))
                    .GameData
                    .CurrentState
                    .ShouldBe(GameState.AWAITING_PLAYERS);
            }
        }

        [Fact]
        public async Task When_game_started_then_game_should_be_in_state_playing()
        {
            using (var module = await (await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
                .SeedAndLoginFirstAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None));
                (await service.StarGameAsync(game, CancellationToken.None))
                    .GameData
                    .CurrentState
                    .ShouldBe(GameState.PLAYING);
            }
        }

        [Fact]
        public async Task When_restarting_the_game_then_it_should_throw_an_error()
        {
            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var joinService = module.GetService<ISnapGameServices>();
                var game = await service.CreateAsync(CancellationToken.None);

                game = await service.StarGameAsync(game, CancellationToken.None);
                throw new NotImplementedException();
                game = await service.StarGameAsync(game, CancellationToken.None);
            }
        }
    }
}
