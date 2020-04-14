using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public class BusyViewModel : PropertyChangedBase, IBusyViewModel
    {
        private readonly IEventAggregator _events;
        private bool _isBusy;
        private string _waitingFor;
        private TaskAction _taskAction;
        private int _progress;

        public BusyViewModel(IEventAggregator events)
        {
            _events = events ?? throw new ArgumentNullException(nameof(events));
            _events.Subscribe(this);
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
                // ReSharper disable once ExplicitCallerInfoArgument
                NotifyOfPropertyChange(nameof(IsIndeterminate));
            }
        }

        public bool IsIndeterminate => _progress < 0;

        public void Cancel()
        {
            if (_taskAction == null) return;
            WaitingFor = $"Cancelling '{_taskAction.Name}'...";
            Progress = -1;
            _taskAction.Cancel();
        }

        public async Task Handle(StartTaskMessage message)
        {
            await Run(new TaskAction(message.Worker, message.TaskName));
        }

        public async Task Handle(StartThreadMessage message)
        {
            await Run(new TaskAction(message.Worker, message.TaskName));
        }

        private async Task Run(TaskAction taskAction)
        {
            using (_taskAction = taskAction)
            {
                try
                {
                    WaitingFor = $"Waiting for '{_taskAction.Name}'...";
                    Progress = -1;
                    IsBusy = true;
                    _events.PublishOnUIThread(TaskState.Started);

                    await _taskAction.Run(new Progress<int>(p => Progress = p));

                    _events.PublishOnUIThread(TaskState.Completed);
                }
                catch (OperationCanceledException)
                {
                    _events.PublishOnUIThread(TaskState.Canceled);
                }
                catch (Exception ex)
                {
                    _events.PublishOnUIThread(TaskState.Faulted);
                    var msg = new TaskExceptionMessage(_taskAction.Name, ex);
                    _events.PublishOnUIThread(msg);
                    if (!msg.Handled)
                        throw;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}