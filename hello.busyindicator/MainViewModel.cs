using System;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IMainViewModel : IScreen { }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainViewModel : Screen, IMainViewModel
    {
        public MainViewModel(IStartTaskViewModel starter)
        {
            if (starter == null) throw new ArgumentNullException(nameof(starter));
            Starter = starter;
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public IStartTaskViewModel Starter { get; }
    }
}