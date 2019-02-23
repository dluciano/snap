using System.Threading;
using System.Threading.Tasks;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;

namespace GameSharp.Services.Impl
{
    internal sealed class GameRoomServices : IGameRoomServices
    {
        private readonly GameSharpContext _db;
        private readonly IPlayerService _playerService;
        private readonly IGameRoomPlayerServices _roomPlayerServices;

        public GameRoomServices(GameSharpContext db,
            IPlayerService playerService,
            IGameRoomPlayerServices roomPlayerServices)
        {
            _db = db;
            _playerService = playerService;
            _roomPlayerServices = roomPlayerServices;
        }

        public async Task<GameRoom> CreateAsync(CancellationToken token)
        {
            using (var tran = await _db.Database.BeginTransactionAsync(token))
            {
                var creator = await _playerService.GetCurrentPlayerAsync();
                if (creator == null)
                    throw new UnauthorizedCreateException();

                var entity = new GameRoom
                {
                    CanJoin = true
                };
                await _db.GameRooms.AddAsync(entity, token);
                await _db.SaveChangesAsync(token);

                await _roomPlayerServices.AddPlayersAsync(entity, false, token);
                tran.Commit();
                return entity;
            }
        }
    }
}