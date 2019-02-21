using System;
using Snap.Services.Notifications;

namespace Snap.Services
{
    public class DefaultNotificationService : INotificationService
    {
        public event EventHandler<CardPopEvent> CardPopEvent;
        public event EventHandler<GameStartedEvent> GameStartEvent;

        public void OnCardPop(object sender, CardPopEvent e) =>
            CardPopEvent?.Invoke(sender, e);

        public void OnGameStarted(object sender, GameStartedEvent e) =>
            GameStartEvent?.Invoke(sender, e);
    }
}