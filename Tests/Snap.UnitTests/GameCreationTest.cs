using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services;
using Xunit;

namespace Snap.UnitTests
{
    public class GameCreationTest
    {
        private readonly static IStateMachineProvider<GameState, GameSessionTransitions> _stateMachine = new StateMachine<GameState, GameSessionTransitions>()
            .AddTransition(GameState.NONE, GameState.AWAITING_PLAYERS, GameSessionTransitions.CREATE_GAME)
            .AddTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING, GameSessionTransitions.START_GAME)
            .AddTransition(GameState.PLAYING, GameState.FINISHED, GameSessionTransitions.FINISH_GAME)
            .AddTransition(GameState.PLAYING, GameState.ABORTED, GameSessionTransitions.ABORT_GAME);
        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                try
                {
                    await connection.OpenAsync();
                    var optBuilder = new DbContextOptionsBuilder<SnapDbContext>();
                    optBuilder.UseSqlite(connection);
                    using (var db = new SnapDbContext(optBuilder.Options))
                    {
                        await db.Database.EnsureCreatedAsync(CancellationToken.None);

                        var service = new SnapGameServices(new GameRoomPlayerServices(db),
                            new SnapGameConfigurationProvider(),
                            new Dealer(),
                            new PlayerTurnsService(db),
                            new CardPilesService(db),
                            _stateMachine,
                            db);
                        var testPlayer = new Player { Username = "test" };
                        var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                            .GameData.GameRoom.RoomPlayers.Select(r => r.Player).SingleOrDefault();
                        player.ShouldNotBeNull();
                        player.ShouldBe(testPlayer);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [Fact]
        public async Task When_create_game_it_players_should_be_player_not_viewers()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                try
                {
                    await connection.OpenAsync();
                    var optBuilder = new DbContextOptionsBuilder<SnapDbContext>();
                    optBuilder.UseSqlite(connection);
                    using (var db = new SnapDbContext(optBuilder.Options))
                    {
                        await db.Database.EnsureCreatedAsync(CancellationToken.None);

                        var service = new SnapGameServices(new GameRoomPlayerServices(db),
                            new SnapGameConfigurationProvider(),
                            new Dealer(),
                            new PlayerTurnsService(db),
                            new CardPilesService(db),
                            _stateMachine,
                            db);
                        var testPlayer = new Player { Username = "test" };
                        var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                            .GameData.GameRoom.RoomPlayers.SingleOrDefault();
                        player.ShouldNotBeNull();
                        player.IsViewer.ShouldBeFalse();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
