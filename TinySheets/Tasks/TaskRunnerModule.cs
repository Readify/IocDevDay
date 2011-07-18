using System.Collections.Generic;
using Autofac;
using Autofac.Features.Metadata;
using TinySheets.Monitoring;
using TinySheets.Persistence;

namespace TinySheets.Tasks
{
    public class TaskRunnerModule : Module
    {
        const string TransactedTaskServiceName = "transacted";

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterDecorator<ITask>(
                (c, t) => new TransactedTask(t, c.Resolve<ITransactionContext>()),
                fromKey: null,
                toKey: TransactedTaskServiceName);

            builder.Register(c => new TaskRunner(
                    c.ResolveNamed<IEnumerable<Meta<TaskFactory,ITaskMetadata>>>(TransactedTaskServiceName),
                    c.Resolve<ILog<TaskRunner>>()))
                .As<ITaskRunner>();
        }
    }
}
