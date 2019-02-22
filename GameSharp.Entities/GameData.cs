using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using Dawlin.Util.Abstract;
using GameSharp.Entities.Enums;

namespace GameSharp.Entities
{
    public abstract class GameData : IEntity, ITransitable<GameState>
    {
        public PlayerTurn FirstPlayer { get; set; }
        public PlayerTurn CurrentTurn { get; private set; }
        public GameRoom GameRoom { get; set; }
        public ICollection<PlayerTurn> PlayerTurns { get; } = new HashSet<PlayerTurn>();

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

        public int Id { get; set; }
        public GameState CurrentState { get; set; }

        public PlayerTurn NextTurn()
        {
            return CurrentTurn = CurrentTurn == null || CurrentTurn.Next == null ? FirstPlayer : CurrentTurn.Next;
        }
    }
}