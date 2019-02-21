using System;
using GameSharp.Entities;
using GameSharp.Services;

namespace Snap.Tests
{
    internal class FakePlayerService : IPlayerService
    {
        public Player Player { private get; set; }
        public Player CurrentPlayer => Player;
    }
}