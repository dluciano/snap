using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;
using Snap.Fakes;

namespace Snap.ConsoleApplication
{
    internal static class Helpers
    {
        public static async Task<Player> SetCurrentPlayer(this IFakePlayerProvider provider, Player player) =>
            await provider
                .Authenticate(dbPlayers => Task.FromResult(player));

        public static async Task<Player> LoginAndCreateUser(this IFakePlayerProvider playerProvider, string username)
        {
            var player = new Player
            {
                Username = username
            };
            await playerProvider.Authenticate(async players => await Task.FromResult(player));
            return await playerProvider.AddAsync(CancellationToken.None);
        }
    }
}
