using System.Linq;
using Autofac;
using TinySheets.Tasks;

namespace TinySheets.PeriodicTasks
{
    public class PeriodicTasksModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var thisAssembly = typeof (PeriodicTasksModule).Assembly;

            builder.RegisterAssemblyTypes(thisAssembly)
                .Except<TransactedTask>()
                .As<ITask>()
                .WithMetadataFrom<ITaskMetadata>();
        }
    }
}
