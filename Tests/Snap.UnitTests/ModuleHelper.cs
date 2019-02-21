using System.Collections.Generic;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.DataAccess;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services;
using Snap.Services.Abstract;

namespace Snap.Tests
{
    internal static class ModuleHelper
    {
        public static async Task<ModuleManager> CreateAndBuildWithDefaults() =>
            await new ModuleManager()
                .ConfigureDefault()
                .BuildAndCreateDatabase();

        public static async Task<ModuleManager> BuildAndCreateDatabase(this ModuleManager module)
        {
            await module.Build().GetService<SnapDbContext>().Database.EnsureCreatedAsync();
            return module;
        }

        internal static ModuleManager ConfigureDefault(this ModuleManager module)
        {
            module
                .ConfigureInMemorySql()
                .AddTransient<IListRandomizer, ListRandomizer>()
                .AddScoped<GameSharpContext>(provider => provider.GetService<SnapDbContext>())
                .AddTransient<ISnapGameServices, SnapSnapGameServices>()
                .AddTransient<IGameRoomPlayerServices, GameRoomPlayerServices>()
                .AddTransient<ISnapGameConfigurationProvider, SnapGameConfigurationProvider>()
                .AddTransient<IPlayerTurnsService, PlayerTurnsService>()
                .AddTransient<ICardPilesService, CardPilesService>()
                .AddTransient<IPlayerService, FakePlayerService>()
                .AddTransient<IPlayerRandomizer, DefaultRandomizer>()
                .AddTransient<ICardRandomizer, DefaultRandomizer>()
                .AddTransient<ICardDealter, DefaultCardDealter>()
                .AddTransient<IDealer, Dealer>()
                .AddTransient(provider => GameStateMachine());
            return module;
        }

        internal static ModuleManager WithFakePlayerRandomizer(this ModuleManager module, IEnumerable<Player> players) =>
            module.Configure(services =>
            {
                services.AddTransient<IPlayerRandomizer>(p => new FakeRandomizer<Player>(players));
            });

        internal static ModuleManager WithFakeCardRandomizer(this ModuleManager module, IEnumerable<Card> cards) =>
            module.Configure(services =>
            {
                services.AddTransient<ICardRandomizer>(p => new FakeRandomizer<Card>(cards));
            });

        internal static ModuleManager WithFakeDealter(this ModuleManager module) =>
            module.Configure(services =>
                    {
                        services.AddTransient<ICardDealter, FakeCardDealter>();
                    });

        internal static ModuleManager WithFakePlayerService(this ModuleManager module, Player loggedPlayer) =>
            module.Configure(services =>
                {
                    services.AddScoped<IPlayerService>(p => new FakePlayerService
                    {
                        Player = loggedPlayer
                    });
                });

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