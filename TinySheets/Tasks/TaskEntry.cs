using System;
using System.Threading;

namespace TinySheets.Tasks
{
    public class TaskEntry
    {
        readonly TimeSpan _maxRunTime;
        readonly TimeSpan _frequency;
        readonly Action _task;
        readonly Action<Exception> _handleException;
        readonly object _lock = new object();
        DateTime? _nextRun;

        // If the task takes longer than 10x frequency to execute it
        // will be re-run.
        public TaskEntry(TimeSpan frequency, Action task, Action<Exception> handleException)
        {
            if (task == null) throw new ArgumentNullException("task");
            if (handleException == null) throw new ArgumentNullException("handleException");
            _frequency = frequency;
            _task = task;
            _handleException = handleException;
            _maxRunTime = TimeSpan.FromMilliseconds(10 * _frequency.TotalMilliseconds);
            _task = task;
        }

        public void RunIfDue(DateTime now)
        {
            lock (_lock)
            {
                if (!_nextRun.HasValue || _nextRun.Value <= now)
                {
                    _nextRun = now + _maxRunTime - _frequency;
                    var nextRunAtStart = _nextRun.Value;

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _task();
                        }
                        catch (Exception ex)
                        {
                            _handleException(ex);
                        }
                        finally
                        {
                            lock (_lock)
                            {
                                if (_nextRun == nextRunAtStart)
                                    _nextRun = now + _frequency;
                            }
                        }
                    });
                }
            }
        }
    }
}