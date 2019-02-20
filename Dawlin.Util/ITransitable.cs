using System;

namespace Dawlin.Util
{
    public interface ITransitable<TState>
    {
        TState From { get; set; }
    }
}