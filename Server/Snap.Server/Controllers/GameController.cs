using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Annotations;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ISnapGameServices _service;
        private readonly SnapDbContext _db;

        public GameController(ISnapGameServices service,
            SnapDbContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<SnapGame>> PostAsync([NotNull] [FromBody] int roomId, CancellationToken token)
        {
            var room = await _db.GameRooms
                .Include(p => p.RoomPlayers)
                .SingleOrDefaultAsync(p => p.Id == roomId, token);
            if (room != null)
                return await _service.StarGameAsync(room, token);
            return BadRequest();
        }
    }
}
