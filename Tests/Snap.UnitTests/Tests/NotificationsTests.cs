﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities.Enums;
using Shouldly;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Impl.Notifications;
using Snap.Tests.Module;
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

        public NotificationsTests(BackgroundHelper backgroundHelper,
            IDealer dealer,
            INotificationService notifier)
        {
            _backgroundHelper = backgroundHelper;
            _dealer = dealer;
            _notifier = notifier;
        }

        private readonly BackgroundHelper _backgroundHelper;
        private readonly IDealer _dealer;
        private readonly INotificationService _notifier;

        [Fact]
        private async Task When_a_game_is_started_players_should_be_notified()
        {
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();
            var notified = false;
            _notifier.GameStartEvent += (sender, e) =>
            {
                notified = true;
                e.Game.GameData.CurrentState.ShouldBe(GameState.PLAYING);
            };
            var game = await _backgroundHelper.CreateGameAsync();
            game = await _backgroundHelper.StartGameAsync(game);
            await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
            notified.ShouldBeTrue();
        }

        [Fact]
        private async Task When_first_playe_pop_the_KING_TILE_then_all_players_should_be_notified()
        {
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();
            var notified = false;
            PlayerGameplay gameplay = null;
            _notifier.CardPopEvent += (sender, e) =>
            {
                gameplay = e.GamePlay;
                notified = true;
                gameplay.Card.ShouldBe(Card.KING_TILE);
            };
            var game = await _backgroundHelper.CreateGameAsync();
            game = await _backgroundHelper.StartGameAsync(game);
            await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
            gameplay.ShouldNotBeNull();
            notified.ShouldBeTrue();
        }

        [Fact]
        private async Task When_player_1_pop_the_KING_TILE_then_player_1_should_be_notified_as_the_current_player()
        {
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();
            var notified = false;
            PlayerGameplay gameplay = null;
            _notifier.CardPopEvent += (sender, e) =>
            {
                gameplay = e.GamePlay;
                notified = true;
                e.NextPlayer.PlayerTurn.Player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
            };
            var game = await _backgroundHelper.CreateGameAsync();
            game = await _backgroundHelper.StartGameAsync(game);
            await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
            gameplay.ShouldNotBeNull();
            notified.ShouldBeTrue();
        }
    }
}