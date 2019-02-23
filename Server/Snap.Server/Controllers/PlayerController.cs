using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;
using Snap.Fakes;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        public readonly IFakePlayerService _fakePlayerService;

        public PlayerController(IFakePlayerService fakePlayerService)
        {
            _fakePlayerService = fakePlayerService;
        }

        [HttpPost]
        public async Task<ActionResult<Player>> PostAsync([FromBody] [NotNull] string username,
            CancellationToken token) =>
            (await _fakePlayerService.AddRangeAsync(token, username)).Single();
    }
}
