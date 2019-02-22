using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Snap.Entities.Enums;
using Snap.Fakes;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class DealtTests
    {
        public DealtTests()
        {
        }

        public DealtTests(BackgroundHelper backgroundHelper,
            IFakePlayerService playerService)
        {
            _backgroundHelper = backgroundHelper;
            _playerService = playerService;
        }

        private readonly BackgroundHelper _backgroundHelper;
        private readonly IFakePlayerService _playerService;

        [Fact]
        public async Task When_dealting_the_cards_every_player_should_have_a_proportional_amount_of_cards()
        {
            //Background or When
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);

            //Then
            var playersStacks = game.PlayersData.Select(p => p.StackEntity).ToList();
            var cardsPerPlayer = Enum.GetValues(typeof(Card)).Length
                                 / _playerService.GetPlayers().Count();
            playersStacks.ShouldAllBe(p => p.Count() == cardsPerPlayer);
        }

        [Fact]
        public async Task When_dealting_then_none_of_the_cards_should_be_repeated()
        {
            //Background or When
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);

            //Then
            game.PlayersData
                .SelectMany(p => p.StackEntity)
                .Select(s => s.Card).ShouldBeUnique();
        }

        [Fact]
        public async Task When_dealting_then_the_cards_of_each_player_should_not_be_repeated()
        {
            //Background or When
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);

            //Then
            game.PlayersData
                .Select(p => p.StackEntity)
                .ToList().ForEach(playerStack => { playerStack.ToList().Select(s => s.Card).ShouldBeUnique(); });
        }
    }
}