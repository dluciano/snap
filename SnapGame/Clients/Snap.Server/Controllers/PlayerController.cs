using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NJsonSchema.Annotations;
using NSwag.Annotations;
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

        [SwaggerOperation("getPlayerById")]
        [HttpGet("/me")]
        public async Task<ActionResult<Player>> GetPlayerAsync(CancellationToken token)
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();
            if (player == null)
                return NotFound();
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
