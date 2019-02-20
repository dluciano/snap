using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface IGameRoomPlayerServices
    {
        Task<IAsyncEnumerable<GameRoomPlayer>> AddPlayersAsync(GameRoom game,
            CancellationToken token,
            params Player[] players);
    }
}