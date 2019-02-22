using System.Collections.Generic;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface ICardShuffler
    {
        IEnumerable<Card> ShuffleCards();
    }
}