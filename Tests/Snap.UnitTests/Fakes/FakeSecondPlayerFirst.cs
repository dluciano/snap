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

        public IEnumerable<PlayerTurn> ChooseTurns(GameData game)
        {
            PlayerTurn lastPlayerTurn = null;
            var playersInverted = _playerService.GetPlayers().ToList();
            playersInverted.Reverse();
            var result = playersInverted.Select(p =>
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
            return result;
        }
    }
}