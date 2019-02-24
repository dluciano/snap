using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IGameRoomPlayerServices
    {
        Task<GameRoomPlayer> AddPlayersAsync(int roomId,
            bool isViewer,
            CancellationToken token);
    }
}