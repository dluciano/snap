using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ISnapGameServices
    {
        Task<SnapGame> CreateAsync(CancellationToken token, params Player[] players);
        Task<SnapGame> StarGame(SnapGame game, CancellationToken token);
    }
}