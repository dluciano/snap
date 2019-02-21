using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Microsoft.AspNetCore.Mvc;
using Snap.DataAccess;
using Snap.Services.Abstract;

namespace Snap.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private readonly ISnapGameServices _service;
        private readonly SnapDbContext _db;
        private readonly IGameRoomPlayerServices _gameRoomService;

        public GameRoomController(ISnapGameServices service,
            SnapDbContext db,
            IGameRoomPlayerServices gameRoomService)
        {
            _service = service;
            _db = db;
            _gameRoomService = gameRoomService;
        }

        [HttpPost]
        public async Task<ActionResult<GameRoom>> PostAsync(CancellationToken token) =>
            (await _service.CreateAsync(token)).GameData.GameRoom;

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
