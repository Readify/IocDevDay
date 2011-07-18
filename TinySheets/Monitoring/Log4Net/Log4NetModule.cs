using Autofac;
using log4net.Config;

namespace TinySheets.Monitoring.Log4Net
{
    public class Log4NetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            XmlConfigurator.Configure();

            builder.RegisterGeneric(typeof(Log4NetLog<>))
                .As(typeof(ILog<>))
                .SingleInstance();
        }
    }
}