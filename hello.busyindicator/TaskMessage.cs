using System;
using System.Threading;
using System.Threading.Tasks;

namespace hello.busyindicator
{
    public class TaskMessage : IDisposable
    {
        private readonly Action<CancellationToken, IProgress<int>> _worker;
        private readonly CancellationTokenSource _ct = new CancellationTokenSource();
        private Task _task;

        public string WaitingFor { get; }
        public CancellationToken Token => _ct.Token;

        public TaskMessage(Action<CancellationToken, IProgress<int>> worker, 
            string waitingFor = null)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            _worker = worker;
            WaitingFor = waitingFor ?? "Please wait...";
        }

        public async Task Run(IProgress<int> progress = null)
        {
            if (_task != null) throw new InvalidOperationException("Worker already started");
            _task = Task.Run(() => _worker(Token, progress), Token);
            await _task;
        }

        public void Cancel() { _ct?.Cancel(); }
        public void Dispose() { _ct?.Dispose(); }
    }
}