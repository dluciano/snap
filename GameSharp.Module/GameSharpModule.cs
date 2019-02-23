using Autofac;
using GameSharp.Services.Abstract;

namespace GameSharp.Services.Impl
{
    public sealed class GameSharpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameRoomPlayerServices>().As<IGameRoomPlayerServices>();
            builder.RegisterType<PlayerTurnsService>().As<IPlayerTurnsService>();
            builder.RegisterType<RandomPlayerChooser>().As<IPlayerChooser>();
            builder.RegisterType<GameRoomServices>().As<IGameRoomServices>();
        }
    }
}