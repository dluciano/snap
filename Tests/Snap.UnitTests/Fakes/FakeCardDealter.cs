using System.Collections.Generic;
using System.Linq;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Tests.Fakes
{
    internal class FakeCardDealter : ICardDealter
    {
        IEnumerable<StackNode> ICardDealter.DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards) =>
            playersStacks.SelectMany((stack, i) =>
            {
                var take = (cards.Count() / playersStacks.Count);
                var from = i * take;
                var c = cards.Skip(@from).Take(take)
                    .Select(card => StackNode.Create(card, stack));
                return c;
            });
    }
}