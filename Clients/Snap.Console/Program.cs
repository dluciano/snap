﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Dawlin.Util.Impl;
using GameSharp.DataAccess;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;
using Snap.Entities.Enums;
using Snap.Fakes;
using Snap.Services.Abstract;
using Snap.Services.Impl;
using Snap.Services.Impl.Notifications;

namespace Snap.ConsoleApplication
{
    public class Program
    {
        private readonly ISnapGameServices _snapGameServices;
        private readonly IDealer _dealer;
        private readonly INotificationService _notifier;
        private readonly IFakePlayerProvider _playerProvider;
        private readonly IGameRoomPlayerServices _gameRoomService;
        private readonly IGameRoomServices _roomService;

        public Program(ISnapGameServices snapGameServices,
            IDealer dealer,
            INotificationService notifier,
            IFakePlayerProvider playerProvider,
            IGameRoomPlayerServices gameRoomService,
            IGameRoomServices roomService)
        {
            _snapGameServices = snapGameServices;
            _dealer = dealer;
            _notifier = notifier;
            _playerProvider = playerProvider;
            _gameRoomService = gameRoomService;
            _roomService = roomService;
        }

        public static async Task Main(string[] args)
        {
            await ConfigureContainer().Resolve<Program>().Start();
        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Program>();
            builder
                .Register(t =>
                {
                    var context = new SnapDbContext(new DbContextOptionsBuilder<SnapDbContext>()
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

            builder.RegisterType<FakePlayerProvider>()
                .As<IPlayerProvider>()
                .As<IFakePlayerProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<DawlinUtilModule>();
            builder.RegisterModule<GameSharpModule>();
            builder.RegisterModule<SnapGameModule>();
            return builder.Build();
        }

        internal async Task Start()
        {
            await _playerProvider.SetCurrentPlayer(players => players.SingleAsync(p => p.Username == "User 1"));

            await _playerProvider.AddAsync(CancellationToken.None);
            await _playerProvider.AddAsync(CancellationToken.None);
            await _playerProvider.SetCurrentPlayer(players => players.SingleAsync(p => p.Username == "User 1"));

            var room = await _roomService.CreateAsync(CancellationToken.None);

            await _playerProvider.SetCurrentPlayer(players => players.SingleAsync(p => p.Username == "User 2"));
            await _gameRoomService.AddPlayersAsync(room, false, CancellationToken.None);

            var game = await _snapGameServices.StarGameAsync(room, CancellationToken.None);

            Console.WriteLine("Press any key to Pop, 's' to Snap! " +
                                     "or 'q' to finish: ");
            Console.WriteLine($"Current Player is: {game.CurrentTurn.PlayerTurn.Player.Username }");
            Console.WriteLine($"Player cards: {game.CurrentTurn.StackEntity}");

            await _playerProvider.SetCurrentPlayer(game);

            _notifier.CardPopEvent += async (sender, e) =>
             {
                 Division();

                 Console.WriteLine($"Card Poped: {Enum.GetName(typeof(Card), e.GamePlay.Card) }");
                 Console.WriteLine($"Player cards: {e.GamePlay.PlayerTurn.StackEntity}");
                 Console.WriteLine($"Central Pile: {e.GamePlay.PlayerTurn.SnapGame.CentralPile}");
                 Division();
                 Console.WriteLine($"Current Player is: {e.NextPlayer.PlayerTurn.Player.Username}");
                 Console.WriteLine($"Player cards: {game.CurrentTurn.StackEntity}");

                 await _playerProvider.SetCurrentPlayer(game);
             };

            var finish = false;
            while (!finish)
            {
                var option = Console.ReadKey().KeyChar;
                switch (option)
                {
                    case 'q':
                    case 'Q':
                        finish = true;
                        break;
                    case 's':
                    case 'S':
                        //var rI = new System.Random().Next(0, game.RoomPlayers.Count - 1);
                        //var p = game.RoomPlayers.ToList()[rI].Player;
                        //_dealer.Snap(game, p);
                        break;
                    default:
                        await _dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                        break;
                }
            }
            Console.ReadKey();
        }
        private static void Division()
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine();
        }
    }
}