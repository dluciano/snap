using Autofac;
using Dawlin.Util.Abstract;
using Dawlin.Util.Impl;
using GameSharp.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Impl.Notifications;

namespace Snap.Services.Impl
{
    public sealed class SnapGameModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CardShuffler>().As<ICardShuffler>();
            builder.RegisterType<CardDealter>().As<ICardDealter>();
            builder.RegisterType<Dealer>().As<IDealer>();

            builder.RegisterType<SnapSnapGameServices>().As<ISnapGameServices>();
            builder.RegisterType<SnapGameConfigurationProvider>().As<ISnapGameConfigurationProvider>();
            builder.RegisterType<CardPilesService>().As<ICardPilesService>();
            builder.RegisterType<DefaultNotificationService>().As<INotificationService>();
            builder.Register(context => new StateMachine<GameState, GameSessionTransitions>()
                    .AddTransition(GameState.NONE, GameState.AWAITING_PLAYERS,
                        GameSessionTransitions.CREATE_GAME)
                    .AddTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING,
                        GameSessionTransitions.START_GAME)
                    .AddTransition(GameState.PLAYING, GameState.FINISHED,
                        GameSessionTransitions.FINISH_GAME)
                    .AddTransition(GameState.PLAYING, GameState.ABORTED,
                        GameSessionTransitions.ABORT_GAME))
                .As<IStateMachineProvider<GameState, GameSessionTransitions>>();
        }
    }
}