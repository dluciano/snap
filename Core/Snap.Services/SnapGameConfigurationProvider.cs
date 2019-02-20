using Snap.Services.Abstract;

namespace Snap.Services
{
    public class SnapGameConfigurationProvider : ISnapGameConfigurationProvider
    {
        public int MinRoomPlayers()
        {
            return 1;
        }
    }
}