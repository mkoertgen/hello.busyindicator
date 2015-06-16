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

        public TaskAction(Action worker)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            _worker = (token, progress) =>
            {
                // NOTE: not really nice, cf.:
                // https://social.msdn.microsoft.com/Forums/vstudio/en-US/d0bcb415-fb1e-42e4-90f8-c43a088537fb/aborting-a-long-running-task-in-tpl
                var t = Thread.CurrentThread;
                using (_ct.Token.Register(t.Abort))
                {
                    try { worker(); }
                    catch (ThreadAbortException ex)
                    {
                        throw new OperationCanceledException("Blocking thread was canceled", ex);
                    }
                }
            };
        }

        public async Task Run(IProgress<int> progress = null)
        {
            await Task.Run(() => _worker(_ct.Token, progress), _ct.Token);
        }

        public void Cancel() { _ct?.Cancel(); }

        public void Dispose() { _ct?.Dispose(); }
    }
}