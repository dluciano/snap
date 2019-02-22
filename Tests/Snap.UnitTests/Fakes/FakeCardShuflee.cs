using System;
using System.Collections.Generic;
using System.Linq;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Tests.Fakes
{
    internal class FakeCardShuflee : ICardShuffler
    {
        public IEnumerable<Card> ShuffleCards() =>
            Enum
                .GetValues(typeof(Card))
                .Cast<Card>()
                .ToList();
    }
}