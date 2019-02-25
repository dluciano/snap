using System.Threading;
using System.Threading.Tasks;

namespace Dawlin.Util.Abstract
{
    public delegate Task AsyncEventHandler<in T>(object sender, T args, CancellationToken token = default(CancellationToken));
}