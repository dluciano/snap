using System.Collections.Generic;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IPlayerChooser
    {
        IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players);
    }
}