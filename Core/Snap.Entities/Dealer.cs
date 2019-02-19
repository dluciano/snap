using System;
using System.Collections.Generic;
using System.Linq;

namespace Snap.Entities
{
    public class Dealer
    {
        private static readonly LinkedList<GameState> gameStates = new LinkedList<GameState>();

        public Dealer()
        {
            LinkedListNode<GameState> lastNode = null;
            Enum.GetValues(typeof(GameState)).Cast<GameState>().ToList().ForEach(s =>
            {
                lastNode = lastNode == null ? gameStates.AddFirst(s) : gameStates.AddAfter(lastNode, s);
            });
        }

        private T[] RandomArray<T>(T[] arr)
        {
            var rnd = new Random();
            return arr.ToList().OrderBy(x => rnd.Next()).ToArray();
        }

        //Game State
        public GameSession CreateGame(params Player[] players)
        {
            var game = new GameSession();
            foreach (var player in players)
                game.RoomPlayers.Add(new GameRoomPlayer
                {
                    Player = player,
                    IsViewer = false,
                    GameSession = game
                });
            NextState(game);
            return game;
        }

        public GameSession StartGame(GameSession game)
        {
            var shuffledCards = ShuffleCards(Enum.GetValues(typeof(Card)).Cast<Card>().ToArray());
            var turns = ChooseTurns(game, game.RoomPlayers.Select(r => r.Player).ToArray());
            DealtCards(game, shuffledCards);
            turns.Select((player, i) => new {Player = player, Turn = i})
                .ToList()
                .ForEach(t => Console.WriteLine($"{t.Turn + 1} - {t.Player}"));

            NextState(game);
            if (game.State != GameState.PLAYING)
                throw new Exception(
                    $"The game was supposed to be in state: ${nameof(GameState.PLAYING)} but was changed to: {Enum.GetName(typeof(GameState), game.State)}");
            game.NextTurn();
            Console.WriteLine($"Next player will be: {game.CurrentTurn.Player.Usename}");
            return game;
        }

        public GameSession TerminateGame(GameSession game)
        {
            game.State = GameState.TERMINATED;
            return game;
        }

        //Game loop
        public void NextMove(GameSession game)
        {
            if (game.State != GameState.PLAYING)
                throw new Exception(
                    $"The game was supposed to be in state: ${nameof(GameState.PLAYING)} but was changed to: {Enum.GetName(typeof(GameState), game.State)}");
            Console.WriteLine($"{game.CurrentTurn.Player.Usename} will pop a card");
            var playerCard = game.CurrentTurn.PopCard();
            Console.WriteLine($"Poped card: {playerCard}");
            Console.WriteLine($"Cards of {game.CurrentTurn.Player.Usename}: \n");
            Console.WriteLine($"{game.CurrentTurn}\n");
            if (playerCard == null)
            {
                PlayerGameOver(game.CurrentTurn);
            }
            else
            {
                game.CurrentTurn.PlayerGameplay.Add(new PlayerGameplay
                {
                    Card = playerCard.Value,
                    PlayerTurn = game.CurrentTurn
                });
                game.Push(playerCard.Value);
                Console.WriteLine($"Central Pile: {game}");
                Console.WriteLine($"CAN SNAP: {CanSnap(game)}");
                if (!CanSnap(game) && game.CurrentTurn.Last == null) PlayerGameOver(game.CurrentTurn);
            }

            game.NextTurn();
            Console.WriteLine($"Next player will be: {game.CurrentTurn.Player.Usename}");
        }

        private void PlayerGameOver(PlayerTurn currentTurn)
        {
            Console.WriteLine($"Player {currentTurn.Player.Usename} has lost");
        }

        private bool CanSnap(GameSession game)
        {
            if (game.CentralPileLast == null || game.CentralPileLast.Previous == null) return false;
            var last = (byte) ((byte) game.CentralPileLast.Card << 4) >> 4;
            var previous = (byte) ((byte) game.CentralPileLast.Previous.Card << 4) >> 4;

            return last == previous;
        }

        public void Snap(GameSession game, Player player)
        {
            if (!CanSnap(game)) return;
            Console.WriteLine($"User: {player.Usename} is going to SNAP");

            game.Turns
                .Single(p => p.Player.Usename == player.Usename)
                .Snap(game.CentralPileLast);
            game.CentralPileLast = null;
            Console.WriteLine($"Snap DONE. Central Pile: {game}");
            Console.WriteLine($"User Pile: {game.Turns.Single(t => t.Player.Usename == player.Usename)}");
            CheckForWinner();
        }

        private void CheckForWinner()
        {
            //If player abandon the session, the last player would be the winner
            //If some player has all the cards then is the winner
        }

        private static void NextState(GameSession game)
        {
            game.State = gameStates.Find(game.State).Next.Value;
        }

        //Initialization
        private Card[] ShuffleCards(Card[] cards)
        {
            return RandomArray(cards);
        }

        private PlayerTurn[] ChooseTurns(GameSession game, Player[] players)
        {
            PlayerTurn lastPlayerTurn = null;
            return RandomArray(players).ToList().Select(p =>
            {
                var newTurn = new PlayerTurn
                {
                    GameSession = game,
                    Player = p
                };
                if (game.FirstPlayer == null) game.FirstPlayer = newTurn;
                if (lastPlayerTurn != null) lastPlayerTurn.Next = newTurn;
                return lastPlayerTurn = newTurn;
            }).ToArray();
        }

        private void DealtCards(GameSession gameSession, Card[] cards)
        {
            var turns = gameSession.Turns.ToList();
            var cardsPerPlayer = cards.Length / turns.Count;
            cards
                .Select((card, index) => new {card, index})
                .GroupBy(g => g.index % cardsPerPlayer, c => c.card).ToList().ForEach(g =>
                {
                    var aux = 0;
                    g.ToList().ForEach(card =>
                    {
                        var newCardNode = new CardPileNode
                        {
                            Card = card,
                            Previous = turns[aux].Last ?? null
                        };
                        turns[aux].Last = newCardNode;
                        aux++;
                    });
                });
        }
    }
}