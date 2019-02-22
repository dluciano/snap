using System.Collections.Generic;
using System.Linq;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Services.Abstract;

namespace GameSharp.Services.Impl
{
    internal sealed class RandomPlayerChooser : IPlayerChooser
    {
        private readonly IListRandomizer _randomizer;

        public RandomPlayerChooser(IListRandomizer randomizer)
        {
            _randomizer = randomizer;
        }
        public IEnumerable<PlayerTurn> ChooseTurns(GameData game)
        {
            PlayerTurn lastPlayerTurn = null;
            return _randomizer
                .Generate(game
                    .GameRoom.RoomPlayers
                    .Where(p => !p.IsViewer).Select(p => p.Player))
                .Select(p =>
                {
                    var newTurn = new PlayerTurn
                    {
                        GameData = game,
                        Player = p
                    };
                    if (game.FirstPlayer == null) game.FirstPlayer = newTurn;
                    if (lastPlayerTurn != null) lastPlayerTurn.Next = newTurn;
                    return lastPlayerTurn = newTurn;
                });
        }
    }
}
