using Snap.Entities;
using System.Linq;
using System.Threading;
using Snap.DataAccess;
using System.Threading.Tasks;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Dawlin.Util;

namespace Snap.Services
{
    public class GameSessionServices : IGameSessionServices
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly SnapDbContext _db;
        private readonly ISnapGameConfigurationProvider _configuration;
        private readonly IDealer _dealer;
        private readonly IPlayerTurnsService _playerTurnsService;
        private readonly ICardPilesService _cardPilesServices;
        private readonly IStateMachineProvider<GameState, GameSessionTransitions> _stateMachineProvider;

        public GameSessionServices(IGameRoomPlayerServices gameRoomService,
            ISnapGameConfigurationProvider configuration,
            IDealer dealer,
            IPlayerTurnsService playerTurnsService,
            ICardPilesService cardPilesServices,
            IStateMachineProvider<GameState, GameSessionTransitions> stateMachineProvider,
            SnapDbContext db)
        {
            _gameRoomService = gameRoomService;
            _db = db;
            _stateMachineProvider = stateMachineProvider;
            _configuration = configuration;
            _dealer = dealer;
            _playerTurnsService = playerTurnsService;
            _cardPilesServices = cardPilesServices;
        }
        public async Task<GameRoom> CreateAsync(CancellationToken token, params Player[] players)
        {
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var game = new GameRoom();
                _stateMachineProvider.ChangeState(game, GameSessionTransitions.CREATE_GAME);

                await _db.GameRooms.AddAsync(game, token);
                await _gameRoomService.AddPlayersAsync(game, token, players);
                await _db.SaveChangesAsync(token);
                trans.Commit();

                return game;
            }
        }

        public async Task<GameRoom> StarGame(GameRoom game, CancellationToken token)
        {
            if (game.RoomPlayers.Count(r => !r.IsViewer) < _configuration.MinRoomPlayers())
            {
                throw new NotEnoughPlayerInGameSession();
            }
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                await _playerTurnsService.AddRangeAsync(token, _dealer.ChooseTurns(game).ToArray());
                await _cardPilesServices.AddRangeAsync(_dealer.DealtCards(game, _dealer.ShuffleCards()), token);
                _stateMachineProvider.ChangeState(game, GameSessionTransitions.START_GAME);
                if (game.From != GameState.PLAYING)
                {
                    throw new InvalidGameStateException();
                }
                game.NextTurn();
                await _db.SaveChangesAsync(token);
                trans.Commit();
                return game;
            }
        }
    }
}
