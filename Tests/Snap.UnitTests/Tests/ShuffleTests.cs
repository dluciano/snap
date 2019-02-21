using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class ShuffleTests
    {
        [Fact]
        public async Task When_shuffleling_cards_should_be_unique()
        {
            using (var module = await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<IDealer>();
                service.ShuffleCards().ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_shuffleling_cards_should_be_52()
        {
            using (var module = await TestModuleHelpers
                .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<IDealer>();
                service.ShuffleCards().Count().ShouldBe(Enum.GetValues(typeof(Card)).Length);
            }
        }

        [Fact]
        public async Task When_shuffle_with_two_player_then_only_two_players_should_exists()
        {
            using (var module = await TestModuleHelpers.CreateAndBuildWithDefaultsAsync())
            {
                //Background or When
                var game = await module.CreateGameAsync();
                await module.SecondPlayerJoin(game);
                game = await module.GameStart(game);

                //Then
                game.PlayersData.Count.ShouldBe(2);
                game.ShouldSatisfyAllConditions(
                    () => game.PlayersData.Select(pd => pd.PlayerTurn.Player.Username).ShouldContain(PlayerServiceSeedHelper.FirstPlayerUsername),
                    () => game.PlayersData.Select(pd => pd.PlayerTurn.Player.Username).ShouldContain(PlayerServiceSeedHelper.SecondPlayerUsername));
            }
        }
    }
}