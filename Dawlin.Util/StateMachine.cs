using System.Collections.Generic;

namespace Dawlin.Util
{
    public sealed class StateMachine<TState, TTransitions> :
        IStateMachineProvider<TState, TTransitions>
    {
        private readonly HashSet<Node> _states = new HashSet<Node>();
        private readonly IDictionary<TTransitions, Node> _transitions = new Dictionary<TTransitions, Node>();

        public IStateMachineProvider<TState, TTransitions> AddTransition(TState from,
            TState to,
            TTransitions transition)
        {
            var node = new Node(from, to);
            _states.Add(node);
            _transitions.Add(transition, node);
            return this;
        }

        public ITransitable<TState> ChangeState(ITransitable<TState> transitable,
            TTransitions transition)
        {
            if (!_transitions[transition].From.Equals(transitable.CurrentState))
                throw new InvalidChangeTransition();
            transitable.CurrentState = _transitions[transition].To;
            return transitable;
        }

        private sealed class Node : EqualityComparer<Node>
        {
            public TState From { get; }
            public TState To { get; }

            public Node(TState from, TState to)
            {
                From = from;
                To = to;
            }

            public override bool Equals(Node x, Node y) =>
                (x.From == null && y.To == null ||
                 (x.From != null && y.From != null && x.From.Equals(y.To)));

            public override int GetHashCode(Node obj) =>
                obj.From.GetHashCode() ^ obj.To.GetHashCode();
        }
    }

}