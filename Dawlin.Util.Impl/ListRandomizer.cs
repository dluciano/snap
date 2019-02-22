using System;
using System.Collections.Generic;
using System.Linq;

namespace Dawlin.Util.Impl
{
    public class ListRandomizer : IListRandomizer
    {
        public virtual IEnumerable<T> Generate<T>(IEnumerable<T> list)
        {
            var rnd = new Random();
            return list.ToList().OrderBy(x => rnd.Next());
        }
    }
}