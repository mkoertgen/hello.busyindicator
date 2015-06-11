using System;
using System.Windows;
using Caliburn.Micro;

namespace hello.busyindicator
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AppViewModel : Screen, IAppViewModel
    {
        public IBusyViewModel BusyIndicator { get; }
        public IMainViewModel MainView { get; }

        private string _notification;

        public AppViewModel(IEventAggregator events, IBusyViewModel busyIndicator, IMainViewModel mainView)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            if (busyIndicator == null) throw new ArgumentNullException(nameof(busyIndicator));
            if (mainView == null) throw new ArgumentNullException(nameof(mainView));
            events.Subscribe(this);
            BusyIndicator = busyIndicator;
            MainView = mainView;
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            DisplayName = "hello.busyindicator";
        }

        public string Notification
        {
            get { return _notification; }
            set
            {
                if (_notification == value) return;
                _notification = value;
                NotifyOfPropertyChange();
            }
        }

        public void Handle(TaskState taskState)
        {
            Notification = $"Task {taskState}.";
        }

        public void DoQuit() { TryClose(); }

        // ReSharper disable UnusedMember.Global
        public void DoGoBack() { ShowNotImplemented(); }
        public void DoGoForward() { ShowNotImplemented(); }
        public void DoShowHelp() { ShowNotImplemented(); }
        // ReSharper restore UnusedMember.Global


        private static void ShowNotImplemented() { MessageBox.Show("Not implemented"); }
    }
}