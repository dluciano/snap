using System.Linq;
using Snap.Entities;

namespace Snap.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dealer = new Dealer();
            var game = dealer.CreateGame(
                new Player { Usename = "Foo 1" },
                new Player { Usename = "Foo 2" },
                new Player { Usename = "Foo 3" },
                new Player { Usename = "Foo 4" });
            dealer.StartGame(game);
            var finish = false;
            while (!finish)
            {
                System.Console.WriteLine("Press any key to Pop or q to finish");
                var option = System.Console.ReadKey().KeyChar;
                switch (option)
                {
                    case 'q':
                    case 'Q':
                        finish = true;
                        break;
                    case 's':
                    case 'S':
                        var rI = new System.Random().Next(0, game.RoomPlayers.Count - 1);
                        var p = game.RoomPlayers.ToList()[rI].Player;
                        dealer.Snap(game, p);
                        break;
                    default:
                        dealer.NextMove(game);
                        break;
                }
            }

            System.Console.ReadKey();
        }
    }
}