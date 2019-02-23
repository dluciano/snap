using System;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Fakes
{
    public sealed class FakePlayerService : PlayerServiceBase, IFakePlayerService
    {
        private Player _currentPlayer;

        public FakePlayerService(SnapDbContext db)
            : base(db)
        {
        }

        public async Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action)
        {
            var playerId = (await action?.Invoke(_db.Players)).Id;
            return _currentPlayer = await _db.Players.SingleAsync(p => p.Id == playerId);
        }

        public IQueryable<Player> GetPlayers() => _db.Players;

        public override async Task<Player> GetCurrentPlayerAsync() => await Task.FromResult(_currentPlayer);
    }
}