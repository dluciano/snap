using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class PlayerTurnsService : IPlayerTurnsService
    {
        private readonly SnapDbContext _db;

        public PlayerTurnsService(SnapDbContext db)
        {
            _db = db;
        }

        public async Task<PlayerTurn[]> AddRangeAsync(CancellationToken token, params PlayerTurn[] turns)
        {
            await _db.AddRangeAsync(turns ?? Enumerable.Empty<PlayerTurn>(), token);
            await _db.SaveChangesAsync(token);
            return turns;
        }
    }
}