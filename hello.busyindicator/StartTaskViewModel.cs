using System;
using System.Threading;
using Caliburn.Micro;

namespace hello.busyindicator
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class StartTaskViewModel : IStartTaskViewModel
    {
        private readonly IEventAggregator _events;

        public StartTaskViewModel(IEventAggregator events)
        {
            _events = events ?? throw new ArgumentNullException(nameof(events));
        }

        public void StartResponsive()
        {
            var message = new StartTaskMessage(
                MyLongRunningTask, nameof(MyLongRunningTask));
            _events.PublishOnUIThread(message);
        }

        public void StartNonResponsive()
        {
            var message = new StartThreadMessage(MyForeverRunningTask, nameof(MyForeverRunningTask));
            _events.PublishOnUIThread(message);
        }

        public void StartExceptional()
        {
            var message = new StartThreadMessage(MyExceptionalTask, nameof(MyExceptionalTask));
            _events.PublishOnUIThread(message);
        }

        // ReSharper disable once UnusedMember.Global
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

        private static void MyForeverRunningTask()
        {
            while (true) { Thread.Sleep(500); }
        }

        private static void MyExceptionalTask()
        {
            Thread.Sleep(1000);
            throw new InvalidOperationException();
        }


    }
}