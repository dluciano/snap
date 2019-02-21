using System;

namespace Snap.Services.Notifications
{
    public interface INotificationService
    {
        event EventHandler<CardPopEvent> CardPopEvent;
        void OnCardPop(object sender, CardPopEvent e);
    }
}