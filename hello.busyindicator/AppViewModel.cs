using System;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface IAppViewModel : IScreen
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class AppViewModel : Screen, IAppViewModel
    {
        public AppViewModel(IStartTaskViewModel starter, INotificationsViewModel notifications)
        {
            if (starter == null) throw new ArgumentNullException(nameof(starter));
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));
            Starter = starter;
            Notifications = notifications;
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public IStartTaskViewModel Starter { get; }

        public INotificationsViewModel Notifications { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore once MemberCanBePrivate.Global

    }
}