using System.Collections.Generic;
using System.Linq;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class DefaultCardDealter : ICardDealter
    {
        public IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards) =>
            cards.Select((card, index) => StackNode.Create(card, playersStacks[index % playersStacks.Count]));
    }
}