using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.DI;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Notifications;
using Xunit;

namespace Snap.Tests
{
    public class NotificationsTests
    {
        [Fact]
        async Task When_player_1_pop_the_KING_TILE_then_all_players_should_be_notified()
        {
            var firstPlayer = new Player() { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await new ModuleManager()
                .UseDefaults()
                .WithFakePlayerRandomizer(players)
                .WithFakeCardRandomizer(fakedRandomCards)
                .WithFakePlayerService(firstPlayer)
                .WithFakeDealter()
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();
                var notifier = module.GetService<INotificationService>();
                PlayerGameplay gameplay = null;
                notifier.CardPopEvent += (sender, e) => { gameplay = e.GamePlay; };
                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);
                await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                gameplay.ShouldNotBeNull();
            }
        }

        [Fact]
        async Task When_player_1_pop_the_KING_TILE_then_player_1_should_be_notified_as_the_current_player()
        {
            var firstPlayer = new Player() { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await new ModuleManager()
                .UseDefaults()
                .WithFakePlayerRandomizer(players)
                .WithFakeCardRandomizer(fakedRandomCards)
                .WithFakePlayerService(firstPlayer)
                .WithFakeDealter()
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();
                var notifier = module.GetService<INotificationService>();
                var notificated = false;
                notifier.CardPopEvent += (sender, e) =>
                {
                    e.NextPlayer.PlayerTurn.Player.Username.ShouldBe(other.Username);
                    notificated = true;
                };

                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);
                await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                notificated.ShouldBeTrue();
            }
        }

        [Fact]
        async Task When_a_game_is_started_players_should_be_notified()
        {
            var firstPlayer = new Player() { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            var fakedRandomCards = Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();

            using (var module = await new ModuleManager()
                .UseDefaults()
                .WithFakePlayerRandomizer(players)
                .WithFakeCardRandomizer(fakedRandomCards)
                .WithFakePlayerService(firstPlayer)
                .WithFakeDealter()
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var notifier = module.GetService<INotificationService>();
                var notificated = false;
                notifier.GameStartEvent += (sender, e) =>
                {
                    e.Game.GameData.From.ShouldBe(GameState.PLAYING);
                    notificated = true;
                };
                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);

                notificated.ShouldBeTrue();
            }
        }
    }

}
