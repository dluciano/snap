using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Xunit;

namespace Snap.Tests
{
    public class DealerTests
    {
        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                (await service.StarGame(game, CancellationToken.None))
                    .GameData
                    .PlayerTurns
                    .Select(p => p.Player.Username)
                    .ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_of_each_player_should_not_be_repeated()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                game = await service.StarGame(game, CancellationToken.None);
                game.PlayersData
                    .Select(p => p.StackEntity)
                    .ToList().ForEach(playerStack =>
                {
                    playerStack.ToList().Select(s => s.Card).ShouldBeUnique();
                });
            }
        }

        [Fact]
        public async Task When_dealting_none_of_the_cards_should_be_repeated()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                game = await service.StarGame(game, CancellationToken.None);
                game.PlayersData
                    .SelectMany(p => p.StackEntity)
                    .Select(s => s.Card).ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_every_player_should_have_same_amount()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                game = await service.StarGame(game, CancellationToken.None);
                var playersStacks = game.PlayersData.Select(p => p.StackEntity).ToList();
                var cardsOfFirstPlayer = playersStacks.First().Count();
                playersStacks.ShouldAllBe(p => p.Count() == cardsOfFirstPlayer);
            }
        }

        [Fact]
        public async Task When_shuffleling_cards_should_be_unique()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<IDealer>();
                service.ShuffleCards().ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_shuffleling_cards_should_be_52()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<IDealer>();
                service.ShuffleCards().Count().ShouldBe(Enum.GetValues(typeof(Card)).Length);
            }
        }

        [Fact]
        public async Task When_shuffle_gamer_with_two_player_then_only_two_players_should_exists()
        {
            using (var module = await ModuleHelper.CreateAndBuildWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var players = new[]{ new Player { Username = "Player 2" },
                    new Player { Username = "Player 1" }};

                var game = (await service.CreateAsync(CancellationToken.None,
                        players));
                game = await service.StarGame(game, CancellationToken.None);
                game.PlayersData.Count.ShouldBe(2);
                game.PlayersData.Select(pd => pd.PlayerTurn.Player).ShouldContain(players[0]);
                game.PlayersData.Select(pd => pd.PlayerTurn.Player).ShouldContain(players[1]);
            }
        }
    }
}
