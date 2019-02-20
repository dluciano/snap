using System.Collections.Generic;

namespace Dawlin.Util
{
    public interface IListRandomizer
    {
        IEnumerable<T> Generate<T>(IEnumerable<T> list);
    }
}