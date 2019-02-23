using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public abstract class PlayerServiceBase : IPlayerService
    {
        protected readonly GameSharpContext _db;

        protected PlayerServiceBase(GameSharpContext db)
        {
            _db = db;
        }

        public async Task<Player> AddAsync(Player player, CancellationToken token = default(CancellationToken))
        {
            await _db.Players.AddAsync(player, token);
            await _db.SaveChangesAsync(token);
            return player;
        }

        public abstract Task<Player> GetCurrentPlayerAsync();
    }
}