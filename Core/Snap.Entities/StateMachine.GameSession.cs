using System.Linq;
using Snap.Entities.Enums;

namespace Snap.Entities
{
    public partial class GameRoom
    {
        private static class StaticStateMachine
        {
            private static readonly GameStateTransition[] TRANSITIONS = new GameStateTransition[]
            {
                new GameStateTransition(GameState.NONE, GameState.AWAITING_PLAYERS, GameSessionTransitions.CREATE_GAME),
                new GameStateTransition(GameState.AWAITING_PLAYERS, GameState.PLAYING,GameSessionTransitions.START_GAME),
                new GameStateTransition(GameState.PLAYING, GameState.FINISHED,GameSessionTransitions.FINISH_GAME),
                new GameStateTransition(GameState.PLAYING, GameState.ABORTED,GameSessionTransitions.ABORT_GAME),
            };

            public static GameRoom ChangeState(GameRoom room, GameSessionTransitions transition)
            {
                room.State = TRANSITIONS.Single(t => t.From == room.State && t.Transition == transition).To;
                return room;
            }

            private sealed class GameStateTransition
            {
                public GameState From { get; }
                public GameState To { get; }
                public GameSessionTransitions Transition { get; }

                public GameStateTransition(GameState from, GameState to, GameSessionTransitions transition)
                {
                    From = from;
                    To = to;
                    Transition = transition;
                }
            }
        }
    }
}