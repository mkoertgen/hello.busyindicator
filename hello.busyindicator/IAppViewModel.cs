using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IAppViewModel 
        : IHandle<TaskState>
        , IHandle<TaskExceptionMessage>
    {
        IBusyViewModel BusyIndicator { get; }
        IMainViewModel MainView { get; }

        string Notification { get; set; }
        void DoQuit();
        void DoGoBack();
        void DoGoForward();
        void DoShowHelp();
    }
}