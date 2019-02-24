using System;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Fakes
{
    public sealed class FakePlayerProvider : PlayerProviderBase, IFakePlayerProvider
    {
        private Player _currentPlayer;

        public FakePlayerProvider(SnapDbContext db)
            : base(db)
        {
        }

        public async Task<Player> Authenticate(Func<IQueryable<Player>, Task<Player>> action)
        {
            var player = (await action?.Invoke(_db.Players));
            var exists = await _db.Players.SingleOrDefaultAsync(p => p.Id == player.Id);
            return _currentPlayer = exists ?? player;
        }

        public IQueryable<Player> GetPlayers() => _db.Players;

        public override async Task<Player> GetCurrentPlayerAsync() => await Task.FromResult(_currentPlayer);
    }
}