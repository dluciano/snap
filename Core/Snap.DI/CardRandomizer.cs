using System;
using System.Collections.Generic;
using System.Linq;
using Dawlin.Util;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.DI
{
    internal class CardRandomizer : ICardRandomizer
    {
        private readonly IListRandomizer _randomizer;

        public CardRandomizer(IListRandomizer randomizer)
        {
            _randomizer = randomizer;
        }
        public IEnumerable<Card> ShuffleCards() =>
            _randomizer.Generate(Enum.GetValues(typeof(Card)).Cast<Card>());
    }
}