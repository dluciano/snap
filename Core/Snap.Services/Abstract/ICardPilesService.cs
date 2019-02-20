using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ICardPilesService
    {
        Task<IEnumerable<StackNode>> AddRangeAsync(IEnumerable<StackNode> piles, CancellationToken token);
    }
}