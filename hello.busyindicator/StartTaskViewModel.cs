using System;
using System.Threading;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IStartTaskViewModel
    {
        void Start();
    }

    public class StartTaskViewModel : IStartTaskViewModel
    {
        private readonly IEventAggregator _events;

        public StartTaskViewModel(IEventAggregator events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            _events = events;
            _events.Subscribe(this);
        }

        public void Start()
        {
            var message = new TaskMessage(MyLongRunningTask,
                $"Waiting for '{GetType().Name}'...");
            _events.PublishOnUIThread(message);
        }

        public bool CanStart => _events.HandlerExistsFor(typeof(TaskMessage));

        private static void MyLongRunningTask(CancellationToken token, IProgress<int> progress)
        {
            const int n = 20;
            var duration = TimeSpan.FromSeconds(20);

            for (int i = 1; i <= n; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep( (int)(duration.TotalMilliseconds / n) );
                progress?.Report((100 * i) / n);
            }
        }
    }
}