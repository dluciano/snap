using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IGameRoomServices
    {
        Task<GameRoom> CreateAsync(CancellationToken token=default(CancellationToken));
        event AsyncEventHandler<GameRoom> OnRoomCreatedEvent;
    }
}
