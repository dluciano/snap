using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Fakes
{
    public class FakePlayerService : IFakePlayerService
    {
        private readonly SnapDbContext _db;
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

        public async Task<IEnumerable<Player>> AddRangeAsync(params string[] usernames)
        {
            var players = usernames?.Select(u => new Player {Username = u})?.ToList();
            if (players == null)
                return await Task.FromResult(Enumerable.Empty<Player>());
            await _db.Players.AddRangeAsync(players);
            await _db.SaveChangesAsync();
            return players;
        }

        public IQueryable<Player> GetPlayers()
        {
            return _db.Players;
        }

        public async Task<Player> GetCurrentPlayerAsync()
        {
            return await Task.FromResult(_currentPlayer);
        }
    }
}