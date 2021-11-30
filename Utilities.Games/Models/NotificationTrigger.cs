using System;

namespace Utilities.Games.Models
{
    /// <summary>
    /// An abstract implementation of a Notification Trigger type.
    /// </summary>
    public abstract class NotificationTrigger : INotificationTrigger {
        public string Tag => GetType().Name;

        public abstract string Title { get; }

        public string Body { get; set; }

        public DateTime TriggerTime { get; set; }

        public string Icon { get; set; }

        public INotificationData Data { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="INotificationTrigger"/> with a specific message and trigger time.
        /// </summary>
        /// <param name="message">Body of the message within the Notification.</param>
        /// <param name="triggerTime">Reference to the time the trigger should take place.</param>
        public NotificationTrigger(string message, DateTime triggerTime)
        {
            Body = message;
            TriggerTime = triggerTime;
        }
    }
}
