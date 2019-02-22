using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Snap.DataAccess;

namespace Snap.Server
{
    internal class ServerPlayerService : IPlayerService
    {
        private readonly SnapDbContext _db;

        public ServerPlayerService(SnapDbContext db)
        {
            _db = db;
        }
        public async Task<Player> GetCurrentPlayerAsync() => await _db.Players.FindAsync(1);
    }
}