using Snap.Services.Abstract;

namespace Snap.Services
{
    public class SnapGameConfigurationProvider : ISnapGameConfigurationProvider
    {
        public int MinRoomPlayers()
        {
            //TODO: This can come from a db or from a the config file
            throw new System.NotImplementedException();
        }
    }
}