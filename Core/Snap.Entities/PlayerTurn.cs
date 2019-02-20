using System;
using System.Collections.Generic;
using System.Text;
using Dawlin.Abstract.Entities;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public class PlayerTurn : IEntity
    {
        public int Id { get; set; }

        public Player Player { get; set; }
        public PlayerTurn Next { get; set; }
        public GameRoom GameRoom { get; set; }
        public CardPileNode Last { get; set; }
        public ICollection<PlayerGameplay> PlayerGameplay { get; } = new HashSet<PlayerGameplay>();
        public ICollection<GameRoom> FirstPlayers { get; } = new HashSet<GameRoom>();
        public ICollection<GameRoom> CurrentTurns { get; } = new HashSet<GameRoom>();

        public Card? PopCard()
        {
            if (Last == null)
                return null;
            var aux = Last;
            Last = Last.Previous;
            return aux.Card;
        }

        public void Snap(CardPileNode centralPileLast)
        {
            if (Last == null)
                return;
            var first = Last;
            while (first.Previous != null) first = first.Previous;
            first.Previous = centralPileLast;
        }

        public override string ToString()
        {
            return $"{Player.Username} {CardString()}";
        }

        private string CardString()
        {
            if (Last == null) return string.Empty;
            var s = new StringBuilder();
            var last = Last;
            while (last != null)
            {
                s.Append(Enum.GetName(typeof(Card), last.Card) + ", ");
                last = last.Previous;
            }

            return s.ToString();
        }
    }
}