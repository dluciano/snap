using System.Collections.Generic;
using Snap.Entities;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface ICardDealter
    {
        IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards);
    }
}