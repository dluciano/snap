using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Tests.Module;

namespace Snap.Tests
{
    public class BackgroundHelper
    {
        private readonly ISnapGameServices _gameService;
        private readonly PlayerServiceSeedHelper _playerSeedHelper;
        private readonly IGameRoomPlayerServices _roomPlayerService;
        private readonly IGameRoomServices _roomService;

        public BackgroundHelper(ISnapGameServices gameService,
            IGameRoomPlayerServices roomPlayerService,
            PlayerServiceSeedHelper playerSeedHelper,
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
            return await _roomPlayerService.AddPlayersAsync(room, false, CancellationToken.None);
        }

        public async Task<SnapGame> StartGameAsync(GameRoom room)
        {
            await _playerSeedHelper.LoginPlayerAsync();
            return await _gameService.StarGameAsync(room, CancellationToken.None);
        }
    }
}