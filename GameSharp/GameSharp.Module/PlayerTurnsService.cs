using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Services.Abstract;

namespace GameSharp.Services.Impl
{
    internal sealed class PlayerTurnsService : IPlayerTurnsService
    {
        private readonly GameSharpContext _db;

        public PlayerTurnsService(GameSharpContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PlayerTurn>> PushListAsync(IEnumerable<PlayerTurn> turns, CancellationToken token)
        {
            PlayerTurn lastPlayerTurn = null;
            var t = turns.ToList();
            t.ToList().ForEach(p =>
           {
               if (lastPlayerTurn != null)
                   lastPlayerTurn.Next = p;
               lastPlayerTurn = p;

           });
            await _db.PlayerTurns.AddRangeAsync(t, token);
            await _db.SaveChangesAsync(token);
            return t;
        }
    }
}