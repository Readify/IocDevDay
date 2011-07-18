using System;
using System.Collections.Generic;
using Autofac;

namespace TinySheets.Eventing
{
    public class AutofacDomainEventDispatcher : IDomainEventDispatcher
    {
        readonly IDomainEventStore _domainEventStore;
        readonly IComponentContext _componentContext;

        public AutofacDomainEventDispatcher(IDomainEventStore domainEventStore, IComponentContext componentContext)
        {
            if (domainEventStore == null) throw new ArgumentNullException("domainEventStore");
            if (componentContext == null) throw new ArgumentNullException("componentContext");
            _domainEventStore = domainEventStore;
            _componentContext = componentContext;
        }

        public bool DispatchEvents()
        {
            var eventsToDispatch = _domainEventStore.RetrieveAndClear();
            if (eventsToDispatch.Count == 0)
                return false;

            foreach (var domainEvent in eventsToDispatch)
            {
                var handlerType = typeof (IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                var allHandlersType = typeof (IEnumerable<>).MakeGenericType(handlerType);
                var handlers = (IEnumerable<object>)_componentContext.Resolve(allHandlersType);
                foreach (dynamic handler in handlers)
                {
                    handler.Handle((dynamic)domainEvent);
                }
            }

            return true;
        }
    }
}