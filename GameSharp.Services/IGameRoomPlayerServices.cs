using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services
{
    public interface IGameRoomPlayerServices
    {
        Task<GameRoomPlayer> AddPlayersAsync(GameRoom game,
            bool isViewer,
            CancellationToken token);
    }
}