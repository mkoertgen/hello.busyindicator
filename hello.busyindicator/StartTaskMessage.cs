using System;
using System.Threading;

namespace hello.busyindicator
{
    public abstract class StartDelegateMessage<TDelegate>
    {
        public TDelegate Worker { get; }
        public string WaitingFor { get; }

        protected StartDelegateMessage(TDelegate worker, string waitingFor = null)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            Worker = worker;
            WaitingFor = waitingFor ?? "Please wait...";
        }
    }

    public class StartTaskMessage : StartDelegateMessage<Action<CancellationToken,IProgress<int>>>
    {
        public StartTaskMessage(Action<CancellationToken, IProgress<int>> worker,
            string waitingFor = null) : base(worker, waitingFor)
        {
        }
    }

    public class StartThreadMessage : StartDelegateMessage<Action>
    {
        public StartThreadMessage(Action worker, string waitingFor = null)
            : base(worker, waitingFor)
        {
        }
    }
}