using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;
using Snap.Services.Impl.Exceptions;
using Snap.Services.Impl.Notifications;

namespace Snap.Services.Impl
{
    public class SnapGameServices : ISnapGameServices
    {
        private readonly ICardPilesService _cardPilesServices;
        private readonly ISnapGameConfigurationProvider _configuration;
        private readonly SnapDbContext _db;
        private readonly IDealer _dealer;
        private readonly INotificationService _notificationService;
        private readonly IPlayerTurnsService _playerTurnsService;
        private readonly IStateMachineProvider<GameState, GameSessionTransitions> _stateMachineProvider;
        private readonly IPlayerService _playerService;

        public SnapGameServices(ISnapGameConfigurationProvider configuration,
            IDealer dealer,
            IPlayerTurnsService playerTurnsService,
            ICardPilesService cardPilesServices,
            IStateMachineProvider<GameState, GameSessionTransitions> stateMachineProvider,
            SnapDbContext db,
            INotificationService notificationService,
            IPlayerService playerService)
        {
            _db = db;
            _notificationService = notificationService;
            _playerService = playerService;
            _stateMachineProvider = stateMachineProvider;
            _configuration = configuration;
            _dealer = dealer;
            _playerTurnsService = playerTurnsService;
            _cardPilesServices = cardPilesServices;
        }

        public async Task<SnapGame> StarGameAsync(GameRoom room, CancellationToken token)
        {
            var creator = await _playerService.GetCurrentPlayerAsync();
            if (creator == null)
                throw new UnauthorizedCreateException();

            if (room.RoomPlayers.Count(r => !r.IsViewer) < _configuration.MinRoomPlayers())
                throw new NotEnoughPlayerInGameSession();

            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var turns = _dealer
                    .ChooseTurns(room.RoomPlayers.Where(p => !p.IsViewer)
                        .Select(p => p.Player));

                turns = await _playerTurnsService.PushListAsync(turns, token);

                var game = new SnapGame
                {
                    GameData = new SnapGameData
                    {
                        FirstPlayer = turns.First()
                    },
                    CentralPile = new StackEntity(),
                };

                await _db.PlayersData.AddRangeAsync(turns.Select(pt => new PlayerData
                {
                    SnapGame = game,
                    PlayerTurn = pt
                }), CancellationToken.None);

                var shuffledCards = _dealer.ShuffleCards();

                await _cardPilesServices.AddRangeAsync(_dealer.DealtCards(game.PlayersData.Select(p => p.StackEntity).ToList(), shuffledCards), token);
                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.START_GAME);
                room.CanJoin = false;

                if (game.GameData.CurrentState != GameState.PLAYING)
                    throw new InvalidGameStateException();

                game.GameData.NextTurn();

                await _db.SaveChangesAsync(token);

                _notificationService?.OnGameStarted(this, new GameStartedEvent(game));

                trans.Commit();
                return game;
            }
        }

    }
}