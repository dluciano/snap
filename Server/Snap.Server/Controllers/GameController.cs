using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var room = await _db.FindAsync<GameRoom>(roomId);
            if (room == null)
            {
                ModelState.AddModelError(nameof(roomId), $"The {nameof(roomId)} is required");
                return BadRequest();
            }

            var game = _db.SnapGames.Single(s => s.GameData.GameRoom.Id == room.Id);
            return await _service.StarGameAsync(game, token);
        }
    }
}
