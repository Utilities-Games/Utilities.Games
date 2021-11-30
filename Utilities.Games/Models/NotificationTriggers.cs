using Microsoft.JSInterop;

namespace Utilities.Games.Models
{
    /// <summary>
    /// Service for creating and cancelling notifications using the experimental Notification API.
    /// </summary>
    public class NotificationTriggers {
        private readonly IJSRuntime js;

        public NotificationTriggers(IJSRuntime js)
        {
            this.js = js;
        }

        /// <summary>
        /// Creates a new Notification Trigger from a standard <see cref="INotificationTrigger"/> type.
        /// </summary>
        /// <typeparam name="T">Reference to the type of Notification Trigger.</typeparam>
        /// <param name="trigger">Reference to the instance of a trigger.</param>
        public void CreateNotificationTrigger<T>(T trigger) where T : INotificationTrigger
        {
            js.InvokeVoidAsync("createScheduledNotification", new object [] { trigger.Tag, trigger.Title, trigger.Body, trigger.TriggerTime });
        }

        /// <summary>
        /// Cancels a Notification Trigger by its respective tag.
        /// </summary>
        /// <param name="tag">Reference to the Notification Trigger tag.</param>
        public void CancelNotificationTrigger(string tag) {
            js.InvokeVoidAsync("cancelScheduledNotification", new object[] { tag });
        }

        /// <summary>
        /// Cancels a Notification Trigger by its respective tag, derived from the <see cref="NotificationTrigger"/> type.
        /// </summary>
        /// <typeparam name="T">Reference to the <see cref="NotificationTrigger"/> type.</typeparam>
        public void CancelNotificationTrigger<T>() where T : NotificationTrigger => CancelNotificationTrigger(typeof(T).Name);
    }
}
