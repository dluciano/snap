using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Server
{
    internal class ServerPlayerService : PlayerServiceBase
    {
        private readonly IHttpContextAccessor _httpContext;

        public ServerPlayerService(SnapDbContext db,
            IHttpContextAccessor httpContext)
            : base(db)
        {
            _httpContext = httpContext;
        }

        public override async Task<Player> GetCurrentPlayerAsync()
        {
            var claims = _httpContext.HttpContext.User.Claims;
            var claim = claims.Single(c => c.Type == "email");
            var playerDb = await _db.Players.SingleOrDefaultAsync(p => p.Username == claim.Value);
            return playerDb ?? new Player
            {
                Username = claim.Value
            };
        }
    }
}