using System.Linq;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;

namespace Snap.Server.Provider
{
    internal class ServerPlayerProvider : PlayerProviderBase
    {
        private readonly IHttpContextAccessor _httpContext;

        public ServerPlayerProvider(SnapDbContext db,
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