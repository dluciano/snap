using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IPlayerService
    {
        Task<Player> GetCurrentPlayerAsync();
        Task<Player> AddAsync(Player player, CancellationToken token = default(CancellationToken));
    }
}