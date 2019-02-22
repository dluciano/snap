using System.Threading.Tasks;
using GameSharp.Entities;

namespace GameSharp.Services.Abstract
{
    public interface IPlayerService
    {
        Task<Player> GetCurrentPlayerAsync();
    }
}