﻿using System.Collections.Generic;
using Dawlin.Abstract.Entities;
using Dawlin.Util;
using Dawlin.Util.Abstract;
using GameSharp.Entities.Enums;

namespace GameSharp.Entities
{
    public abstract class GameData : IEntity, ITransitable<GameState>
    {
        public int Id { get; set; }

        public PlayerTurn FirstPlayer { get; set; }
        public PlayerTurn CurrentTurn { get; private set; }
        public GameRoom GameRoom { get; set; }
        public ICollection<PlayerTurn> PlayerTurns { get; } = new HashSet<PlayerTurn>();
        public GameState CurrentState { get; set; }

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

        public PlayerTurn NextTurn() =>
             CurrentTurn = CurrentTurn == null || CurrentTurn.Next == null ? FirstPlayer : CurrentTurn.Next;
    }
}
