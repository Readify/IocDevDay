using Autofac;
using NHibernate;
using NHibernate.Cfg;

namespace TinySheets.Persistence.NHibernate
{
    public class NHibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => ConfigureSessionFactory())
                .As<ISessionFactory>()
                .SingleInstance();

            builder.RegisterType<NHibernateDomainEventListener>()
                .As<IInterceptor>();

            builder.RegisterGeneric(typeof(NHibernateRepository<>))
                .As(typeof(IRepository<>));

            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession(c.Resolve<IInterceptor>()))
                .As<ISession>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NHibernateTransactionContext>()
                .As<ITransactionContext>()
                .InstancePerLifetimeScope();
        }

        static ISessionFactory ConfigureSessionFactory()
        {
            var cfg = new Configuration();

            cfg.SetProperty("dialect", "NHibernate.Dialect.MsSql2008Dialect");
            cfg.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider, NHibernate");
            cfg.SetProperty("connection.connection_string_name", "TinySheets");
            cfg.SetProperty("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");

            cfg.AddAssembly(typeof(NHibernateModule).Assembly);

            return cfg.BuildSessionFactory();
        }
    }
}
