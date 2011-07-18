using System;
using NHibernate;
using NHibernate.Type;
using TinySheets.Eventing;

namespace TinySheets.Persistence.NHibernate
{
    public class NHibernateDomainEventListener : EmptyInterceptor
    {
        readonly IDomainEventStore _domainEventStore;

        public NHibernateDomainEventListener(IDomainEventStore domainEventStore)
        {
            if (domainEventStore == null) throw new ArgumentNullException("domainEventStore");
            _domainEventStore = domainEventStore;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            SubscribeToDomainEvents(entity);
            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            SubscribeToDomainEvents(entity);
            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        void SubscribeToDomainEvents(object entity)
        {
            var rde = entity as IGenerateDomainEvents;
            if (rde != null)
                rde.SetEventStore(_domainEventStore);
        }
    }
}
