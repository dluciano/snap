using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Services.Impl
{
    internal class SnapGameCommands : ISnapGameCommands
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly ISnapGameServices _service;
        private readonly IDealer _dealer;

        public SnapGameCommands(IGameRoomPlayerServices gameRoomService,
            ISnapGameServices service,
            IDealer dealer)
        {
            _gameRoomService = gameRoomService;
            _service = service;
            _dealer = dealer;
        }

        public async Task<GameRoomPlayer> JoinGame(int roomId,
            bool isViewer, CancellationToken token = default(CancellationToken)) =>
            await _gameRoomService.AddPlayersAsync(roomId, isViewer, CancellationToken.None);

        public async Task<SnapGame> StartGameAsync(int roomId, CancellationToken token = default(CancellationToken)) =>
            await _service.StarGameAsync(roomId, token);

        public async Task<PlayerGameplay> PopCardAsync(int gameId, CancellationToken token = default(CancellationToken)) =>
            await _dealer.PopCurrentPlayerCardAsync(gameId, token);
    }
}