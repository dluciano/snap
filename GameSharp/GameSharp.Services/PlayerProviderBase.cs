using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public abstract class PlayerProviderBase : IPlayerProvider
    {
        protected readonly GameSharpContext _db;

        protected PlayerProviderBase(GameSharpContext db)
        {
            _db = db;
        }

        public async Task<Player> AddAsync(CancellationToken token = default(CancellationToken))
        {
            var player = await this.GetCurrentPlayerAsync();
            await _db.Players.AddAsync(player, token);
            await _db.SaveChangesAsync(token);
            return player;
        }

        public abstract Task<Player> GetCurrentPlayerAsync();
    }
}