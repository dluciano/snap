using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Dawlin.Util
{
    public class ListRandomizer : IListRandomizer
    {
        public IEnumerable<T> Generate<T>(IEnumerable<T> list)
        {
            var rnd = new Random();
            return list.ToList().OrderBy(x => rnd.Next());
        }
    }
}