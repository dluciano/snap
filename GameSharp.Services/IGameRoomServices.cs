using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IGameRoomServices
    {
        Task<GameRoom> CreateAsync(CancellationToken token);
    }
}
