using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace TinySheets.Web.Mvc
{
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(MvcModule).Assembly)
                .InjectActionInvoker();

            builder.RegisterType<ExtensibleActionInvoker>()
                .As<IActionInvoker>();

            builder.RegisterType<TransactedActionFilter>()
                .As<IActionFilter>();
        }
    }
}