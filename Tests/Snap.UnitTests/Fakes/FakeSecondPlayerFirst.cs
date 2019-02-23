using System.Collections.Generic;
using System.Linq;
using GameSharp.Entities;
using GameSharp.Services.Abstract;
using Snap.Fakes;

namespace Snap.Tests.Fakes
{
    internal sealed class FakeSecondPlayerFirst : IPlayerChooser
    {
        private readonly IFakePlayerService _playerService;

        public FakeSecondPlayerFirst(IFakePlayerService playerService)
        {
            _playerService = playerService;
        }

        public IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players)
        {
            var playersInverted = _playerService.GetPlayers().ToList();
            playersInverted.Reverse();
            return playersInverted.Select(p => new PlayerTurn
            {
                Player = p
            });
        }
    }
}