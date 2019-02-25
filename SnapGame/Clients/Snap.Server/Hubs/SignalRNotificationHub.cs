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
    public sealed class SignalRNotificationHub : Hub
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly IGameRoomServices _roomService;
        private readonly ISnapGameServices _snapGameService;
        private readonly IDealer _dealer;
        public const string JOIN_GAME = nameof(JoinGame);
        public const string GAME_STARTED = nameof(StartGame);

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
            _snapGameService.OnGameStartEvent += SnapGameServiceOnOnGameStartEvent;
            _dealer.OnCardPopEvent += DealerOnOnCardPopEvent;
        }

        private async Task DealerOnOnCardPopEvent(object sender, CardPopEvent args, CancellationToken token) =>
            await NotifyRoomGroup(nameof(JoinGame), args.GamePlay.GameData.Room, args, token);

        private async Task SnapGameServiceOnOnGameStartEvent(object sender, SnapGame args, CancellationToken token = default(CancellationToken)) =>
            await NotifyRoomGroup(nameof(JoinGame), args.GameData.Room, args, token);

        private async Task OnPlayerJoined(object sender, GameRoomPlayer args, CancellationToken token = default(CancellationToken))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, args.GameRoom.GameIdentifier.ToString(), token);
            await NotifyRoomGroup(nameof(JoinGame), args.GameRoom, args, token);
        }

        private async Task OnRoomCreatedEvent(object sender, GameRoom args, CancellationToken token = default(CancellationToken)) =>
            await Groups.AddToGroupAsync(Context.ConnectionId, args.GameIdentifier.ToString(), token);

        public async Task<GameRoom> CreateRoom() =>
            await _roomService.CreateAsync(Context.ConnectionAborted);

        public async Task<GameRoomPlayer> JoinGame(int roomId, bool isViewer)
        {
            var token = CancellationToken.None;
            var roomPlayer = await _gameRoomService.AddPlayersAsync(roomId, isViewer, Context.ConnectionAborted);
            return roomPlayer;
        }

        public async Task<SnapGame> StartGame(int roomId)
        {
            var token = Context.ConnectionAborted;
            var game = await _snapGameService.StarGameAsync(roomId, token);
            await NotifyRoomGroup(nameof(StartGame), game.GameData.Room, game, token);
            return game;
        }

        public async Task<PlayerGameplay> PopCard(int gameId)
        {
            var token = Context.ConnectionAborted;
            var gameplay = await _dealer.PopCurrentPlayerCardAsync(gameId, token);
            await NotifyRoomGroup(nameof(PopCard), gameplay.GameData.Room, gameplay, token);
            return gameplay;
        }

        private async Task NotifyRoomGroup<TData>(string method, GameRoom room, TData data, CancellationToken token) =>
            await Clients.Group(room.GameIdentifier.ToString()).SendAsync(method, data, token);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;
            _roomService.OnRoomCreatedEvent -= OnRoomCreatedEvent;
            _gameRoomService.OnPlayerJoinedEvent -= OnPlayerJoined;
            _snapGameService.OnGameStartEvent -= SnapGameServiceOnOnGameStartEvent;
            _dealer.OnCardPopEvent -= DealerOnOnCardPopEvent;
        }
    }
}
