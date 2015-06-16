using System;
using System.Threading;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IStartTaskViewModel
    {
        void Start();
        void StartNonResponsive();
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class StartTaskViewModel : IStartTaskViewModel
    {
        private readonly IEventAggregator _events;

        public StartTaskViewModel(IEventAggregator events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            _events = events;
        }

        public void Start()
        {
            var message = new StartTaskMessage(
                MyLongRunningTask,
                "Waiting for 'long running process'...");
            _events.PublishOnUIThread(message);
        }

        public void StartNonResponsive()
        {
            var message = new StartThreadMessage(MyForeverRunningTask,
                "Waiting for 'long running, non responsive  process'...");
            _events.PublishOnUIThread(message);
        }

        private static void MyForeverRunningTask()
        {
            while (true)
            {
                Thread.Sleep(500);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool CanStart => _events.HandlerExistsFor(typeof(StartTaskMessage));

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