using System;
using Snap.Entities;

namespace Snap.Services.Notifications
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