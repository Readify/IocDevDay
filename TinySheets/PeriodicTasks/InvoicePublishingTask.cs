using System;
using System.Linq;
using TinySheets.Entities;
using TinySheets.Monitoring;
using TinySheets.Persistence;
using TinySheets.Tasks;

namespace TinySheets.PeriodicTasks
{
    [TaskFrequency(30000)]
    public class InvoicePublishingTask : ITask
    {
        readonly IRepository<TimeEntry> _timeEntries;
        readonly ILog<InvoicePublishingTask> _log;

        public InvoicePublishingTask(IRepository<TimeEntry> timeEntries, ILog<InvoicePublishingTask> log)
        {
            if (timeEntries == null) throw new ArgumentNullException("timeEntries");
            if (log == null) throw new ArgumentNullException("log");
            _timeEntries = timeEntries;
            _log = log;
        }

        public void Run()
        {
            var totalHours = _timeEntries.Items.Where(e => e.IsApproved).ToArray().Sum(e => e.Hours);
            var charge = totalHours * 1234;
           _log.Information("[InvoicePublishingTask] Invoice value: $" + charge);
        }
    }
}
