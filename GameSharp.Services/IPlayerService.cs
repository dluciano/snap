using GameSharp.Entities;

namespace GameSharp.Services
{
    public interface IPlayerService
    {
        Player CurrentPlayer { get; }
    }
}