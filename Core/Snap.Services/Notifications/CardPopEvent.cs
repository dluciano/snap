using System;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Notifications
{
    public class CardPopEvent : EventArgs
    {
        public PlayerGameplay GamePlay { get; }
        public PlayerData NextPlayer { get; }

        public CardPopEvent(PlayerGameplay gamePlay,
            PlayerData nextPlayer)
        {
            GamePlay = gamePlay;
            NextPlayer = nextPlayer;
        }
    }
}