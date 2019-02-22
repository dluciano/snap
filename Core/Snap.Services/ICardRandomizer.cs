using Dawlin.Util;
using System.Collections.Generic;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface ICardRandomizer
    {
        IEnumerable<Card> ShuffleCards();
    }
}