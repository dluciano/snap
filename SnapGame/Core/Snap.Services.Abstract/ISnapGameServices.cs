using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ISnapGameServices
    {
        Task<SnapGame> StarGameAsync(int roomId, CancellationToken token);
        event AsyncEventHandler<SnapGame> OnGameStartEvent;
    }
}