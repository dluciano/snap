using System;
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
    public class TurnsTests
    {
        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None));
                (await service.StarGameAsync(game, CancellationToken.None))
                    .GameData
                    .PlayerTurns
                    .Select(p => p.Player.Username)
                    .ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_game_start_with_2_player_then_player_2_should_be_the_current_player()
        {
            throw new NotImplementedException();

            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None));
                (await service.StarGameAsync(game, CancellationToken.None))
                    .CurrentTurn
                    .PlayerTurn
                    .Player
                    .Username
                    .ShouldBe(PlayerServiceSeedHelper.SecondPlayerUsername);
            }
        }
    }
}