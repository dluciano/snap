using System.Threading;
using System.Threading.Tasks;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ISnapGameServices
    {
        Task<SnapGame> CreateAsync(CancellationToken token);
        Task<SnapGame> StarGameAsync(SnapGame game, CancellationToken token);
    }
}