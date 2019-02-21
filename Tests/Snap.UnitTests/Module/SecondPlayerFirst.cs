using System.Collections.Generic;
using System.Linq;
using Snap.Fakes;
using Snap.Services.Abstract;

namespace Snap.Tests.Module
{
    internal class SecondPlayerFirst : IPlayerRandomizer
    {
        private readonly IFakePlayerService _playerService;

        public SecondPlayerFirst(IFakePlayerService playerService)
        {
            _playerService = playerService;
        }
        public IEnumerable<T> Generate<T>(IEnumerable<T> list) =>
            _playerService.GetPlayers().Result.Reverse().Cast<T>();
    }
}