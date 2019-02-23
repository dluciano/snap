using System;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;

namespace Snap.Fakes
{
    public interface IFakePlayerProvider : IPlayerProvider
    {
        Task<Player> SetCurrentPlayer(Func<IQueryable<Player>, Task<Player>> action);
        IQueryable<Player> GetPlayers();
    }
}