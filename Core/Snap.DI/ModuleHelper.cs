using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.DataAccess;
using GameSharp.Entities.Enums;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;
using Snap.Fakes;
using Snap.Services;
using Snap.Services.Abstract;
using Snap.Services.Impl;
using Snap.Services.Notifications;

namespace Snap.DI
{
    public static class ModuleHelper
    {
        public static async Task<SnapModuleManager> BuildAndCreateDatabaseAsync(this SnapModuleManager snapModule)
        {
            await snapModule.Build().GetService<SnapDbContext>().Database.EnsureCreatedAsync();
            return snapModule;
        }

        public static SnapModuleManager WithDefaults(this SnapModuleManager snapModule)
        {
            snapModule
                .ConfigureInMemorySql()
                .AddTransient<IListRandomizer, ListRandomizer>()
                .AddScoped<GameSharpContext>(provider => provider.GetService<SnapDbContext>())
                .AddTransient<ISnapGameServices, SnapSnapGameServices>()
                .AddTransient<IGameRoomPlayerServices, GameRoomPlayerServices>()
                .AddTransient<ISnapGameConfigurationProvider, SnapGameConfigurationProvider>()
                .AddTransient<IPlayerTurnsService, PlayerTurnsService>()
                .AddTransient<ICardPilesService, CardPilesService>()
                .AddTransient<IPlayerRandomizer, DefaultRandomizer>()
                .AddTransient<ICardRandomizer, DefaultRandomizer>()
                .AddTransient<ICardDealter, DefaultCardDealter>()
                .AddTransient<IDealer, Dealer>()
                .AddScoped<INotificationService, DefaultNotificationService>()
                .AddTransient(provider => GameStateMachine());
            return snapModule;
        }

        public static SnapModuleManager WithFakes(this SnapModuleManager snapModule) => snapModule
                .Configure(service => service
                    .AddScoped<IPlayerService, FakePlayerService>()
                    .AddScoped<IFakePlayerService>(s => (FakePlayerService)s.GetService<IPlayerService>()));

        private static IStateMachineProvider<GameState, GameSessionTransitions> GameStateMachine() =>
            new StateMachine<GameState, GameSessionTransitions>()
                .AddTransition(GameState.NONE, GameState.AWAITING_PLAYERS,
                    GameSessionTransitions.CREATE_GAME)
                .AddTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING,
                    GameSessionTransitions.START_GAME)
                .AddTransition(GameState.PLAYING, GameState.FINISHED,
                    GameSessionTransitions.FINISH_GAME)
                .AddTransition(GameState.PLAYING, GameState.ABORTED,
                    GameSessionTransitions.ABORT_GAME);
    }
}