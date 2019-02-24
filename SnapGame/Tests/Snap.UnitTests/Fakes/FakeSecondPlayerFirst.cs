using System.Collections.Generic;
using System.Linq;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Snap.Fakes;

namespace Snap.Tests.Fakes
{
    internal sealed class FakeSecondPlayerFirst : IPlayerChooser
    {
        private readonly IFakePlayerProvider _playerProvider;

        public FakeSecondPlayerFirst(IFakePlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players)
        {
            var playersInverted = _playerProvider.GetPlayers().ToList();
            playersInverted.Reverse();
            return playersInverted.Select(p => new PlayerTurn
            {
                Player = p
            });
        }
    }
}