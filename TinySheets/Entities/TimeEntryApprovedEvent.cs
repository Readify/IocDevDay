using TinySheets.Eventing;

namespace TinySheets.Entities
{
    public class TimeEntryApprovedEvent : DomainEvent<TimeEntry>
    {
        readonly TimeEntry _timeEntry;
        readonly string _approver;

        public TimeEntryApprovedEvent(TimeEntry timeEntry, string approver)
            : base(timeEntry)
        {
            _timeEntry = timeEntry;
            _approver = approver;
        }

        public string Approver
        {
            get { return _approver; }
        }

        public TimeEntry TimeEntry
        {
            get { return _timeEntry; }
        }
    }
}