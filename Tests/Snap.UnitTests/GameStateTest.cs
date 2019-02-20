using System.Threading;
using Snap.Entities;
using Shouldly;
using Xunit;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;
using Snap.Entities.Enums;
using Snap.Services;

namespace Snap.UnitTests
{
    public class GameStateTest
    {
        private static readonly IStateMachineProvider<GameState, GameSessionTransitions> _stateMachine = new StateMachine<GameState, GameSessionTransitions>()
            .AddTransition(GameState.NONE, GameState.AWAITING_PLAYERS, GameSessionTransitions.CREATE_GAME)
            .AddTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING, GameSessionTransitions.START_GAME)
            .AddTransition(GameState.PLAYING, GameState.FINISHED, GameSessionTransitions.FINISH_GAME)
            .AddTransition(GameState.PLAYING, GameState.ABORTED, GameSessionTransitions.ABORT_GAME);
        [Fact]
        public async Task When_create_game_state_should_be_awaiting_player()
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
                        (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }))
                            .GameData.From
                            .ShouldBe(GameState.AWAITING_PLAYERS);
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
