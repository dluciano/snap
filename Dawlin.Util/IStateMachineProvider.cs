namespace Dawlin.Util.Abstract
{
    public interface IStateMachineProvider<TState, in TTransitions>
    {
        IStateMachineProvider<TState, TTransitions> AddTransition(TState from,
            TState to,
            TTransitions transition);

        ITransitable<TState> ChangeState(ITransitable<TState> transitable,
            TTransitions transition);
    }
}