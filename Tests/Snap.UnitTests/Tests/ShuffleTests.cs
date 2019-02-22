using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Tests.Module;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class ShuffleTests
    {
        public ShuffleTests()
        {
        }

        public ShuffleTests(ICardShuffler shuffleService,
            BackgroundHelper backgroundHelper)
        {
            _shuffleService = shuffleService;
            _backgroundHelper = backgroundHelper;
        }

        private readonly ICardShuffler _shuffleService;
        private readonly BackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_shuffle_with_two_player_then_only_two_players_should_exists()
        {
            //Background or When
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);

            //Then
            game.PlayersData.Count.ShouldBe(2);
            game.ShouldSatisfyAllConditions(
                () => game.PlayersData.Select(pd => pd.PlayerTurn.Player.Username)
                    .ShouldContain(PlayerServiceSeedHelper.FirstPlayerUsername),
                () => game.PlayersData.Select(pd => pd.PlayerTurn.Player.Username)
                    .ShouldContain(PlayerServiceSeedHelper.SecondPlayerUsername));
        }

        [Fact]
        public void When_shuffleling_cards_should_be_52()
        {
            _shuffleService.ShuffleCards().Count().ShouldBe(Enum.GetValues(typeof(Card)).Length);
        }

        [Fact]
        public void When_shuffleling_cards_should_be_unique()
        {
            _shuffleService.ShuffleCards().ShouldBeUnique();
        }
    }
}