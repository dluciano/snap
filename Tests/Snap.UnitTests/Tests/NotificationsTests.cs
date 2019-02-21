using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Notifications;
using Snap.Tests.Module;
using Xunit;

namespace Snap.Tests.Tests
{
    public class NotificationsTests
    {
        [Fact]
        async Task When_first_playe_pop_the_KING_TILE_then_all_players_should_be_notified()
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
                var notifier = module.GetService<INotificationService>();
                PlayerGameplay gameplay = null;
                notifier.CardPopEvent += async (sender, e) =>
                {
                    gameplay = e.GamePlay;
                };
                var game = await service.CreateAsync(CancellationToken.None);
                game = await service.StarGameAsync(game, CancellationToken.None);
                await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                gameplay.ShouldNotBeNull();
            }
        }

        [Fact]
        async Task When_player_1_pop_the_KING_TILE_then_player_1_should_be_notified_as_the_current_player()
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
                var notifier = module.GetService<INotificationService>();
                var notificated = false;
                notifier.CardPopEvent += (sender, e) =>
                {
                    e.NextPlayer.PlayerTurn.Player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
                    notificated = true;
                };

                var game = await service.CreateAsync(CancellationToken.None);
                game = await service.StarGameAsync(game, CancellationToken.None);
                await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                notificated.ShouldBeTrue();
            }
        }

        [Fact]
        async Task When_a_game_is_started_players_should_be_notified()
        {
            using (var module = await TestModuleHelpers
                    .CreateAndBuildWithDefaultsAsync())
            {
                var service = module.GetService<ISnapGameServices>();
                var notifier = module.GetService<INotificationService>();
                var notificated = false;
                notifier.GameStartEvent += (sender, e) =>
                {
                    e.Game.GameData.CurrentState.ShouldBe(GameState.PLAYING);
                    notificated = true;
                };
                var game = await service.CreateAsync(CancellationToken.None);
                game = await service.StarGameAsync(game, CancellationToken.None);

                notificated.ShouldBeTrue();
            }
        }
    }

}
