using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerProvider _playerProvider;

        public PlayerController(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        [HttpPost]
        public async Task<ActionResult<Player>> PostAsync(CancellationToken token) =>
            (await _playerProvider.AddAsync(token));
    }
}
