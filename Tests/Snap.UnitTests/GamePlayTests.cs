using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Snap.Services.Abstract;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Entities.Enums;

namespace Snap.Tests
{
    public class GamePlayTests
    {
        [Fact]
        public async Task When_first_player_play_then_the_pop_card_should_be_KING_TILE()
        {
            var firstPlayer = new Player() { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await new ModuleManager()
                .ConfigureDefault()
                .WithFakePlayerRandomizer(players)
                .WithFakeCardRandomizer(fakedRandomCards)
                .WithFakePlayerService(firstPlayer)
                .WithFakeDealter()
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();

                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);
                var gameplay = await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                gameplay.Card.ShouldBe(Card.KING_TILE);
            }
        }

        [Fact]
        public async Task When_first_player_play_then_central_pile_should_have_KING_TILE()
        {
            var firstPlayer = new Player() { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await new ModuleManager()
                .ConfigureDefault()
                .WithFakePlayerRandomizer(players)
                .WithFakeCardRandomizer(fakedRandomCards)
                .WithFakePlayerService(firstPlayer)
                .WithFakeDealter()
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();

                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);
                var gameplay = await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                game.CentralPile.Last.Card.ShouldBe(Card.KING_TILE); //TODO: the game can be retrieve from a service
            }
        }
    }
}
