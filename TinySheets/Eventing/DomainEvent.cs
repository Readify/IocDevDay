using System;

namespace TinySheets.Eventing
{
    public abstract class DomainEvent { }

    public abstract class DomainEvent<TSender> : DomainEvent
        where TSender : class
    {
        readonly TSender _sender;

        protected DomainEvent(TSender sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            _sender = sender;
        }

        public TSender Sender
        {
            get { return _sender; }
        }
    }
}