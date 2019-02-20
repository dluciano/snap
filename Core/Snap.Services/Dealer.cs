using System;
using System.Collections.Generic;
using System.Linq;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class Dealer : IDealer
    {
        private T[] RandomArray<T>(T[] arr)
        {
            var rnd = new Random();
            return arr.ToList().OrderBy(x => rnd.Next()).ToArray();
        }

        public GameRoom TerminateGame(GameRoom game)
        {
            return game;
        }

        //Game loop
        public void NextMove(GameRoom game)
        {
            if (game.From != GameState.PLAYING)
                throw new Exception(
                    $"The game was supposed to be in state: ${nameof(GameState.PLAYING)} but was changed to: {Enum.GetName(typeof(GameState), game.From)}");
            Console.WriteLine($"{game.CurrentTurn.Player.Username} will pop a card");
            var playerCard = game.CurrentTurn.PopCard();
            Console.WriteLine($"Poped card: {playerCard}");
            Console.WriteLine($"Cards of {game.CurrentTurn.Player.Username}: \n");
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
            Console.WriteLine($"Next player will be: {game.CurrentTurn.Player.Username}");
        }

        private void PlayerGameOver(PlayerTurn currentTurn)
        {
            Console.WriteLine($"Player {currentTurn.Player.Username} has lost");
        }

        private bool CanSnap(GameRoom game)
        {
            if (game.CentralPileLast == null || game.CentralPileLast.Previous == null) return false;
            var last = (byte)((byte)game.CentralPileLast.Card << 4) >> 4;
            var previous = (byte)((byte)game.CentralPileLast.Previous.Card << 4) >> 4;

            return last == previous;
        }

        public void Snap(GameRoom game, Player player)
        {
            if (!CanSnap(game)) return;
            Console.WriteLine($"User: {player.Username} is going to SNAP");

            game.Turns
                .Single(p => p.Player.Username == player.Username)
                .Snap(game.CentralPileLast);
            game.CentralPileLast = null;
            Console.WriteLine($"Snap DONE. Central Pile: {game}");
            Console.WriteLine($"User Pile: {game.Turns.Single(t => t.Player.Username == player.Username)}");
        }

        public IEnumerable<Card> ShuffleCards() =>
            RandomArray(Enum.GetValues(typeof(Card)).Cast<Card>().ToArray());

        public IEnumerable<PlayerTurn> ChooseTurns(GameRoom game)
        {
            PlayerTurn lastPlayerTurn = null;
            return RandomArray(game.RoomPlayers.Select(r => r.Player).ToArray())
                .ToList()
                .Select(p =>
            {
                var newTurn = new PlayerTurn
                {
                    GameRoom = game,
                    Player = p
                };
                if (game.FirstPlayer == null) game.FirstPlayer = newTurn;
                if (lastPlayerTurn != null) lastPlayerTurn.Next = newTurn;
                return lastPlayerTurn = newTurn;
            });
        }

        public IEnumerable<CardPileNode> DealtCards(GameRoom gameRoom, IEnumerable<Card> cards)
        {
            var turns = gameRoom.Turns.ToList();
            var cardsPerPlayer = cards.Count() / turns.Count;
            return cards
                .Select((card, index) => new { card, index })
                .GroupBy(g => g.index % cardsPerPlayer, c => c.card)
                .SelectMany(g =>
                {
                    var aux = 0;
                    return g.ToList().Select(card =>
                    {
                        var newCardNode = new CardPileNode
                        {
                            Card = card,
                            Previous = turns[aux].Last ?? null
                        };
                        turns[aux].Last = newCardNode;
                        aux++;
                        return newCardNode;
                    });
                });
        }
    }
}