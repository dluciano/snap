using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface ISnapGameServices
    {
        Task<SnapGame> StarGameAsync(GameRoom game, CancellationToken token);
    }
}