using System;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;

namespace GameSharp.Services.Impl
{
    internal sealed class GameRoomServices : IGameRoomServices
    {
        private readonly GameSharpContext _db;
        private readonly IPlayerProvider _playerProvider;
        private readonly IGameRoomPlayerServices _roomPlayerServices;
        public event AsyncEventHandler<GameRoom> OnRoomCreatedEvent;

        public GameRoomServices(GameSharpContext db,
            IPlayerProvider playerProvider,
            IGameRoomPlayerServices roomPlayerServices)
        {
            _db = db;
            _playerProvider = playerProvider;
            _roomPlayerServices = roomPlayerServices;
        }

        public async Task<GameRoom> CreateAsync(CancellationToken token = default(CancellationToken))
        {
            using (var tran = await _db.Database.BeginTransactionAsync(token))
            {
                var creator = await _playerProvider.GetCurrentPlayerAsync();
                if (creator == null)
                    throw new UnauthorizedCreateException();

                var room = new GameRoom
                {
                    CanJoin = true,
                    GameIdentifier = Guid.NewGuid(),
                    CreatedBy = creator
                };
                await _db.GameRooms.AddAsync(room, token);
                await _db.SaveChangesAsync(token);

                await _roomPlayerServices.AddPlayersAsync(room.Id, false, token);
                //TODO: test this and all other notifications
                OnRoomCreatedEvent?.Invoke(this, room, token);
                token.ThrowIfCancellationRequested();
                tran.Commit();
                return room;
            }
        }
    }
}