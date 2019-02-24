using System.Collections.Generic;
using System.Linq;
using Dawlin.Util.Abstract;
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

        public IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players) =>
            _randomizer
                .Generate(players)
                .Select(p => new PlayerTurn
                {
                    Player = p
                });
    }
}