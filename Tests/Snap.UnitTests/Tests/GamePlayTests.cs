using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Tests.Helpers;
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

        public GamePlayTests(IBackgroundHelper backgroundHelper,
            IPlayerServiceSeedHelper playerHelperService,
            IDealer dealer)
        {
            _backgroundHelper = backgroundHelper;
            _playerHelperService = playerHelperService;
            _dealer = dealer;
        }

        private readonly IBackgroundHelper _backgroundHelper;
        private readonly IPlayerServiceSeedHelper _playerHelperService;
        private readonly IDealer _dealer;

        [Fact]
        public async Task When_first_player_play_then_central_pile_should_have_KING_TILE()
        {
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);
            await _playerHelperService.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);
            var r = (await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None));
            r.PlayerTurn.SnapGame
                .CentralPile
                .Last
                .Card.ShouldBe(Card.KING_TILE);
        }

        [Fact]
        public async Task When_first_player_play_then_the_pop_card_should_be_KING_TILE()
        {
            //Background or When
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);
            await _playerHelperService.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);

            //When
            var gameplay = await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);

            //Then
            gameplay.Card.ShouldBe(Card.KING_TILE);
        }
    }
}