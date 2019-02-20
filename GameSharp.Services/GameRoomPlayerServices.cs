using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Exceptions;

namespace GameSharp.Services
{
    public class GameRoomPlayerServices : IGameRoomPlayerServices
    {
        private readonly GameSharpContext _db;

        public GameRoomPlayerServices(GameSharpContext db)
        {
            _db = db;
        }

        public async Task<IAsyncEnumerable<GameRoomPlayer>> AddPlayersAsync(GameData game,
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
                    GameRoom = game.GameRoom,
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
