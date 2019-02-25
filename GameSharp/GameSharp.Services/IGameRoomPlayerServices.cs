using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IGameRoomPlayerServices
    {
        Task<GameRoomPlayer> AddPlayersAsync(int roomId,
            bool isViewer,
            CancellationToken token);

        event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinedEvent;
    }
}