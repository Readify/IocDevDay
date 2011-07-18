using System;

namespace TinySheets.Monitoring
{
    // ReSharper disable UnusedTypeParameter
    public interface ILog<TClient>
    // ReSharper restore UnusedTypeParameter
    {
        void Debug(string message, params object[] formatArgs);
        void Information(string message, params object[] formatArgs);
        void Warning(string message, params object[] formatArgs);
        void Error(Exception exception, string message, params object[] formatArgs);
        void Error(string message, params object[] formatArgs);
    }
}