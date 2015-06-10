using System.Diagnostics.Tracing;

namespace hello.busyindicator
{
    public class NotificationEvent
    {
        public string Notification { get; private set; }
        public EventLevel Level { get; private set; }

        public NotificationEvent(string notification, EventLevel level = EventLevel.Informational)
        {
            Notification = notification;
            Level = level;
        }
    }
}