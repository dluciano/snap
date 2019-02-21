using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class GamePlayTests
    {
        [Fact]
        public async Task When_first_player_play_then_the_pop_card_should_be_KING_TILE()
        {
            throw new NotImplementedException();
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();
                var game = await service.CreateAsync(CancellationToken.None);
                game = await service.StarGameAsync(game, CancellationToken.None);
                var gameplay = await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                gameplay.Card.ShouldBe(Card.KING_TILE);
            }
        }

        [Fact]
        public async Task When_first_player_play_then_central_pile_should_have_KING_TILE()
        {
            throw new NotImplementedException();
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();

                var game = await service.CreateAsync(CancellationToken.None);
                game = await service.StarGameAsync(game, CancellationToken.None);
                (await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None))
                    .PlayerTurn
                    .SnapGame
                    .CentralPile
                    .Last
                    .Card.ShouldBe(Card.KING_TILE);
            }
        }
    }
}
