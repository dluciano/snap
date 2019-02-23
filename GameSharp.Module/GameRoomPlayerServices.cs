using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;

namespace GameSharp.Services.Impl
{
    internal sealed class GameRoomPlayerServices : IGameRoomPlayerServices
    {
        private readonly GameSharpContext _db;
        private readonly IPlayerService _playerService;

        public GameRoomPlayerServices(GameSharpContext db,
            IPlayerService playerService)
        {
            _db = db;
            _playerService = playerService;
        }

        public async Task<GameRoomPlayer> AddPlayersAsync(GameRoom room,
            bool isViewer,
            CancellationToken token)
        {
            //TODO: Implement this correctly
            if (!isViewer && !room.CanJoin)
            {
                //TODO: Make exception handling i18n
                throw new GameNotAcceptingMorePlayersException();
            }
            var creator = await _playerService.GetCurrentPlayerAsync();
            if (creator == null)
                throw new UnauthorizedCreateException();

            var entity = new GameRoomPlayer
            {
                GameRoom = room,
                Player = creator,
                IsViewer = isViewer
            };

            await _db.GameRoomPlayers.AddAsync(entity, token);
            await _db.SaveChangesAsync(token);
            return entity;
        }
    }
}