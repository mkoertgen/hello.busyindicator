using System;
using System.Threading;

namespace hello.busyindicator
{
    public interface IStartDelegateInfo
    {
        string TaskName { get; }
    }

    public abstract class StartDelegateMessage<TDelegate> : IStartDelegateInfo
    {
        public string TaskName { get; }
        public TDelegate Worker { get; }

        protected StartDelegateMessage(TDelegate worker, string taskName)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            if (string.IsNullOrWhiteSpace(taskName)) throw new ArgumentNullException(nameof(taskName));
            Worker = worker;
            TaskName = taskName;
        }
    }

    public class StartTaskMessage : StartDelegateMessage<Action<CancellationToken,IProgress<int>>>
    {
        public StartTaskMessage(Action<CancellationToken, IProgress<int>> worker, string taskName) 
            : base(worker, taskName)
        {
        }
    }

    public class StartThreadMessage : StartDelegateMessage<Action>
    {
        public StartThreadMessage(Action worker, string taskName)
            : base(worker, taskName)
        {
        }
    }
}