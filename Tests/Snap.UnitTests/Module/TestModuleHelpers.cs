using System.Collections.Generic;
using System.Threading.Tasks;
using GameSharp.Entities;
using Microsoft.Extensions.DependencyInjection;
using Snap.DI;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Tests.Fakes;

namespace Snap.Tests.Module
{
    internal static class TestModuleHelpers
    {
        internal static async Task<SnapModuleManager> BuildWithDefaults(this SnapModuleManager snapModule) =>
            await snapModule
                .WithDefaults()
                .WithFakes()
                .BuildAndCreateDatabaseAsync();

        internal static async Task<SnapModuleManager> CreateAndBuildWithDefaultsAsync() =>
            await new SnapModuleManager().BuildWithDefaults();

        internal static SnapModuleManager WithSecondPlayerFirstRandomizer(this SnapModuleManager snapModule) =>
               snapModule.Configure(services =>
               {
                   services.AddTransient<IPlayerRandomizer, SecondPlayerFirst>();
               });

        internal static SnapModuleManager WithFakeCardRandomizer(this SnapModuleManager snapModule, IEnumerable<Card> cards) =>
            snapModule.Configure(services =>
            {
                services.AddTransient<ICardRandomizer>(p => new FakeRandomizer<Card>(cards));
            });

        internal static SnapModuleManager WithFakeDealter(this SnapModuleManager snapModule) =>
            snapModule.Configure(services =>
            {
                services.AddTransient<ICardDealter, FakeCardDealter>();
            });
    }
}
