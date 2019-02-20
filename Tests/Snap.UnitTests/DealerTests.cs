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
    public class DealerTests
    {
        //[Fact]
        //public void When_shuffle_the_cards_should_not_be_repeated() { }

        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                await service.StarGame(game, CancellationToken.None);
                game.GameData.PlayerTurns.Select(p => p.Player.Username).ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_of_each_player_should_not_be_repeated()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                await service.StarGame(game, CancellationToken.None);
                game.PlayersData
                    .Select(p => p.StackEntity)
                    .ToList().ForEach(playerStack =>
                {
                    playerStack.ToList().Select(s => s.Card).ShouldBeUnique();
                });
            }
        }

        [Fact]
        public async Task When_dealting_the_none_card_should_be_repeated()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                await service.StarGame(game, CancellationToken.None);
                game.PlayersData
                    .SelectMany(p => p.StackEntity)
                    .Select(s => s.Card).ShouldBeUnique();
            }
        }

        [Fact]
        public async Task When_dealting_the_cards_every_player_should_have_same_amountAsync()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }));
                await service.StarGame(game, CancellationToken.None);
                var playersStacks = game.PlayersData.Select(p => p.StackEntity).ToList();
                var cardsOfFirstPlayer = playersStacks.First().Count();
                playersStacks.ShouldAllBe(p => p.Count() == cardsOfFirstPlayer);
            }
        }
    }
}
