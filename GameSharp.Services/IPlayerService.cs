using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services
{
    public interface IPlayerService
    {
        Task<Player> GetCurrentPlayer();
    }
}