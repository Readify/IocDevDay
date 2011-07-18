using System.Collections.Generic;

namespace TinySheets.Eventing
{
    public interface IDomainEventStore
    {
        void Add(DomainEvent domainEvent);
        IList<DomainEvent> RetrieveAndClear();
    }
}