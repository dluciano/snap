using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;
using Snap.DataAccess;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly SnapDbContext _db;

        public PlayerController(IPlayerProvider playerProvider,
            SnapDbContext db)
        {
            _playerProvider = playerProvider;
            _db = db;
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Player>> Get([NotNull] [FromRoute] int id, CancellationToken token)
        {
            var player = _db.Players.Find(id);
            if (player == null)
                return NotFound();
            if (player.Id != id)
            {
                return Unauthorized();
            }
            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> PostAsync(CancellationToken token)
        {
            var player = await _playerProvider.AddAsync(token);
            return Created(Url.Action($"Get/{player.Id}"), player);
        }
    }
}
