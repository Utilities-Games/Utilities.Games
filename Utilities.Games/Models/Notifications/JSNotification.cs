using System;

namespace Utilities.Games.Models.Notifications
{
    /// <summary>
    /// Mirror of JS Notification
    /// </summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/API/notification"/>
    public class JSNotification {
        public string badge { get; set; }

        public string body { get; set; }

        public object data { get; set; }

        public string tag { get; set; }

        public string icon { get; set; }

        public string image { get; set; }

        public long timestamp { get; set; }

        public string title { get; set; }
    }
    public class JSNotification<T> : JSNotification
    {
        public new T data { get; set; }
    }
}
