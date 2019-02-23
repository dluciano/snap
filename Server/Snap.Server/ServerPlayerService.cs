using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.AspNetCore.Http;
using Snap.DataAccess;
using Snap.Fakes;

namespace Snap.Server
{
    internal class ServerPlayerService : FakePlayerService
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
            return await _db.Players.FindAsync(claim.Value);
        }
    }
}