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
            var game = await _db
                .SnapGames
                .Include(g => g.CentralPile.Last)
                .ThenInclude(p => p.Previous)
                .Include(g => g.GameData)
                .ThenInclude(gd => gd.CurrentTurn)
                .ThenInclude(pt => pt.Next)
                .Include(p => p.GameData.CurrentTurn.Player)
                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.StackEntity.Last)
                .ThenInclude(n => n.Previous)
                .SingleOrDefaultAsync(p => p.Id == gameId, token);
            if (game == null)
            {
                ModelState.AddModelError(nameof(gameId), $"The {nameof(gameId)} is required");
                return BadRequest();
            }
            return await _dealer.PopCurrentPlayerCardAsync(game, token);
        }
    }
}
