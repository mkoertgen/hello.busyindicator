using System;
using System.Threading;

namespace hello.busyindicator
{
    public class StartTaskMessage
    {
        public Action<CancellationToken, IProgress<int>> Worker { get; }
        public string WaitingFor { get; }

        public StartTaskMessage(Action<CancellationToken, IProgress<int>> worker,
            string waitingFor = null)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            Worker = worker;
            WaitingFor = waitingFor ?? "Please wait...";
        }
    }
}