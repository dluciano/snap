using System.Collections.Generic;
using GameSharp.Entities;
using Snap.Entities;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface IDealer
    {
        IEnumerable<Card> ShuffleCards();
        IEnumerable<PlayerTurn> ChooseTurns(GameData game);
        IEnumerable<StackNode> DealtCards(SnapGame game, IEnumerable<Card> cards);
    }
}