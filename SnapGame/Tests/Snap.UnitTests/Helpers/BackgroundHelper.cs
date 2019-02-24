using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Tests.Helpers
{
    internal class BackgroundHelper : IBackgroundHelper
    {
        private readonly ISnapGameServices _gameService;
        private readonly IPlayerServiceSeedHelper _playerSeedHelper;
        private readonly IGameRoomPlayerServices _roomPlayerService;
        private readonly IGameRoomServices _roomService;

        public BackgroundHelper(ISnapGameServices gameService,
            IGameRoomPlayerServices roomPlayerService,
            IPlayerServiceSeedHelper playerSeedHelper,
            IGameRoomServices roomService)
        {
            _gameService = gameService;
            _roomPlayerService = roomPlayerService;
            _playerSeedHelper = playerSeedHelper;
            _roomService = roomService;
        }

        public async Task<GameRoom> CreateRoomAsync()
        {
            await _playerSeedHelper.SeedAndLoginAsync();
            return await _roomService.CreateAsync(CancellationToken.None);
        }

        public async Task<GameRoomPlayer> PlayerJoinAsync(GameRoom room,
            string username = PlayerServiceSeedHelper.SecondPlayerUsername)
        {
            await _playerSeedHelper.SeedAndLoginAsync(username);
            return await _roomPlayerService.AddPlayersAsync(room.Id, false, CancellationToken.None);
        }

        public async Task<SnapGame> StartGameAsync(GameRoom room)
        {
            await _playerSeedHelper.LoginPlayerAsync();
            return await _gameService.StarGameAsync(room.Id, CancellationToken.None);
        }
    }
}