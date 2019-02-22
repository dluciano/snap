using System.Collections.Generic;

namespace Dawlin.Util.Abstract
{
    public interface IListRandomizer
    {
        IEnumerable<T> Generate<T>(IEnumerable<T> list);
    }
}