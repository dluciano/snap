using System.Linq;
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
    public class PlayerGameplayController : ControllerBase
    {
        private readonly IDealer _dealer;

        public PlayerGameplayController(IDealer dealer)
        {

            _dealer = dealer;
        }

        [HttpPost]
        public async Task<ActionResult<PlayerGameplay>> PostAsync([NotNull] [FromBody] int gameId, CancellationToken token) =>
            await _dealer.PopCurrentPlayerCardAsync(gameId, token);
    }
}
