using System.ComponentModel;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IBusyViewModel : INotifyPropertyChanged, IHandleWithTask<StartTaskMessage>
    {
        bool IsBusy { get; }
        string WaitingFor { get; }
        int Progress { get; }
        bool IsIndeterminate { get; }
        void Cancel();
    }
}