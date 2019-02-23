using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Fakes
{
    public class FakePlayerService : IFakePlayerService
    {
        protected readonly SnapDbContext _db;
        private Player _currentPlayer;

        public FakePlayerService(SnapDbContext db)
        {
            _db = db;
        }

        public async Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action)
        {
            var playerId = (await action?.Invoke(_db.Players)).Id;
            return _currentPlayer = await _db.Players.SingleAsync(p => p.Id == playerId);
        }

        public async Task<IEnumerable<Player>> AddRangeAsync(CancellationToken token, params string[] usernames)
        {
            var players = usernames?.Select(u => new Player { Username = u })?.ToList();
            if (players == null)
                return await Task.FromResult(Enumerable.Empty<Player>());
            await _db.Players.AddRangeAsync(players, token);
            await _db.SaveChangesAsync(token);
            return players;
        }

        public IQueryable<Player> GetPlayers() => _db.Players;

        public virtual async Task<Player> GetCurrentPlayerAsync() => await Task.FromResult(_currentPlayer);
    }
}