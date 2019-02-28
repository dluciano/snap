using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Services.Abstract.Notifications;

namespace Snap.Server.Services
{
    [Authorize]
    internal sealed class SignalRNotificationHub : Hub
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly IGameRoomServices _roomService;
        private readonly ISnapGameServices _snapGameService;
        private readonly IDealer _dealer;

        public SignalRNotificationHub(IGameRoomPlayerServices gameRoomService,
            IGameRoomServices roomService,
            ISnapGameServices snapGameService,
            IDealer dealer)
        {
            _gameRoomService = gameRoomService;
            _roomService = roomService;
            _snapGameService = snapGameService;
            _dealer = dealer;

            _roomService.OnRoomCreatedEvent += OnRoomCreatedEvent;
            _gameRoomService.OnPlayerJoinedEvent += OnPlayerJoined;
            _snapGameService.OnGameStartEvent += OnGameStartEvent;
            _dealer.OnCardPopEvent += OnCardPopEvent;
        }

        private async Task OnCardPopEvent(object sender, CardPopEvent args, CancellationToken token) =>
            await NotifyRoomGroup(nameof(PopCard), args.GamePlay.GameData.Room, new { card = args.GamePlay.Card, currentPlayer = args.NextPlayer.PlayerTurn.Player.Username }, token);

        private async Task OnGameStartEvent(object sender, SnapGame args, CancellationToken token = default(CancellationToken)) =>
            await NotifyRoomGroup(nameof(StartGame), args.GameData.Room, new { gameId = args.Id, currentPlayer = args.CurrentTurn.PlayerTurn.Player.Username }, token);

        private async Task OnPlayerJoined(object sender, GameRoomPlayer args, CancellationToken token = default(CancellationToken))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, args.GameRoom.GameIdentifier.ToString(), token);
            await NotifyRoomGroup(nameof(JoinGame), args.GameRoom, new { username = args.Player.Username, roomId = args.RoomId }, token);
        }

        private async Task OnRoomCreatedEvent(object sender, GameRoom args, CancellationToken token = default(CancellationToken))
        {
            var methodName = nameof(CreateRoom);
            var data = new { roomId = args.Id, createdBy = args.CreatedBy.Username };
            await Groups.AddToGroupAsync(Context.ConnectionId, args.GameIdentifier.ToString(), token);
            await Clients
                .All
                .SendAsync(methodName, data, token);
        }

        public async Task<int> CreateRoom() =>
            (await _roomService.CreateAsync(Context.ConnectionAborted)).Id;

        public async Task<int> JoinGame(int roomId, bool isViewer) =>
            (await _gameRoomService.AddPlayersAsync(roomId, isViewer, Context.ConnectionAborted)).Id;

        public async Task<int> StartGame(int roomId) =>
            (await _snapGameService.StarGameAsync(roomId, Context.ConnectionAborted)).Id;

        public async Task<int> PopCard(int gameId) =>
            (await _dealer.PopCurrentPlayerCardAsync(gameId, Context.ConnectionAborted)).Id;

        private async Task NotifyRoomGroup<TData>(string method, GameRoom room, TData data, CancellationToken token) =>
            await Clients.Group(room.GameIdentifier.ToString()).SendAsync(method, data, token);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;
            _roomService.OnRoomCreatedEvent -= OnRoomCreatedEvent;
            _gameRoomService.OnPlayerJoinedEvent -= OnPlayerJoined;
            _snapGameService.OnGameStartEvent -= OnGameStartEvent;
            _dealer.OnCardPopEvent -= OnCardPopEvent;
        }
    }
}
