using System;
using System.Collections.Generic;
using System.Text;
using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public partial class GameRoom : IEntity
    {
        public int Id { get; set; }

        public GameState State { get; private set; } = GameState.NONE;
        public ICollection<GameRoomPlayer> RoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public PlayerTurn FirstPlayer { get; set; }
        public PlayerTurn CurrentTurn { get; private set; }
        public CardPileNode CentralPileLast { get; set; }

        public IEnumerable<PlayerTurn> Turns
        {
            get
            {
                var turn = FirstPlayer;
                while (turn != null)
                {
                    yield return turn;
                    turn = turn.Next;
                }
            }
        }

        public void NextTurn() =>
            CurrentTurn = CurrentTurn == null || CurrentTurn.Next == null ? FirstPlayer : CurrentTurn.Next;

        public void Push(Card card) =>
            CentralPileLast = new CardPileNode
            {
                Previous = CentralPileLast,
                Card = card
            };

        public override string ToString()
        {
            if (CentralPileLast == null) return string.Empty;
            var s = new StringBuilder();
            var last = CentralPileLast;
            while (last != null)
            {
                s.Append(Enum.GetName(typeof(Card), last.Card) + ", ");
                last = last.Previous;
            }

            return s.ToString();
        }

        public GameRoom ChangeState(GameSessionTransitions transition) =>
             StaticStateMachine.ChangeState(this, transition);
    }
}