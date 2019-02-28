using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Annotations;
using Snap.DataAccess;

namespace Snap.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private readonly SnapDbContext _db;

        public GameRoomController(SnapDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<GameRoom>> GetAllAsync(CancellationToken token) =>
            Ok(await _db.GameRooms.ToListAsync(token));

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<GameRoom>> GetAsync([NotNull][FromRoute]int id, CancellationToken token)
        {
            var room = await _db
                .GameRooms
                .Include(gr => gr.RoomPlayers)
                .ThenInclude(rp => rp.Player)
                .SingleOrDefaultAsync(r => r.Id == id, token);
            if (room == null)
                return NotFound();
            return Ok(room);
        }
    }
}
