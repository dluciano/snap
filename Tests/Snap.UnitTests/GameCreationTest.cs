using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Snap.Services.Abstract;
using Xunit;

namespace Snap.Tests
{
    public class GameCreationTest
    {
        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var testPlayer = new Player { Username = "test" };
                var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                    .GameData.GameRoom.RoomPlayers.Select(r => r.Player).SingleOrDefault();
                player.ShouldNotBeNull();
                player.ShouldBe(testPlayer);
            }
        }

        [Fact]
        public async Task When_create_game_it_players_should_be_player_not_viewers()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var testPlayer = new Player { Username = "test" };
                var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                    .GameData.GameRoom.RoomPlayers.SingleOrDefault();
                player.ShouldNotBeNull();
                player.IsViewer.ShouldBeFalse();
            }
        }

        private sealed class FirstPlayerIsLastInTheRoomRandomizer : IListRandomizer
        {
            public IEnumerable<T> Generate<T>(IEnumerable<T> list) =>
                list.Reverse();
        }

        [Fact]
        public async Task When_game_started_player_2_should_be_the_current_player()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults(new FirstPlayerIsLastInTheRoomRandomizer()))
            {
                var service = module.GetService<ISnapGameServices>();
                var expected = new Player() { Username = "Player 2" };
                var game = (await service.CreateAsync(CancellationToken.None,
                    new Player { Username = "Player 1" },
                    expected));
                (await service.StarGame(game, CancellationToken.None)).CurrentTurn
                    .PlayerTurn.Player.Username.ShouldBe(expected.Username);
            }
        }

        [Fact]
        public async Task When_game_started_with_2_player_each_player_should_be_have_26_cards()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var expected = new Player() { Username = "Player 2" };
                var game = (await service.CreateAsync(CancellationToken.None,
                    new Player { Username = "Player 1" },
                    expected));
                game = await service.StarGame(game, CancellationToken.None);
                game.PlayersData.First().StackEntity.ToList()
                    .Count().ShouldBe(26);
                game.PlayersData.Last().StackEntity.ToList()
                    .Count().ShouldBe(26);
            }
        }

        [Fact]
        public async Task When_game_started_with_2_player_not_cards_should_be_repeated()
        {
            using (var module = await ModuleHelper.CreateModuleWithDefaults())
            {
                var service = module.GetService<ISnapGameServices>();
                var game = (await service.CreateAsync(CancellationToken.None,
                    new Player { Username = "Player 1" },
                    new Player() { Username = "Player 2" }));
                game = await service.StarGame(game, CancellationToken.None);
                game.PlayersData
                    .SelectMany(p => p.StackEntity.ToList().Select(s => s.Card))
                    .ShouldBeUnique();
            }
        }
    }
}
