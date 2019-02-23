using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;

namespace Snap.Fakes
{
    public interface IFakePlayerService : IPlayerService
    {
        Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action);
        Task<IEnumerable<Player>> AddRangeAsync(CancellationToken token = default(CancellationToken), params string[] usernames);
        IQueryable<Player> GetPlayers();
    }
}