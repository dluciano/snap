using Snap.Services.Abstract;

namespace Snap.Services.Impl
{
    public class SnapGameConfigurationProvider : ISnapGameConfigurationProvider
    {
        public int MinRoomPlayers()
        {
            return 1;
        }
    }
}