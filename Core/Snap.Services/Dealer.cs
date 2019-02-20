using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dawlin.Util;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Exceptions;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;

namespace Snap.Services
{
    public class Dealer : IDealer
    {
        private readonly IListRandomizer _playerRandomizer;
        private readonly IListRandomizer _carRandomizer;

        public Dealer(IListRandomizer playerRandomizer,
            IListRandomizer carRandomizer)
        {
            _playerRandomizer = playerRandomizer;
            _carRandomizer = carRandomizer;
        }

        //Game loop
        public async Task PopCurrentPlayerCard(SnapGame game)
        {
            if (game.GameData.From != GameState.PLAYING)
                throw new InvalidGameStateException();

            var playerCard = game.CurrentTurn.StackEntity.PopCard();

            if (playerCard == null)
            {
                PlayerGameOver(game.CurrentTurn.PlayerTurn);
            }
            else
            {
                game.CurrentTurn.PlayerGameplay.Add(new PlayerGameplay
                {
                    Card = playerCard.Value,
                    PlayerTurn = game.CurrentTurn.PlayerTurn
                });
                game.CentralPile.Push(playerCard.Value);
                if (!CanSnap(game) && game.CurrentTurn.StackEntity.Last == null)
                    PlayerGameOver(game.CurrentTurn.PlayerTurn);
            }

            game.GameData.NextTurn();
        }

        private void PlayerGameOver(PlayerTurn currentTurn)
        {
            throw new NotImplementedException();
        }

        private bool CanSnap(SnapGame game)
        {
            //if (game.CentralPileLast == null || game.CentralPileLast.Previous == null) return false;
            //var last = (byte)((byte)game.CentralPileLast.Card << 4) >> 4;
            //var previous = (byte)((byte)game.CentralPileLast.Previous.Card << 4) >> 4;

            //return last == previous;
            return false;
        }

        public void Snap(GameRoom game, Player player)
        {
            throw new NotImplementedException();

            //if (!CanSnap(game)) return;
            //Console.WriteLine($"User: {player.Username} is going to SNAP");

            //game.Turns
            //    .Single(p => p.Player.Username == player.Username)
            //    .Snap(game.CentralPileLast);
            //game.CentralPileLast = null;
            //Console.WriteLine($"Snap DONE. Central Pile: {game}");
            //Console.WriteLine($"User Pile: {game.Turns.Single(t => t.Player.Username == player.Username)}");
        }

        public IEnumerable<Card> ShuffleCards() =>
            _carRandomizer.Generate(Enum.GetValues(typeof(Card)).Cast<Card>().ToArray());

        public virtual IEnumerable<PlayerTurn> ChooseTurns(GameData game)
        {
            PlayerTurn lastPlayerTurn = null;
            return _playerRandomizer
                .Generate(game.GameRoom.RoomPlayers)
                    .Where(p => !p.IsViewer).Select(r => r.Player).ToArray()
                .ToList()
                .Select(p =>
                {
                    var newTurn = new PlayerTurn
                    {
                        GameData = game,
                        Player = p
                    };
                    if (game.FirstPlayer == null) game.FirstPlayer = newTurn;
                    if (lastPlayerTurn != null) lastPlayerTurn.Next = newTurn;
                    return lastPlayerTurn = newTurn;
                });
        }

        public IEnumerable<StackNode> DealtCards(SnapGame game, IEnumerable<Card> cards)
        {
            var lastIdx = 0;
            var playersData = game.PlayersData.ToList();
            var r = cards.Select(c =>
            {
                var playerD = playersData[lastIdx];
                var newCardNode = new StackNode()
                {
                    Card = c,
                    Previous = playerD.StackEntity.Last
                };
                playerD.StackEntity.Last = newCardNode;
                lastIdx++;
                lastIdx = lastIdx >= playersData.Count ? 0 : lastIdx;

                return newCardNode;
            });
            return r;
            //var turns = game.PlayersData.ToList();

            //var cardsPerPlayer = cards.Count() / turns.Count;
            //var groups = cards
            //        .Select((card, index) => new { card, index })
            //        .GroupBy(g => g.index % turns.Count, c => c.card);

            //return groups.SelectMany(g =>
            //    {
            //        var aux = 0;
            //        return g.ToList().Select(card =>
            //        {
            //            var newCardNode = new StackNode()
            //            {
            //                Card = card,
            //                Previous = turns[aux].StackEntity.Last ?? null
            //            };
            //            turns[aux].StackEntity.Last = newCardNode;
            //            aux++;
            //            return newCardNode;
            //        });
            //    });
        }
    }
}