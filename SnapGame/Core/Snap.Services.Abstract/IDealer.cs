using System.Threading;
using System.Threading.Tasks;
using Dawlin.Util.Abstract;
using GameSharp.Services.Abstract;
using Snap.Entities;
using Snap.Services.Abstract.Notifications;

namespace Snap.Services.Abstract
{
    public interface IDealer :
        ICardDealter,
        ICardShuffler,
        IPlayerChooser
    {
        Task<PlayerGameplay> PopCurrentPlayerCardAsync(int gameId, CancellationToken token = default(CancellationToken));
        Task<bool> Snap(int gameId, CancellationToken token = default(CancellationToken));
        event AsyncEventHandler<CardPopEvent> OnCardPopEvent;
        event AsyncEventHandler<CardSnapEvent> OnSnap;
    }
}