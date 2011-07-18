using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac.Features.Metadata;
using TinySheets.Monitoring;

namespace TinySheets.Tasks
{
    public class TaskRunner : ITaskRunner
    {
        readonly IList<TaskEntry> _tasks; 
        readonly ILog<TaskRunner> _log;
        readonly Timer _timer;
        static readonly TimeSpan _frequency = TimeSpan.FromSeconds(1);

        public TaskRunner(IEnumerable<Meta<TaskFactory, ITaskMetadata>> tasks, ILog<TaskRunner> log)
        {
            if (tasks == null) throw new ArgumentNullException("tasks");
            
            _log = log;
            
            _tasks = tasks
                .Select(t => new TaskEntry(
                    t.Metadata.Frequency,
                    () => { using (var o = t.Value()) o.Value.Run(); },
                    e => _log.Error(e, "Error while running task.")))
                .ToList();

            _timer = new Timer(CheckTasks);
        }

        public void Start()
        {
            _log.Information("Task runner starting.");
            _timer.Change(TimeSpan.Zero, _frequency);
        }

        public void Stop()
        {
            _log.Information("Task runner stopping.");
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void CheckTasks(object unused)
        {
            var now = DateTime.Now;
            foreach (var crontabEntry in _tasks)
            {
                crontabEntry.RunIfDue(now);
            }
        }
    }
}
