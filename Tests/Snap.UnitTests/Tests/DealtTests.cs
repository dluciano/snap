using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Entities;
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
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                game.PlayersData
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
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                game.PlayersData
                    .SelectMany(p => p.StackEntity)
                    .Select(s => s.Card).ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_every_player_should_have_a_proportional_amount_of_cards()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                var fakePlayerService = module.GetService<IFakePlayerService>();
                var playersStacks = game.PlayersData.Select(p => p.StackEntity).ToList();
                var cardsPerPlayer = Enum.GetValues(typeof(Card)).Length
                                     / (await fakePlayerService.GetPlayers()).Count();
                playersStacks.ShouldAllBe(p => p.Count() == cardsPerPlayer);
            }
        }
    }
}
