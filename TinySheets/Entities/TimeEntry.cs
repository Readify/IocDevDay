using System;
using TinySheets.Eventing;

namespace TinySheets.Entities
{
    public class TimeEntry : IGenerateDomainEvents
    {
        readonly DomainEventStore _domainEventStore = new DomainEventStore();
        readonly string _description;
        readonly double _hours;
        bool _isApproved;
        int _id = 0;

        [Obsolete("Persistence only")]
        protected TimeEntry() { }

        public TimeEntry(string description, double hours)
        {
            _description = description;
            _hours = hours;
        }

        public virtual int Id { get { return _id; } }
        public virtual double Hours { get { return _hours; } }
        public virtual string Description { get { return _description; } }
        public virtual bool IsApproved { get { return _isApproved; } }

        public virtual void Approve(string approver)
        {
            if (_isApproved)
                throw new InvalidOperationException("Time entry is already approved.");

            _domainEventStore.Add(new TimeEntryApprovedEvent(this, approver));
            _isApproved = true;
        }

        void IGenerateDomainEvents.SetEventStore(IDomainEventStore domainEventStore)
        {
            _domainEventStore.MergeInto(domainEventStore);
        }
    }
}