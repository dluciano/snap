using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities.Enums;
using Shouldly;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Impl.Notifications;
using Snap.Tests.Helpers;
using Xunit;
using Xunit.Ioc.Autofac;

namespace Snap.Tests.Tests
{
    [UseAutofacTestFramework]
    public class NotificationsTests
    {
        public NotificationsTests()
        {

        }

        public NotificationsTests(IBackgroundHelper backgroundHelper,
            IPlayerServiceSeedHelper playerServiceSeedHelper,
            IDealer dealer,
            INotificationService notifier)
        {
            _backgroundHelper = backgroundHelper;
            _playerServiceSeedHelper = playerServiceSeedHelper;
            _dealer = dealer;
            _notifier = notifier;
        }

        private readonly IBackgroundHelper _backgroundHelper;
        private readonly IPlayerServiceSeedHelper _playerServiceSeedHelper;
        private readonly IDealer _dealer;
        private readonly INotificationService _notifier;

        [Fact]
        private async Task When_a_game_is_started_players_should_be_notified_with_the_game_stared_state()
        {
            var notified = false;
            _notifier.GameStartEvent += (sender, e, token) =>
            {
                notified = true;
                e.Game.GameData.CurrentState.ShouldBe(GameState.PLAYING);
                return Task.CompletedTask;
            };

            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room, PlayerServiceSeedHelper.SecondPlayerUsername);
            await _backgroundHelper.StartGameAsync(room);
            notified.ShouldBeTrue();
        }

        [Fact]
        private async Task When_first_player_pop_the_KING_TILE_then_all_players_should_be_notified()
        {
            var notified = false;
            PlayerGameplay gameplay = null;
            _notifier.CardPopEvent += (sender, e, token) =>
            {
                gameplay = e.GamePlay;
                notified = true;
                gameplay.Card.ShouldBe(Card.KING_TILE);
                return Task.CompletedTask;
            };
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room, PlayerServiceSeedHelper.SecondPlayerUsername);
            var game = await _backgroundHelper.StartGameAsync(room);
            await _playerServiceSeedHelper.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);
            await _dealer.PopCurrentPlayerCardAsync(game.Id, CancellationToken.None);
            notified.ShouldBeTrue();

            gameplay.ShouldNotBeNull();
        }

        [Fact]
        private async Task When_player_2_pop_then_player_1_should_be_notified_as_the_current_player()
        {
            var notified = false;
            PlayerGameplay gameplay = null;
            _notifier.CardPopEvent += (sender, e, token) =>
            {
                gameplay = e.GamePlay;
                notified = true;
                e.NextPlayer.PlayerTurn.Player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
                return Task.CompletedTask;
            };
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room, PlayerServiceSeedHelper.SecondPlayerUsername);
            var game = await _backgroundHelper.StartGameAsync(room);
            await _playerServiceSeedHelper.LoginPlayerAsync(PlayerServiceSeedHelper.SecondPlayerUsername);
            await _dealer.PopCurrentPlayerCardAsync(game.Id, CancellationToken.None);

            notified.ShouldBeTrue();
            gameplay.ShouldNotBeNull();
            notified.ShouldBeTrue();
        }
    }
}