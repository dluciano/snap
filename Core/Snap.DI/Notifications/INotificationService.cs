﻿using System;

namespace Snap.Services.Impl.Notifications
{
    public interface INotificationService
    {
        event EventHandler<GameStartedEvent> GameStartEvent;
        event EventHandler<CardPopEvent> CardPopEvent;
        void OnGameStarted(object sender, GameStartedEvent e);
        void OnCardPop(object sender, CardPopEvent e);
    }
}