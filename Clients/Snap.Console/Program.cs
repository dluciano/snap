using System;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Services;
using Microsoft.Extensions.DependencyInjection;
using Snap.DI;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Notifications;

namespace Snap.ConsoleApplication
{
    public class Program
    {
        public class FakePlayerService : IPlayerService
        {
            public Player Player { get; set; }
            public Player CurrentPlayer => Player;
        }
        public static async Task Main(string[] args)
        {
            var firstPlayer = new Player { Username = "Player 2" };
            var other = new Player { Username = "Player 1" };
            var players = new[] { firstPlayer, other };
            using (var module = await new ModuleManager()
                .WithDefaults()
                .Configure(provider => provider.AddScoped<IPlayerService, FakePlayerService>())
                .BuildAndCreateDatabase())
            {
                var service = module.GetService<ISnapGameServices>();
                var dealer = module.GetService<IDealer>();
                var notifier = module.GetService<INotificationService>();
                var game = await service.CreateAsync(CancellationToken.None, players);
                game = await service.StarGame(game, CancellationToken.None);

                Console.WriteLine("Press any key to Pop, 's' to Snap! " +
                                         "or 'q' to finish: ");
                Console.WriteLine($"Current Player is: {game.CurrentTurn.PlayerTurn.Player.Username }");
                Console.WriteLine($"Player cards: {game.CurrentTurn.StackEntity}");

                ((FakePlayerService)module.GetService<IPlayerService>()).Player = game.CurrentTurn.PlayerTurn.Player;

                notifier.CardPopEvent += (sender, e) =>
                {
                    Division();

                    Console.WriteLine($"Card Poped: {Enum.GetName(typeof(Card), e.GamePlay.Card) }");
                    Console.WriteLine($"Player cards: {e.GamePlay.PlayerTurn.StackEntity}");
                    Console.WriteLine($"Central Pile: {e.GamePlay.PlayerTurn.SnapGame.CentralPile}");
                    Division();
                    Console.WriteLine($"Current Player is: {e.NextPlayer.PlayerTurn.Player.Username}");
                    Console.WriteLine($"Player cards: {game.CurrentTurn.StackEntity}");

                    ((FakePlayerService)module.GetService<IPlayerService>()).Player = game.CurrentTurn.PlayerTurn.Player;
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
                            //dealer.Snap(game, p);
                            break;
                        default:
                            var gameplay = await dealer.PopCurrentPlayerCardAsync(game, CancellationToken.None);
                            break;
                    }
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