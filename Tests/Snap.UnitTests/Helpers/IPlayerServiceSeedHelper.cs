using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;

namespace Snap.Tests.Helpers
{
    public interface IPlayerServiceSeedHelper
    {
        Task<Player> SeedAndLoginAsync(string username = PlayerServiceSeedHelper.FirstPlayerUsername, CancellationToken token = default);
        Task<Player> LoginPlayerAsync(string username = PlayerServiceSeedHelper.FirstPlayerUsername);
    }
}