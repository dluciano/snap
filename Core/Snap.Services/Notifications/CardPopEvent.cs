using System;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Services.Notifications
{
    public class CardPopEvent : EventArgs
    {
        public PlayerGameplay GamePlay { get; }
        public PlayerTurn NextPlayer { get; }

        public CardPopEvent(PlayerGameplay gamePlay,
            PlayerTurn nextPlayer)
        {
            GamePlay = gamePlay;
            NextPlayer = nextPlayer;
        }
    }
}