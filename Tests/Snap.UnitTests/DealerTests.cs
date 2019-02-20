using System.Linq;
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
    public class DealerTests
    {
        [Fact]
        public void When_shuffle_the_cards_should_not_be_repeated() { }
        [Fact]
        public void When_choosing_the_turns_should_not_be_repeated() { }
        [Fact]
        public void When_dealting_the_cards_should_not_be_repeated() { }
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
