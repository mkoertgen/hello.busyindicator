using System;
using System.Threading;
using System.Threading.Tasks;

namespace hello.busyindicator
{
    class TaskAction : IDisposable
    {
        private readonly Action<CancellationToken, IProgress<int>> _worker;
        private readonly CancellationTokenSource _ct = new CancellationTokenSource();

        public TaskAction(Action<CancellationToken, IProgress<int>> worker)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            _worker = worker;
        }

        public async Task Run(IProgress<int> progress = null)
        {
            await Task.Run(() => _worker(_ct.Token, progress), _ct.Token);
        }

        public void Cancel() { _ct?.Cancel(); }

        public void Dispose() { _ct?.Dispose(); }
    }
}