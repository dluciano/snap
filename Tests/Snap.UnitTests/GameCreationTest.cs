using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services;
using Xunit;

namespace Snap.UnitTests
{
    public class GameCreationTest
    {
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

                        var service = new GameSessionServices(new GameRoomPlayerServices(db),
                            new SnapGameConfigurationProvider(),
                            new Dealer(),
                            new PlayerTurnsService(db),
                            new CardPilesService(db),
                            db);
                        var testPlayer = new Player { Username = "test" };
                        var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                            .RoomPlayers.Select(r => r.Player).SingleOrDefault();
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

                        var service = new GameSessionServices(new GameRoomPlayerServices(db),
                            new SnapGameConfigurationProvider(),
                            new Dealer(),
                            new PlayerTurnsService(db),
                            new CardPilesService(db),
                            db);
                        var testPlayer = new Player { Username = "test" };
                        var player = (await service.CreateAsync(CancellationToken.None, testPlayer))
                            .RoomPlayers.SingleOrDefault();
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
