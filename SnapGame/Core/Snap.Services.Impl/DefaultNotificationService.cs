using System.Threading;
using System.Threading.Tasks;
using Snap.Services.Abstract;
using Snap.Services.Abstract.Notifications;

namespace Snap.Services.Impl
{
    internal sealed class DefaultNotificationService : INotificationService
    {
        public event AsyncEventHandler<GameStartedEvent> GameStartEvent;
        public event AsyncEventHandler<CardPopEvent> CardPopEvent;

        public async Task OnCardPop(object sender, CardPopEvent e, CancellationToken token = default(CancellationToken))
        {
            if (CardPopEvent != null)
                await CardPopEvent?.Invoke(sender, e, token);
        }

        public async Task OnGameStarted(object sender, GameStartedEvent e, CancellationToken token = default(CancellationToken))
        {
            if (GameStartEvent != null)
                await GameStartEvent?.Invoke(sender, e, token);
        }
    }
}