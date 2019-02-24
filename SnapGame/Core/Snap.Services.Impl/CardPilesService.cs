using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Services.Impl
{
    internal sealed class CardPilesService : ICardPilesService
    {
        private readonly SnapDbContext _db;

        public CardPilesService(SnapDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<StackNode>> AddRangeAsync(IEnumerable<StackNode> piles, CancellationToken token)
        {
            await _db.AddRangeAsync(piles ?? Enumerable.Empty<StackNode>(), token);
            await _db.SaveChangesAsync(token);
            return piles;
        }
    }
}