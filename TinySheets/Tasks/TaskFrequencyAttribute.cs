using System;

namespace TinySheets.Tasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskFrequencyAttribute : Attribute, ITaskMetadata
    {
        readonly TimeSpan _frequency;

        public TaskFrequencyAttribute(int frequencyMilliseconds)
        {
            _frequency = TimeSpan.FromMilliseconds(frequencyMilliseconds);
        }

        public TimeSpan Frequency
        {
            get { return _frequency; }
        }
    }
}
