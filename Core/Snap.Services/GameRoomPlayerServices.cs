using Snap.DataAccess;
using Snap.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class GameRoomPlayerServices : IGameRoomPlayerServices
    {
        private readonly SnapDbContext _db;

        public GameRoomPlayerServices(SnapDbContext db)
        {
            _db = db;
        }

        public async Task<IAsyncEnumerable<GameRoomPlayer>> AddPlayersAsync(GameRoom game,
            CancellationToken token,
            params Player[] players)
        {
            var entities = new List<GameRoomPlayer>();
            if (game.From != GameState.AWAITING_PLAYERS)
            {
                //TODO: Make exception handling i18n
                throw new InvalidGameStateException("The game is not in the state where players can join");
            }
            foreach (var player in (players ?? Array.Empty<Player>()))
            {
                entities.Add(new GameRoomPlayer
                {
                    GameRoom = game,
                    Player = player,
                    IsViewer = false,
                });
            }
            await _db.GameRoomPlayers.AddRangeAsync(entities, token);
            await _db.SaveChangesAsync(token);
            return entities.ToAsyncEnumerable();
        }
    }
}
