using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        public readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<ActionResult<Player>> PostAsync([FromBody] [NotNull] string username,
            CancellationToken token) =>
            (await _playerService.AddAsync(new Player
            {
                Username = username
            }, token));
    }
}
