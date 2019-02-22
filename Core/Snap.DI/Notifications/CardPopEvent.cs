﻿using System;
using Snap.Entities;

namespace Snap.Services.Impl.Notifications
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