using System;

namespace hello.busyindicator
{
    public class TaskExceptionMessage
    {
        public string TaskName { get; }
        public Exception Exception { get; }

        public bool Handled { get; set; }

        public TaskExceptionMessage(string taskName, Exception exception)
        {
            TaskName = taskName;
            Exception = exception;
        }
    }
}