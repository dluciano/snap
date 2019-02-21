using System;
using Snap.Services.Notifications;

namespace Snap.Services
{
    public class DefaultNotificationService : INotificationService
    {
        public event EventHandler<CardPopEvent> CardPopEvent;

        public void OnCardPop(object sender, CardPopEvent e) =>
            CardPopEvent?.Invoke(sender, e);
    }
}