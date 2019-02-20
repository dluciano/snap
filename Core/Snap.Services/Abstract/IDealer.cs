using System.Collections.Generic;
using Snap.Entities;
using Snap.Entities.Enums;

namespace Snap.Services.Abstract
{
    public interface IDealer
    {
        IEnumerable<Card> ShuffleCards();
        IEnumerable<PlayerTurn> ChooseTurns(GameRoom game);
        IEnumerable<CardPileNode> DealtCards(GameRoom gameRoom, IEnumerable<Card> cards);
    }
}