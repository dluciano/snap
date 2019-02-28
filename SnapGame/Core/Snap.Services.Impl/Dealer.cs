using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dawlin.Abstract.Entities.Exceptions;
using Dawlin.Util.Abstract;
using GameSharp.Entities;
using GameSharp.Entities.Enums;
using GameSharp.Services.Abstract;
using GameSharp.Services.Impl.Exceptions;
using Microsoft.EntityFrameworkCore;
using Snap.DataAccess;
using Snap.Entities;
using Snap.Entities.Enums;
using Snap.Services.Abstract;
using Snap.Services.Abstract.Notifications;
using Snap.Services.Impl.Exceptions;

namespace Snap.Services.Impl
{
    internal sealed class Dealer : IDealer
    {
        private readonly ICardDealter _cardDealter;
        private readonly ICardShuffler _carShuffler;
        private readonly SnapDbContext _db;
        private readonly IPlayerChooser _playerChooser;
        public event AsyncEventHandler<CardPopEvent> OnCardPopEvent;
        public event AsyncEventHandler<CardSnapEvent> OnSnap;
        private readonly IPlayerProvider _playerProvider;

        public Dealer(IPlayerChooser playerChooser,
            ICardShuffler carShuffler,
            IPlayerProvider playerProvider,
            SnapDbContext db,
            ICardDealter cardDealter)
        {
            _playerChooser = playerChooser;
            _carShuffler = carShuffler;
            _playerProvider = playerProvider;
            _db = db;
            _cardDealter = cardDealter;
        }

        public IEnumerable<Card> ShuffleCards() =>
            _carShuffler.ShuffleCards();

        public IEnumerable<PlayerTurn> ChooseTurns(IEnumerable<Player> players) =>
            _playerChooser.ChooseTurns(players);

        public IEnumerable<StackNode> DealtCards(IList<StackEntity> playersStacks, IEnumerable<Card> cards) =>
            _cardDealter.DealtCards(playersStacks, cards);

        //Game loop
        public async Task<PlayerGameplay> PopCurrentPlayerCardAsync(int gameId, CancellationToken token = default(CancellationToken))
        {
            var player = (await _playerProvider.GetCurrentPlayerAsync());
            if (player == null)
            {
                throw new UnauthorizedAccessException();
            }

            var game = await _db
                .SnapGames
                .Include(g => g.CentralPile.Last)
                .ThenInclude(p => p.Previous)

                .Include(g => g.GameData)
                .ThenInclude(gd => gd.CurrentTurn)
                .ThenInclude(pt => pt.Next)
                .Include(p => p.GameData.CurrentTurn.Player)

                .Include(g => g.GameData)
                .ThenInclude(gd => gd.Room)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.PlayerTurn)
                .ThenInclude(pt => pt.Player)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.PlayerTurn)
                .ThenInclude(pt => pt.Next)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.StackEntity.Last)
                .ThenInclude(n => n.Previous)

                .SingleOrDefaultAsync(p => p.Id == gameId, token);
            if (game == null)
            {
                throw new EntityNotFoundException("The game does not exists");
            }

            if (game.GameData.CurrentState != GameState.PLAYING)
                throw new InvalidGameStateException();
            if (game.CurrentTurn.PlayerTurn.Player.Id != player.Id)
                throw new NotCurrentPlayerTryToPlayException();

            using (var trans = await _db.Database.BeginTransactionAsync(token))
            {
                var playerCard = game.CurrentTurn.StackEntity.PopCard();
                if (playerCard == null)
                    throw new PlayerHasNotMoreCardToPlayException();

                var gamePlay = new PlayerGameplay
                {
                    Card = playerCard.Value,
                    PlayerTurn = game.CurrentTurn,
                    GameData = game.GameData
                };
                game.CurrentTurn.PlayerGameplay.Add(gamePlay);
                game.CentralPile.Push(playerCard.Value);

                if (!CanSnap(game) && game.CurrentTurn.StackEntity.Last == null)
                    PlayerGameOver(game.CurrentTurn.PlayerTurn);

                game.GameData.NextTurn();
                await _db.SaveChangesAsync(token);

                await OnCardPopEvent?.Invoke(this, new CardPopEvent(gamePlay, game.CurrentTurn), token);
                token.ThrowIfCancellationRequested();

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
            var last = game.CentralPile.Last.Card.GetCardValue();
            var previous = game.CentralPile.Last.Previous.Card.GetCardValue();

            return last == previous;
        }

        public async Task<bool> Snap(int gameId, CancellationToken token = default(CancellationToken))
        {
            var player = (await _playerProvider.GetCurrentPlayerAsync());
            if (player == null)
            {
                throw new UnauthorizedAccessException();
            }
            
            var game = await _db
                .SnapGames
                .Include(g => g.CentralPile.Last)
                .ThenInclude(p => p.Previous)

                .Include(g => g.GameData)
                .ThenInclude(g => g.Room)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.PlayerTurn)
                .ThenInclude(pt => pt.Player)

                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.StackEntity)
                .ThenInclude(p => p.Last)
                .ThenInclude(n => n.Previous)

                
                .Include(p => p.PlayersData)
                .ThenInclude(pd => pd.SnapGame)
                .ThenInclude(p => p.GameData)
                .ThenInclude(n => n.Room)

                .SingleOrDefaultAsync(p => p.Id == gameId, token);

            if (!CanSnap(game))
                return false;

            var playerData = game.PlayersData.Single(p => p.PlayerTurn.Player.Id == player.Id);
            playerData.StackEntity.Snap(game.CentralPile);
            await OnSnap?.Invoke(this, new CardSnapEvent
            {
                PlayerData = playerData
            }, token);
            await _db.SaveChangesAsync(token);
            return true;
        }
    }
}