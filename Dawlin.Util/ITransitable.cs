using System;

namespace Dawlin.Util
{
    public interface ITransitable<TState>
    {
        TState CurrentState { get; set; }
    }
}