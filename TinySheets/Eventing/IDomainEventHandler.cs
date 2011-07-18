namespace TinySheets.Eventing
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : DomainEvent
    {
        void Handle(TEvent domainEvent);
    }
}