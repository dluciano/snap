using System;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using GameSharp.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Tests.Module;

namespace Snap.Tests
{
    public class BackgroundHelper
    {
        private readonly ISnapGameServices _gameService;
        private readonly IGameRoomPlayerServices _roomService;
        private readonly PlayerServiceSeedHelper _playerSeedHelper;

        public BackgroundHelper(ISnapGameServices gameService,
            IGameRoomPlayerServices roomService,
            PlayerServiceSeedHelper playerSeedHelper)
        {
            _gameService = gameService;
            _roomService = roomService;
            _playerSeedHelper = playerSeedHelper;
        }
        public async Task<SnapGame> CreateGameAsync()
        {
            await _playerSeedHelper.SeedAndLoginAsync();
            return await _gameService.CreateAsync(CancellationToken.None);
        }

        public async Task<GameRoomPlayer> PlayerJoinAsync(SnapGame game, string username = PlayerServiceSeedHelper.SecondPlayerUsername)
        {
            await _playerSeedHelper.SeedAndLoginAsync(username);
            return await _roomService.AddPlayersAsync(game.GameData.GameRoom, false, CancellationToken.None);
        }

        public async Task<SnapGame> StartGameAsync(SnapGame game)
        {
            await _playerSeedHelper.LoginPlayerAsync();
            return await _gameService.StarGameAsync(game, CancellationToken.None);
        }
    }
}
