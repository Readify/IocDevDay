using System;
using System.Collections.Generic;

namespace TinySheets.Eventing
{
    public class DomainEventStore : IDomainEventStore
    {
        IList<DomainEvent> _domainEvents = new List<DomainEvent>();
        IDomainEventStore _mergedStore;

        public IList<DomainEvent> RetrieveAndClear()
        {
            if (_mergedStore != null)
                throw new InvalidOperationException("This store has been merged into another store.");

            var result = _domainEvents;
            _domainEvents = new List<DomainEvent>();
            return result;
        }

        public void Add(DomainEvent domainEvent)
        {
            if (_mergedStore != null)
                _mergedStore.Add(domainEvent);
            else
                _domainEvents.Add(domainEvent);
        }

        public void MergeInto(IDomainEventStore domainEventStore)
        {
            if (_mergedStore != null)
                throw new InvalidOperationException("This store has already been merged with another store.");

            var domainEventsToMerge = RetrieveAndClear();
            _mergedStore = domainEventStore;

            foreach (var domainEvent in domainEventsToMerge)
                _mergedStore.Add(domainEvent);
        }
    }
}