using Snap.Entities;
using System.Collections.Generic;
using System.Linq;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Services.Impl
{
    internal sealed class CardDealter : ICardDealter
    {
        public IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards)
        {
            return cards.Select((card, index) => StackNode.Create(card, playersStacks[index % playersStacks.Count]));
        }
    }
}