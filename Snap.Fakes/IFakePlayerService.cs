using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using GameSharp.Services.Abstract;

namespace Snap.Fakes
{
    public interface IFakePlayerService : IPlayerService
    {
        Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action);
        Task<IEnumerable<Player>> AddRangeAsync(params string[] usernames);
        IQueryable<Player> GetPlayers();
    }
}