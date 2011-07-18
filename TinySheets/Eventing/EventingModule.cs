using Autofac;

namespace TinySheets.Eventing
{
    public class EventingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DomainEventStore>()
                .As<IDomainEventStore>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AutofacDomainEventDispatcher>()
                .As<IDomainEventDispatcher>();
        }
    }
}