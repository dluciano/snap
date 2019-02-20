using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services
{
    public interface IPlayerTurnsService
    {
        Task<PlayerTurn[]> AddRangeAsync(CancellationToken token, params PlayerTurn[] turns);
    }
}