using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IPlayerTurnsService
    {
        Task<IEnumerable<PlayerTurn>> PushListAsync(IEnumerable<PlayerTurn> turns, CancellationToken token);
    }
}