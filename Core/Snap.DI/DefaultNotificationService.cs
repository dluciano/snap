using System;
using Snap.Services.Impl.Notifications;

namespace Snap.Services.Impl
{
    internal sealed class DefaultNotificationService : INotificationService
    {
        public event EventHandler<CardPopEvent> CardPopEvent;
        public event EventHandler<GameStartedEvent> GameStartEvent;

        public void OnCardPop(object sender, CardPopEvent e) =>
            CardPopEvent?.Invoke(sender, e);

        public void OnGameStarted(object sender, GameStartedEvent e) =>
            GameStartEvent?.Invoke(sender, e);
    }
}