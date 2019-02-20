using System.Collections;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.DataAccess;
using GameSharp.Entities.Enums;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DataAccess;
using Snap.Services;
using Snap.Services.Abstract;

namespace Snap.Tests
{
    public static class ModuleHelper
    {
        public static async Task<ModuleManager> CreateModuleWithDefaults(IListRandomizer playerRandomizer = null,
            IListRandomizer cardRandomizer = null)
        {
            var module = new ModuleManager();
            module
                .ConfigureInMemorySql()
                .Configure(collection => collection.AddTransient<ISnapGameServices, SnapSnapGameServices>()
                    .AddScoped<GameSharpContext>(provider => provider.GetService<SnapDbContext>())
                    .AddTransient<IGameRoomPlayerServices, GameRoomPlayerServices>()
                    .AddTransient<ISnapGameConfigurationProvider, SnapGameConfigurationProvider>()
                    .AddTransient<IDealer>(provider => new Dealer(playerRandomizer ?? provider.GetService<IListRandomizer>(),
                        cardRandomizer ?? provider.GetService<IListRandomizer>()))
                    .AddTransient<IListRandomizer, ListRandomizer>()
                    .AddTransient<IPlayerTurnsService, PlayerTurnsService>()
                    .AddTransient<ICardPilesService, CardPilesService>()
                    .AddTransient(provider =>
                        new StateMachine<GameState, GameSessionTransitions>()
                            .AddTransition(GameState.NONE, GameState.AWAITING_PLAYERS,
                                GameSessionTransitions.CREATE_GAME)
                            .AddTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING,
                                GameSessionTransitions.START_GAME)
                            .AddTransition(GameState.PLAYING, GameState.FINISHED,
                                GameSessionTransitions.FINISH_GAME)
                            .AddTransition(GameState.PLAYING, GameState.ABORTED,
                                GameSessionTransitions.ABORT_GAME)));
            await module.GetService<SnapDbContext>().Database.EnsureCreatedAsync();
            return module;
        }
    }
}