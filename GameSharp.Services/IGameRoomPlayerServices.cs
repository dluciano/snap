using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services
{
    public interface IGameRoomPlayerServices
    {
        Task<IAsyncEnumerable<GameRoomPlayer>> AddPlayersAsync(GameData game,
            CancellationToken token,
            params Player[] players);
    }
}