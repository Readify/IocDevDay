using System;
using TinySheets.Entities;
using TinySheets.Eventing;
using TinySheets.Monitoring;

namespace TinySheets.EventHandlers
{
    public class ApprovalEmailSender :
        IDomainEventHandler<TimeEntryApprovedEvent>
    {
        readonly ILog<ApprovalEmailSender> _log;

        public ApprovalEmailSender(ILog<ApprovalEmailSender> log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }

        public void Handle(TimeEntryApprovedEvent domainEvent)
        {
            _log.Information("Your time entry for {0} was approved by {1}.", domainEvent.TimeEntry.Description, domainEvent.Approver);
        }
    }
}
