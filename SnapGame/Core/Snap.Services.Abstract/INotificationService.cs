using Snap.Services.Abstract.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Snap.Services.Abstract
{
    public delegate Task AsyncEventHandler<in T>(object sender, T args, CancellationToken token = default(CancellationToken));
    public interface INotificationService
    {
        event AsyncEventHandler<GameStartedEvent> GameStartEvent;
        event AsyncEventHandler<CardPopEvent> CardPopEvent;
        Task OnGameStarted(object sender, GameStartedEvent e, CancellationToken token = default(CancellationToken));
        Task OnCardPop(object sender, CardPopEvent e, CancellationToken token = default(CancellationToken));
    }
}