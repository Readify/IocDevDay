using System;
using System.Linq;
using TinySheets.Entities;
using TinySheets.Monitoring;
using TinySheets.Persistence;
using TinySheets.Tasks;

namespace TinySheets.PeriodicTasks
{
    [TaskFrequency(5000)]
    public class AutomaticApprovalTask : ITask
    {
        readonly IRepository<TimeEntry> _timeEntries;
        readonly ILog<InvoicePublishingTask> _log;

        public AutomaticApprovalTask(IRepository<TimeEntry> timeEntries, ILog<InvoicePublishingTask> log)
        {
            if (timeEntries == null) throw new ArgumentNullException("timeEntries");
            if (log == null) throw new ArgumentNullException("log");
            _timeEntries = timeEntries;
            _log = log;
        }

        public void Run()
        {
            foreach (var automaticallyApproved in _timeEntries.Items.Where(e => !e.IsApproved && e.Hours <= 8))
            {
                automaticallyApproved.Approve("Automatic Approver");
                _log.Information("Automatically approved time entry {0}.", automaticallyApproved.Id);
            }
        }
    }
}
