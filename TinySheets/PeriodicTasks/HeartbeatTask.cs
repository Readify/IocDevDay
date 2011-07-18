using System;
using TinySheets.Monitoring;
using TinySheets.Tasks;

namespace TinySheets.PeriodicTasks
{
    [TaskFrequency(10000)]
    public class HeartbeatTask : ITask
    {
        readonly ILog<HeartbeatTask> _log;

        public HeartbeatTask(ILog<HeartbeatTask> log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }

        public void Run()
        {
            _log.Debug("Heartbeat task running.");
        }
    }
}
