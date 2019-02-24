using System.Threading.Tasks;
using GameSharp.Entities;
using Snap.Entities;

namespace Snap.Tests.Helpers
{
    public interface IBackgroundHelper
    {
        Task<GameRoom> CreateRoomAsync();

        Task<GameRoomPlayer> PlayerJoinAsync(GameRoom room,
            string username = PlayerServiceSeedHelper.SecondPlayerUsername);

        Task<SnapGame> StartGameAsync(GameRoom room);
    }
}