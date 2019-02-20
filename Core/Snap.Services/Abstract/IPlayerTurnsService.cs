using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface IPlayerTurnsService
    {
        Task<PlayerTurn[]> AddRangeAsync(CancellationToken token, params PlayerTurn[] turns);
    }
}