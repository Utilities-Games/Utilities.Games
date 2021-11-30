namespace Utilities.Games.Models
{
    /// <summary>
    /// An interface for defining types of Notification data.
    /// </summary>
    public interface INotificationData {
        /// <summary>
        /// Return URL the user should be directed to when notification is clicked.
        /// </summary>
        string ReturnUrl { get; set; }
    }
}
