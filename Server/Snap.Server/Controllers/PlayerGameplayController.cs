using System.Threading;
using System.Threading.Tasks;
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
    public class PlayerGameplayController : ControllerBase
    {
        private readonly SnapDbContext _db;
        private readonly IDealer _dealer;

        public PlayerGameplayController(SnapDbContext db, IDealer dealer)
        {
            _db = db;
            _dealer = dealer;
        }

        [HttpPost]
        public async Task<ActionResult<PlayerGameplay>> PostAsync([NotNull] [FromBody] int gameId, CancellationToken token)
        {
            var game = await _db.FindAsync<SnapGame>(gameId);
            if (game == null)
            {
                ModelState.AddModelError(nameof(gameId), $"The {nameof(gameId)} is required");
                return BadRequest();
            }
            return await _dealer.PopCurrentPlayerCardAsync(game, token);
        }
    }
}
