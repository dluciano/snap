namespace Dawlin.Util.Abstract
{
    public interface ITransitable<TState>
    {
        TState CurrentState { get; set; }
    }
}