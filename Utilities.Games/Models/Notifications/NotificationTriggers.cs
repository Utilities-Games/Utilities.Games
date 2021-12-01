using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Games.Models.Notifications
{
    /// <summary>
    /// Service for creating and cancelling notifications using the experimental Notification API.
    /// </summary>
    public class NotificationTriggers {
        private readonly IJSRuntime js;
        const string PREFIX = "scheduledNotifications";

        public NotificationTriggers(IJSRuntime js)
        {
            this.js = js;
        }

        public async ValueTask<bool> IsSupported() => await js.InvokeAsync<bool>($"{PREFIX}.isSupported", new object[] { });

        /// <summary>
        /// Creates a new Notification Trigger from a standard <see cref="INotificationTrigger"/> type.
        /// </summary>
        /// <typeparam name="T">Reference to the type of Notification Trigger.</typeparam>
        /// <param name="trigger">Reference to the instance of a trigger.</param>
        public async ValueTask CreateNotificationTrigger<T>(T trigger) where T : INotificationTrigger => await js.InvokeVoidAsync($"{PREFIX}.createScheduledNotification", trigger);

        /// <summary>
        /// Cancels a Notification Trigger by its respective tag.
        /// </summary>
        /// <param name="tag">Reference to the Notification Trigger tag.</param>
        public async ValueTask CancelNotificationTrigger(string tag) => await js.InvokeVoidAsync($"{PREFIX}.cancelScheduledNotification", new object[] { tag });

        /// <summary>
        /// Cancels a Notification Trigger by its respective tag, derived from the <see cref="NotificationTrigger"/> type.
        /// </summary>
        /// <typeparam name="T">Reference to the <see cref="NotificationTrigger"/> type.</typeparam>
        public async ValueTask CancelNotificationTrigger<T>() where T : NotificationTrigger => await CancelNotificationTrigger(typeof(T).Name);

        /// <summary>
        /// Gets all notifications of the specified type.
        /// </summary>
        /// <typeparam name="T">Reference to the type of notification.</typeparam>
        public async Task<IEnumerable<T>> GetNotifications<T>() where T : INotificationTrigger, new()
        {
            return await js.InvokeAsync<T[]>($"{PREFIX}.getScheduledNotifications", new object[] { typeof(T).Name });
        }
    }
}
