namespace TinySheets.Eventing
{
    public interface IGenerateDomainEvents
    {
        void SetEventStore(IDomainEventStore domainEventStore);
    }
}