﻿using System.Reflection;
using Autofac;
using Dawlin.Util.Impl;
using GameSharp.DataAccess;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;
using Snap.Fakes;
using Snap.Services.Abstract;
using Snap.Services.Impl;
using Snap.Tests.Fakes;
using Xunit;
using Xunit.Abstractions;
using Xunit.Ioc.Autofac;
using Xunit.Sdk;

[assembly: TestFramework("Snap.Tests.Module.AutoFacTestConfiguration", "Snap.Tests")]

namespace Snap.Tests.Module
{
    public class AutoFacTestConfiguration : AutofacTestFramework
    {
        private const string TestSuffixConvention = "Tests";

        public AutoFacTestConfiguration(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith(TestSuffixConvention));

            builder.Register(context => new TestOutputHelper())
                .AsSelf()
                .As<ITestOutputHelper>()
                .InstancePerLifetimeScope();

            // configure your container
            builder
                .Register(t =>
                {
                    var context = new SnapDbContext(new DbContextOptionsBuilder()
                        .UseSqlite(new SqliteConnection("DataSource=:memory:"))
                        .Options);
                    return context;
                })
                .AsSelf()
                .As<GameSharpContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .OnActivated(async args =>
                {
                    var db = args.Instance;
                    await db.Database.OpenConnectionAsync();
                    await db.Database.EnsureCreatedAsync();
                }).OnRelease(context => { context.Database.CloseConnection(); });

            builder.RegisterType<FakePlayerService>()
                .As<IPlayerService>()
                .As<IFakePlayerService>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DawlinUtilModule>();
            builder.RegisterModule<GameSharpModule>();
            builder.RegisterModule<SnapGameModule>();

            builder.RegisterType<PlayerServiceSeedHelper>();
            builder.RegisterType<BackgroundHelper>();

            builder.RegisterType<FakeSecondPlayerFirst>().As<IPlayerChooser>();
            builder.RegisterType<FakeCardShuflee>().As<ICardShuffler>();
            builder.RegisterType<FakeCardDealter>().As<ICardDealter>();

            Container = builder.Build();
        }
    }
}