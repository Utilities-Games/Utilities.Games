using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Utilities.Games.Models.Notifications
{
    /// <summary>
    /// An abstract implementation of a Notification Trigger type.
    /// </summary>
    public abstract class NotificationTrigger : INotificationTrigger
    {
        const long epoch_time = 621_355_968_000_000_000;
        const long ticks_per_millisecond = 10_000;

        public string Tag => GetType().Name;

        public abstract string Title { get; }

        [MaxLength(500)]
        public virtual string Body { get; set; } = string.Empty;

        protected long _timestamp { get; set; }
        public virtual long Timestamp  {
            get {
                return _timestamp;
            }
            set {
                _timestamp = value;
                if (_triggerTime == default)
                {
                    var ticks = epoch_time + (_timestamp * ticks_per_millisecond); // Convert JS Ticks to C# Ticks
                    _triggerTime = new DateTime(ticks, DateTimeKind.Utc);
                }
            }
        }

        protected DateTime _triggerTime { get; set; }
        public virtual DateTime TriggerTime  {
            get {
                return _triggerTime;
            }
            set
            {
                _triggerTime = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
                if (_timestamp == default)
                {
                    var ticks = (_triggerTime.Ticks - epoch_time) / ticks_per_millisecond; // Convert C# Ticks to JS Ticks
                    _timestamp = ticks;
                }
            }
        }

        public virtual string Icon { get; set; }

        public virtual INotificationData Data { get; set; }

        public NotificationTrigger() { }

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

        public NotificationTrigger(string token)
        {
            var trigger = FromToken<NotificationTrigger>(token);
            if (trigger != null)
            {
                Body = trigger.Body;
                TriggerTime = trigger.TriggerTime;
                Icon = trigger.Icon;
                Data = trigger.Data;
            }
        }

        /// <summary>
        /// Creates a sharable token of the trigger.
        /// </summary>
        /// <returns></returns>
        public string Tokenize()
        {
            string serializedObject = JsonConvert.SerializeObject(this, Formatting.None);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(serializedObject);
            return Convert.ToBase64String(tokenBytes);
        }

        /// <summary>
        /// Creates a <see cref="NotificationTrigger"/> from a token.
        /// </summary>
        /// <param name="token">Sharable token of a trigger.</param>
        /// <returns>Deserialized <see cref="NotificationTrigger"/>.</returns>
        public static T FromToken<T>(string token) where T : NotificationTrigger
        {
            byte[] tokenBytes = Convert.FromBase64String(token);
            string serializedObject = Encoding.UTF8.GetString(tokenBytes);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }

    public abstract class NotificationTrigger<T> : NotificationTrigger where T : new()
    {
        public virtual new T Data { get; set; } = new();

        public NotificationTrigger() : base() { }

        public NotificationTrigger(string message, DateTime triggerTime) : base(message, triggerTime) { }
    }
}
