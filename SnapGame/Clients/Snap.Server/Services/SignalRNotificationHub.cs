using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.SignalR;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Server.Services
{
    internal sealed class SignalRNotificationHub : Hub
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly ISnapGameServices _service;
        private readonly IDealer _dealer;

        public SignalRNotificationHub(IGameRoomPlayerServices gameRoomService,
            ISnapGameServices service,
            IDealer dealer)
        {
            _gameRoomService = gameRoomService;
            _service = service;
            _dealer = dealer;
        }

        public async Task<GameRoomPlayer> JoinGame(int roomId,
           bool isViewer, CancellationToken token = default(CancellationToken))
        {
            var roomPlayer = await _gameRoomService.AddPlayersAsync(roomId, isViewer, CancellationToken.None);
            await NotifyRoomGroup(nameof(JoinGame), roomPlayer.GameRoom, roomPlayer, token);
            return roomPlayer;
        }

        public async Task<SnapGame> StartGame(int roomId, CancellationToken token = default(CancellationToken))
        {
            var game = await _service.StarGameAsync(roomId, token);
            await NotifyRoomGroup(nameof(StartGame), game.GameData.Room, game, token);
            return game;
        }

        public async Task<PlayerGameplay> PopCard(int gameId, CancellationToken token = default(CancellationToken))
        {
            var gameplay = await _dealer.PopCurrentPlayerCardAsync(gameId, token);
            await NotifyRoomGroup(nameof(PopCard), gameplay.GameData.Room, gameplay, token);
            return gameplay;
        }

        private async Task NotifyRoomGroup<TData>(string method, GameRoom room, TData data, CancellationToken token) =>
            await Clients.Group(room.GameIdentifier.ToString()).SendAsync(method, data, token);
    }
}
