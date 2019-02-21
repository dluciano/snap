using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DI;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Tests
{
    public static class TestModuleHelpers
    {
        public static async Task<ModuleManager> CreateAndBuildWithDefaults() =>
            await new ModuleManager()
                .UseDefaults()
                .BuildAndCreateDatabase();

        public static ModuleManager UseDefaults(this ModuleManager module)
            => module
                .WithDefaults()
                .Configure(service => service.AddTransient<IPlayerService, FakePlayerService>());

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
    }
}
