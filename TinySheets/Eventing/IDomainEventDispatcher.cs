namespace TinySheets.Eventing
{
    public interface IDomainEventDispatcher
    {
        bool DispatchEvents();
    }
}