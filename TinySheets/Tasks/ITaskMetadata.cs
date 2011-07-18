using System;

namespace TinySheets.Tasks
{
    public interface ITaskMetadata
    {
        TimeSpan Frequency { get; }
    }
}
