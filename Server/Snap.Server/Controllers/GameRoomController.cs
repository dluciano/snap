using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.DataAccess;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private readonly IGameRoomServices _roomService;
        private readonly SnapDbContext _db;
        private readonly IGameRoomPlayerServices _gameRoomService;

        public GameRoomController(IGameRoomServices roomService,
            SnapDbContext db,
            IGameRoomPlayerServices gameRoomService)
        {
            _roomService = roomService;
            _db = db;
            _gameRoomService = gameRoomService;
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> PostAsync(CancellationToken token) =>
            (await _roomService.CreateAsync(token));

        [HttpPost("{id}/Players")]
        public async Task<ActionResult<GameRoomPlayer>> PostPlayerAsync([FromRoute]int id,
            bool isViewer,
            CancellationToken token)
        {
            var room = await _db.FindAsync<GameRoom>(id, token);
            if (room == null)
            {
                return NotFound();
            }
            return await _gameRoomService.AddPlayersAsync(room, isViewer, token);
        }
    }
}
