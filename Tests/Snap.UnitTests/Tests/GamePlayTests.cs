using System.Threading;
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
    public sealed class GamePlayTests
    {
        public GamePlayTests()
        {
        }

        public GamePlayTests(BackgroundHelper backgroundHelper,
            PlayerServiceSeedHelper playerHelperService,
            IDealer dealer)
        {
            _backgroundHelper = backgroundHelper;
            _playerHelperService = playerHelperService;
            _dealer = dealer;
        }

        private readonly BackgroundHelper _backgroundHelper;
        private readonly PlayerServiceSeedHelper _playerHelperService;
        private readonly IDealer _dealer;

        [Fact]
        public async Task When_first_player_play_then_central_pile_should_have_KING_TILE()
        {
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);
            await _playerHelperService.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);
            var r = (await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None));
                r.PlayerTurn
                .SnapGame
                .CentralPile
                .Last
                .Card.ShouldBe(Card.KING_TILE);
        }

        [Fact]
        public async Task When_first_player_play_then_the_pop_card_should_be_KING_TILE()
        {
            //using (var module = await new SnapModuleManager()
            //    .WithFakeCardRandomizer(fakedRandomCards)
            //    .WithFakeDealter()
            //    .WithFakeSecondPlayerFirstRandomizer()
            //    .BuildWithDefaultsAsync())
            //Background or When
            var game = await _backgroundHelper.CreateGameAsync();
            await _backgroundHelper.PlayerJoinAsync(game);
            game = await _backgroundHelper.StartGameAsync(game);
            await _playerHelperService.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);

            //When
            var gameplay = await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);

            //Then
            gameplay.Card.ShouldBe(Card.KING_TILE);
        }
    }
}