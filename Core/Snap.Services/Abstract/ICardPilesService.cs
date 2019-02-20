using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ICardPilesService
    {
        Task<IEnumerable<CardPileNode>> AddRangeAsync(IEnumerable<CardPileNode> piles, CancellationToken token);
    }
}