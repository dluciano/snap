using System;
using Snap.Entities;

namespace Snap.Services.Impl.Notifications
{
    public class GameStartedEvent : EventArgs
    {
        public GameStartedEvent(SnapGame game)
        {
            Game = game;
        }

        public SnapGame Game { get; }
    }
}