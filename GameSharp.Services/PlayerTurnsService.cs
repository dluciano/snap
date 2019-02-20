using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;

namespace GameSharp.Services
{
    public class PlayerTurnsService : IPlayerTurnsService
    {
        private readonly GameSharpContext _db;

        public PlayerTurnsService(GameSharpContext db)
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