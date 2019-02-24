using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;
using Snap.Entities;
using Snap.Services.Abstract;

namespace Snap.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IDealer _dealer;

        public GameController(IDealer dealer)
        {
            _dealer = dealer;
        }

        [HttpPost("{gameId}/PlayerGameplay")]
        public async Task<ActionResult<PlayerGameplay>> PostAsync([NotNull] [FromRoute] int gameId, CancellationToken token) =>
            await _dealer.PopCurrentPlayerCardAsync(gameId, token);
    }
}