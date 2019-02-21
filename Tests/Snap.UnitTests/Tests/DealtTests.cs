using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Entities.Enums;
using Snap.Fakes;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class DealtTests
    {
        [Fact]
        public async Task When_dealting_then_the_cards_of_each_player_should_not_be_repeated()
        {
            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None));
                (await service.StarGameAsync(game, CancellationToken.None))
                    .PlayersData
                    .Select(p => p.StackEntity)
                    .ToList().ForEach(playerStack =>
                {
                    playerStack.ToList().Select(s => s.Card).ShouldBeUnique();
                });
            }
        }

        [Fact]
        public async Task When_dealting_then_none_of_the_cards_should_be_repeated()
        {
            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None));
                game = await service.StarGameAsync(game, CancellationToken.None);
                game.PlayersData
                    .SelectMany(p => p.StackEntity)
                    .Select(s => s.Card).ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_every_player_should_have_a_proportional_amount_of_cards()
        {
            using (var module = await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var fakePlayerService = module.GetService<IFakePlayerService>();
                var game = (await service.CreateAsync(CancellationToken.None));
                game = await service.StarGameAsync(game, CancellationToken.None);
                var playersStacks = game.PlayersData.Select(p => p.StackEntity).ToList();
                var cardsPerPlayer = Enum.GetValues(typeof(Card)).Length
                                     / (await fakePlayerService.GetPlayers()).Count();
                playersStacks.ShouldAllBe(p => p.Count() == cardsPerPlayer);
            }
        }
    }
}
