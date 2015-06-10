using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public class BusyViewModel<TScreen>
        : PropertyChangedBase
        , IBusyViewModel<TScreen>
        where TScreen : IScreen
    {
        private readonly IEventAggregator _events;
        public TScreen Screen { get; }

        private bool _isBusy;
        private string _waitingFor;
        private TaskMessage _taskMessage;
        private int _progress;

        public BusyViewModel(IEventAggregator events, TScreen screen)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            if (screen == null) throw new ArgumentNullException(nameof(screen));
            _events = events;
            _events.Subscribe(this);
            Screen = screen;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        public string WaitingFor
        {
            get { return _waitingFor; }
            private set
            {
                if (_waitingFor == value) return;
                _waitingFor = value;
                NotifyOfPropertyChange();
            }
        }

        public int Progress
        {
            get { return _progress; }
            private set
            {
                if (_progress == value) return;
                _progress = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(IsIndeterminate));
            }
        }

        public bool IsIndeterminate => _progress < 0;

        public void Cancel()
        {
            WaitingFor = "Canceling...";
            Progress = -1;
            _taskMessage?.Cancel();
        }

        public async Task Handle(TaskMessage message)
        {
            using (_taskMessage = message)
            {
                _events.PublishOnUIThread(new NotificationEvent("Task started."));

                WaitingFor = message.WaitingFor;
                Progress = -1;
                IsBusy = true;
                try
                {
                    await message.Run(new Progress<int>(p => Progress = p));
                    _events.PublishOnUIThread(new NotificationEvent("Task completed."));
                }
                catch (OperationCanceledException)
                {
                    _events.PublishOnUIThread(new NotificationEvent("Task canceled."));
                }
                finally { IsBusy = false; }
            }
        }
    }
}