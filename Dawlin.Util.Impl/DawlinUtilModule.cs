using Autofac;

namespace Dawlin.Util.Impl
{
    public sealed class DawlinUtilModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ListRandomizer>().As<IListRandomizer>();
        }
    }
}
