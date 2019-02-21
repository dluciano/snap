using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface IDealer
    {
        IEnumerable<Card> ShuffleCards();
        IEnumerable<PlayerTurn> ChooseTurns(GameData game);
        IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards);
        Task<PlayerGameplay> PopCurrentPlayerCardAsync(SnapGame game, CancellationToken token);
    }
}