using Autofac;
using TinySheets.Eventing;

namespace TinySheets.EventHandlers
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof (EventHandlersModule).Assembly)
                .AsClosedTypesOf(typeof (IDomainEventHandler<>));
        }
    }
}