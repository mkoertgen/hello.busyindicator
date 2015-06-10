using System;
using System.ComponentModel;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IBusyViewModel<out TScreen>
        : INotifyPropertyChanged
        , IHandleWithTask<TaskMessage>
        where TScreen : IScreen
    {
        bool IsBusy { get; }
        string WaitingFor { get; }
        void Cancel();
        TimeSpan TimeOut { get; set; }

        int Progress { get; }
        bool IsIndeterminate { get; }

        TScreen Screen { get; }
    }
}