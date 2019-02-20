using Snap.Entities;
using System.Linq;
using System.Threading;
using Snap.DataAccess;
using System.Threading.Tasks;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services;
using GameSharp.Services.Exceptions;
using Snap.Services.Exceptions;

namespace Snap.Services
{
    public class SnapSnapGameServices : ISnapGameServices
    {
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly SnapDbContext _db;
        private readonly ISnapGameConfigurationProvider _configuration;
        private readonly IDealer _dealer;
        private readonly IPlayerTurnsService _playerTurnsService;
        private readonly ICardPilesService _cardPilesServices;
        private readonly IStateMachineProvider<GameState, GameSessionTransitions> _stateMachineProvider;

        public SnapSnapGameServices(IGameRoomPlayerServices gameRoomService,
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
        public async Task<SnapGame> CreateAsync(CancellationToken token, params Player[] players)
        {
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var game = new SnapGame
                {
                    GameData = new GameData
                    {
                        GameRoom = new GameRoom()
                    },
                    CentralPile = new StackEntity()
                };

                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.CREATE_GAME);

                await _db.SnapGames.AddAsync(game, token);
                await _gameRoomService.AddPlayersAsync(game.GameData, token, players);
                await _db.SaveChangesAsync(token);
                trans.Commit();

                return game;
            }
        }

        public async Task<SnapGame> StarGame(SnapGame game, CancellationToken token)
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
                await _playerTurnsService.AddRangeAsync(token, _dealer.ChooseTurns(game.GameData).ToArray());
                await _cardPilesServices.AddRangeAsync(_dealer.DealtCards(game, _dealer.ShuffleCards()), token);
                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.START_GAME);
                if (game.GameData.From != GameState.PLAYING)
                {
                    throw new InvalidGameStateException();
                }
                game.GameData.NextTurn();
                await _db.SaveChangesAsync(token);
                trans.Commit();
                return game;
            }
        }
    }
}
