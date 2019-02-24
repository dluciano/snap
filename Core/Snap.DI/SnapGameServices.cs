using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Abstract.Entities.Exceptions;
using Dawlin.Util.Abstract;
using GameSharp.Entities.Enums;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPlayerProvider _playerProvider;

        public SnapGameServices(ISnapGameConfigurationProvider configuration,
            IDealer dealer,
            IPlayerTurnsService playerTurnsService,
            ICardPilesService cardPilesServices,
            IStateMachineProvider<GameState, GameSessionTransitions> stateMachineProvider,
            SnapDbContext db,
            INotificationService notificationService,
            IPlayerProvider playerProvider)
        {
            _db = db;
            _notificationService = notificationService;
            _playerProvider = playerProvider;
            _stateMachineProvider = stateMachineProvider;
            _configuration = configuration;
            _dealer = dealer;
            _playerTurnsService = playerTurnsService;
            _cardPilesServices = cardPilesServices;
        }

        public async Task<SnapGame> StarGameAsync(int roomId, CancellationToken token = default(CancellationToken))
        {
            var room = await _db.GameRooms
                .Include(p => p.RoomPlayers)
                .ThenInclude(rp => rp.Player)
                .SingleOrDefaultAsync(p => p.Id == roomId, token);
            if (room == null)
            {
                throw new EntityNotFoundException("The room do not exists");
            }

            //TODO: Validate that only players can start the game
            var creator = await _playerProvider.GetCurrentPlayerAsync();
            if (creator == null)
                throw new UnauthorizedCreateException();

            if (room.RoomPlayers.Count(r => !r.IsViewer) < _configuration.MinRoomPlayers())
                throw new NotEnoughPlayerInGameSession();

            if (!room.CanJoin)
                throw new GameAlreadyStartedException();

            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var game = new SnapGame
                {
                    CentralPile = new StackEntity(),
                    GameData = new SnapGameData
                    {
                        Room = room
                    }
                };
                _stateMachineProvider.ChangeState(game.GameData, GameSessionTransitions.START_GAME);

                var turns = _dealer
                    .ChooseTurns(room.RoomPlayers.Where(p => !p.IsViewer)
                        .Select(p => p.Player));

                turns = await _playerTurnsService.PushListAsync(turns, token);
                game.GameData.FirstPlayer = turns.First();

                await _db.PlayersData.AddRangeAsync(turns.Select(pt => new PlayerData
                {
                    SnapGame = game,
                    PlayerTurn = pt
                }), CancellationToken.None);

                var shuffledCards = _dealer.ShuffleCards();

                await _cardPilesServices.AddRangeAsync(_dealer.DealtCards(game.PlayersData.Select(p => p.StackEntity).ToList(), shuffledCards), token);

                room.CanJoin = false;

                if (game.GameData.CurrentState != GameState.PLAYING)
                    throw new InvalidGameStateException();

                game.GameData.NextTurn();
                await _db.SaveChangesAsync(token);

                await _notificationService?.OnGameStarted(this, new GameStartedEvent(game), token);

                trans.Commit();
                return game;
            }
        }

    }
}