using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;

namespace Snap.Fakes
{
    public interface IFakePlayerService : IPlayerService
    {
        Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action);
        Task<IEnumerable<Player>> AddRangeAsync(params string[] usernames);
        Task<IQueryable<Player>> GetPlayers();
    }
}