using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ISnapGameCommands
    {
        Task<GameRoomPlayer> JoinGame(int roomId,
            bool isViewer, CancellationToken token = default(CancellationToken));
        Task<SnapGame> StartGameAsync(int roomId, CancellationToken token = default(CancellationToken));
        Task<PlayerGameplay> PopCardAsync(int gameId, CancellationToken token = default(CancellationToken));
    }
}
