using System.Threading;
using System.Threading.Tasks;
using GameSharp.Services.Abstract;
using Snap.Entities;

namespace Snap.Services.Abstract
{
    public interface IDealer :
        ICardDealter,
        ICardShuffler,
        IPlayerChooser
    {
        Task<PlayerGameplay> PopCurrentPlayerCardAsync(int gameId, CancellationToken token = default(CancellationToken));
    }
}