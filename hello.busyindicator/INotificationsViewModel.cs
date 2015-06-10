using System.Collections.Generic;
using Caliburn.Micro;

namespace hello.busyindicator
{
    public interface INotificationsViewModel : IHandle<NotificationEvent>

    {
        IEnumerable<string> Items { get; }
        string SelectedItem { get; set; }
        string LastItem { get; set; }

        void Add(string notification);
        int MaxItems { get; set; }

        bool ItemsVisible { get; set; }
    }
}