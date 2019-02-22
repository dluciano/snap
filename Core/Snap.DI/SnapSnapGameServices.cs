using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Services.Exceptions;
using Snap.Services.Notifications;

namespace Snap.Services.Impl
{
    public class SnapSnapGameServices : ISnapGameServices
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly SnapDbContext _db;
        private readonly INotificationService _notificationService;
        private readonly ISnapGameConfigurationProvider _configuration;
        private readonly IDealer _dealer;
        private readonly IPlayerTurnsService _playerTurnsService;
        private readonly ICardPilesService _cardPilesServices;
        private readonly IStateMachineProvider<GameState, GameSessionTransitions> _stateMachineProvider;
        private IPlayerService _playerService;

        public SnapSnapGameServices(IGameRoomPlayerServices gameRoomService,
            ISnapGameConfigurationProvider configuration,
            IDealer dealer,
            IPlayerTurnsService playerTurnsService,
            ICardPilesService cardPilesServices,
            IStateMachineProvider<GameState, GameSessionTransitions> stateMachineProvider,
            SnapDbContext db,
            INotificationService notificationService,
            IPlayerService playerService)
        {
            _gameRoomService = gameRoomService;
            _db = db;
            _notificationService = notificationService;
            _playerService = playerService;
            _stateMachineProvider = stateMachineProvider;
            _configuration = configuration;
            _dealer = dealer;
            _playerTurnsService = playerTurnsService;
            _cardPilesServices = cardPilesServices;
        }

        public async Task<SnapGame> CreateAsync(CancellationToken token)
        {
            var creator = await _playerService.GetCurrentPlayerAsync();
            if (creator == null)
                throw new UnauthorizedCreateException();
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var game = new SnapGame
                {
                    GameData = new SnapGameData
                    {
                        GameRoom = new GameRoom()
                    },
                    CentralPile = new StackEntity()
                };

                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.CREATE_GAME);
                await _db.SnapGames.AddAsync(game, token);
                await _db.SaveChangesAsync(token);

                await _gameRoomService.AddPlayersAsync(game.GameData.GameRoom, false, token);
                trans.Commit();
                return game;
            }
        }

        public async Task<SnapGame> StarGameAsync(SnapGame game, CancellationToken token)
        {
            if (game.GameData
                    .GameRoom
                    .RoomPlayers
                    .Count(r => !r.IsViewer) < _configuration.MinRoomPlayers())
            {
                throw new NotEnoughPlayerInGameSession();
            }
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var turns = await _playerTurnsService.AddRangeAsync(token, _dealer.ChooseTurns(game.GameData).ToArray());
                await _db.PlayersData.AddRangeAsync(turns.Select(pt => new PlayerData()
                {
                    SnapGame = game,
                    PlayerTurn = pt,
                }), CancellationToken.None);
                var shuffledCards = _dealer.ShuffleCards();
                await _cardPilesServices.AddRangeAsync(
                    _dealer.DealtCards(game.PlayersData.Select(p => p.StackEntity).ToList(), shuffledCards), token);
                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.START_GAME);

                if (game.GameData.CurrentState != GameState.PLAYING)
                {
                    throw new InvalidGameStateException();
                }
                game.GameData.NextTurn();
                await _db.SaveChangesAsync(token);
                _notificationService?.OnGameStarted(this, new GameStartedEvent(game));
                trans.Commit();
                return game;
            }
        }
    }
}
