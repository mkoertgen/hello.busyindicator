using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IBusyRibbon<TScreen> : IScreen
        where TScreen : IScreen
    {
        IBusyViewModel<TScreen> BusyScreen { get; }
    }
}