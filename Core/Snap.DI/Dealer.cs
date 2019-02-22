using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Exceptions;
using Snap.Services.Notifications;

namespace Snap.Services.Impl
{
    internal sealed class Dealer : IDealer
    {
        private readonly IPlayerChooser _playerChooser;
        private readonly ICardRandomizer _carRandomizer;
        private readonly ICardDealter _cardDealter;

        private readonly IPlayerService _playerService;
        private readonly INotificationService _notificationService;
        private readonly SnapDbContext _db;

        public Dealer(IPlayerChooser playerChooser,
            ICardRandomizer carRandomizer,
            IPlayerService playerService,
            SnapDbContext db,
            ICardDealter cardDealter,
            INotificationService notificationService)
        {
            _playerChooser = playerChooser;
            _carRandomizer = carRandomizer;
            _playerService = playerService;
            _db = db;
            _cardDealter = cardDealter;
            _notificationService = notificationService;
        }

        //Game loop
        public async Task<PlayerGameplay> PopCurrentPlayerCardAsync(SnapGame game, CancellationToken token)
        {
            if (game.GameData.CurrentState != GameState.PLAYING)
                throw new InvalidGameStateException();
            if (game.CurrentTurn.PlayerTurn.Player.Username != (await _playerService.GetCurrentPlayerAsync()).Username)
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

                if ((!CanSnap(game) && game.CurrentTurn.StackEntity.Last == null))
                    PlayerGameOver(game.CurrentTurn.PlayerTurn);

                await _db.SaveChangesAsync(token);
                game.GameData.NextTurn();
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
            _carRandomizer.ShuffleCards();

        public IEnumerable<PlayerTurn> ChooseTurns(GameData game) =>
            _playerChooser.ChooseTurns(game);

        public IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards) =>
            _cardDealter.DealtCards(playersStacks, cards);
    }
}