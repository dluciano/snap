using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class CardPilesService : ICardPilesService
    {
        private readonly SnapDbContext _db;

        public CardPilesService(SnapDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CardPileNode>> AddRangeAsync(IEnumerable<CardPileNode> piles, CancellationToken token)
        {
            await _db.AddRangeAsync(piles ?? Enumerable.Empty<CardPileNode>(), token);
            await _db.SaveChangesAsync(token);
            return piles;
        }
    }
}