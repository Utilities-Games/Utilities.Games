using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities.Games.Models
{
    /// <summary>
    /// An interface for implementing new scheduled notifications via the experimental Notification API.
    /// </summary>
    public interface INotificationTrigger {
        /// <summary>
        /// An identifier for the type of notification.
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// Display text for the title of the notification.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Display message of the notification.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Specific time that the notification should be triggered.
        /// </summary>
        DateTime TriggerTime { get; set; }

        /// <summary>
        /// Icon to be displayed with notification.
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        /// Optional data to be provided
        /// </summary>
        INotificationData Data { get; set; }
    }
}
