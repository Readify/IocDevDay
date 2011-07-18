using Autofac.Features.OwnedInstances;

namespace TinySheets.Tasks
{
    public delegate Owned<ITask> TaskFactory();
}
