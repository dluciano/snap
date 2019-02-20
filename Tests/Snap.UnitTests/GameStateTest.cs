using System.Threading;
using Snap.Entities;
using Shouldly;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;
using Snap.Entities.Enums;
using Snap.Services;

namespace Snap.UnitTests
{
    public class GameStateTest
    {
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

                        var service = new GameSessionServices(new GameRoomPlayerServices(db),
                            new SnapGameConfigurationProvider(),
                            new Dealer(),
                            new PlayerTurnsService(db),
                            new CardPilesService(db),
                            db);
                        (await service.CreateAsync(CancellationToken.None, new Player { Username = "test" }))
                            .State
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
