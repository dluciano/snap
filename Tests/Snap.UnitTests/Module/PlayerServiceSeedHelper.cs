using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.EntityFrameworkCore;
using Snap.Fakes;

namespace Snap.Tests.Module
{
    public sealed class PlayerServiceSeedHelper
    {
        public const string FirstPlayerUsername = "First Player";
        public const string SecondPlayerUsername = "Second Player";
        private readonly IFakePlayerService _playerService;

        public PlayerServiceSeedHelper(IFakePlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<Player> SeedPlayerAsync(string username = FirstPlayerUsername, CancellationToken token = default) =>
            await _playerService
                .AddAsync(new Player { Username = username }, token);

        public async Task<Player> SeedAndLoginAsync(string username = FirstPlayerUsername, CancellationToken token = default) => 
            await LoginPlayerAsync((await SeedPlayerAsync(username, token)).Username);

        public async Task<Player> LoginPlayerAsync(string username = FirstPlayerUsername) =>
            await _playerService
                .SetCurrentPlayer(async players => await players.SingleAsync(p => p.Username == username));
    }
}