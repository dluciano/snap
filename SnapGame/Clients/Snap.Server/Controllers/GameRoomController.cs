using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.Annotations;
using NSwag;
using NSwag.Annotations;
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
        public async Task<IEnumerable<GameRoom>> GetAllAsync(CancellationToken token) =>
            await _db.GameRooms.ToListAsync(token);

        [HttpGet, Route("{id}")]
        [SwaggerResponse("200", typeof(GameRoom))]
        public async Task<GameRoom> GetAsync([NotNull][FromRoute]int id, CancellationToken token)
        {
            var room = await _db
                .GameRooms
                .Include(gr => gr.RoomPlayers)
                .ThenInclude(rp => rp.Player)
                .SingleOrDefaultAsync(r => r.Id == id, token);
            if (room != null) return room;
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return null;
        }
    }
}
