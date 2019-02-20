using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface IGameSessionServices
    {
        Task<GameRoom> CreateAsync(CancellationToken token, params Player[] players);
        Task<GameRoom> StarGame(GameRoom game, CancellationToken token);
    }
}