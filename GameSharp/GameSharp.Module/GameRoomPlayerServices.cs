using System.Threading;
using System.Threading.Tasks;
using Dawlin.Abstract.Entities.Exceptions;
using Dawlin.Util.Abstract;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Services.Impl
{
    internal sealed class GameRoomPlayerServices : IGameRoomPlayerServices
    {
        private readonly GameSharpContext _db;
        private readonly IPlayerProvider _playerProvider;
        public event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinedEvent;
        public GameRoomPlayerServices(GameSharpContext db,
            IPlayerProvider playerProvider)
        {
            _db = db;
            _playerProvider = playerProvider;
        }

        public async Task<GameRoomPlayer> AddPlayersAsync(int roomId,
            bool isViewer,
            CancellationToken token = default(CancellationToken))
        {
            var room = await _db.GameRooms.
                Include(p => p.RoomPlayers)
                .SingleOrDefaultAsync(p => p.Id == roomId, token);
            if (room == null)
            {
                throw new EntityNotFoundException("The room does not exists");
            }

            //TODO: Implement this correctly
            if (!isViewer && !room.CanJoin)
            {
                //TODO: Make exception handling i18n
                throw new GameNotAcceptingMorePlayersException();
            }
            var creator = await _playerProvider.GetCurrentPlayerAsync();
            if (creator == null)
                throw new UnauthorizedCreateException();

            var entity = new GameRoomPlayer
            {
                GameRoom = room,
                Player = creator,
                IsViewer = isViewer
            };

            await _db.GameRoomPlayers.AddAsync(entity, token);
            if (OnPlayerJoinedEvent != null)
                await OnPlayerJoinedEvent?.Invoke(this, entity, token);
            await _db.SaveChangesAsync(token);
            return entity;
        }
    }
}