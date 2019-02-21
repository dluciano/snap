using System.Collections.Generic;
using System.Linq;
using Snap.Services.Abstract;

namespace Snap.Tests.Fakes
{
    internal sealed class FakeRandomizer<TObject> :
        IPlayerRandomizer,
        ICardRandomizer
    {
        private readonly IEnumerable<TObject> _objects;
        public FakeRandomizer(IEnumerable<TObject> objects)
        {
            _objects = objects;
        }
        public IEnumerable<T> Generate<T>(IEnumerable<T> list) =>
            _objects.Cast<T>();
    }
}
