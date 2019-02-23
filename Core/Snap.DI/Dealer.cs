using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Impl.Exceptions;
using Snap.Services.Impl.Notifications;

namespace Snap.Services.Impl
{
    internal sealed class Dealer : IDealer
    {
        private readonly ICardDealter _cardDealter;
        private readonly ICardShuffler _carShuffler;
        private readonly SnapDbContext _db;
        private readonly INotificationService _notificationService;
        private readonly IPlayerChooser _playerChooser;

        private readonly IPlayerProvider _playerProvider;

        public Dealer(IPlayerChooser playerChooser,
            ICardShuffler carShuffler,
            IPlayerProvider playerProvider,
            SnapDbContext db,
            ICardDealter cardDealter,
            INotificationService notificationService)
        {
            _playerChooser = playerChooser;
            _carShuffler = carShuffler;
            _playerProvider = playerProvider;
            _db = db;
            _cardDealter = cardDealter;
            _notificationService = notificationService;
        }

        public IEnumerable<Card> ShuffleCards() =>
            _carShuffler.ShuffleCards();

        public IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players) =>
            _playerChooser.ChooseTurns(players);

        public IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards) =>
            _cardDealter.DealtCards(playersStacks, cards);

        //Game loop
        public async Task<PlayerGameplay> PopCurrentPlayerCardAsync(SnapGame game, CancellationToken token)
        {
            if (game.GameData.CurrentState != GameState.PLAYING)
                throw new InvalidGameStateException();
            if (game.CurrentTurn.PlayerTurn.Player.Id != (await _playerProvider.GetCurrentPlayerAsync()).Id)
                throw new NotCurrentPlayerTryToPlayException();
            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var playerCard = game.CurrentTurn.StackEntity.PopCard();
                if (playerCard == null)
                    throw new PlayerHasNotMoreCardToPlayException();

                var gamePlay = new PlayerGameplay
                {
                    Card = playerCard.Value,
                    PlayerTurn = game.CurrentTurn
                };
                game.CurrentTurn.PlayerGameplay.Add(gamePlay);
                game.CentralPile.Push(playerCard.Value);

                if (!CanSnap(game) && game.CurrentTurn.StackEntity.Last == null)
                    PlayerGameOver(game.CurrentTurn.PlayerTurn);

                game.GameData.NextTurn();
                await _db.SaveChangesAsync(token);

                _notificationService.OnCardPop(this, new CardPopEvent(gamePlay, game.CurrentTurn));
                trans.Commit();
                return gamePlay;
            }
        }

        private void PlayerGameOver(PlayerTurn currentTurn)
        {
            //When a gamer lost then delete them from the turns
            throw new NotImplementedException();
        }

        private bool CanSnap(SnapGame game)
        {
            if (game.CentralPile == null ||
                game.CentralPile.Last == null
                || game.CentralPile.Last.Previous == null)
                return false;
            var last = (byte)((byte)game.CentralPile.Last.Card << 4) >> 4;
            var previous = (byte)((byte)game.CentralPile.Last.Previous.Card << 4) >> 4;

            return last == previous;
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
    }
}