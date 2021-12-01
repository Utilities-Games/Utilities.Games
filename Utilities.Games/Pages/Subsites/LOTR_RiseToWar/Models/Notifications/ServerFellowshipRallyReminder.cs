using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Games.Models.Notifications;
using Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models.LocalStores;

namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models.Notifications
{
    public class ServerFellowshipRallyReminder : NotificationTrigger<ServerFellowshipRallyReminder.FellowshipRallyInformation>
    {
        public override string Title => "LOTR: Rise to War - Fellowship Rally Reminder";

        public override string Icon => "/Subsites/LOTR_RiseToWar/images/favicon.ico";

        public override FellowshipRallyInformation Data { get; set; } = new();

        public ServerFellowshipRallyReminder() : base() { }

        public ServerFellowshipRallyReminder(FellowshipRallyInformation details, string message, DateTime reminderTime) : base(message, reminderTime) {
            Data = details;
        }

        public class FellowshipRallyInformation : INotificationData
        {
            public string ReturnUrl { get; set; } = "/lotr_risetowar";

            /// <summary>
            /// Timestamp of when the event starts in local time.
            /// </summary>
            [Required]
            public DateTime EventTime { get; set; } = DateTime.Now.ToLocalTime().AddDays(1);

            /// <summary>
            /// Reference to the server the event takes place in.
            /// </summary>
            [Required]
            public int ServerNumber { get; set; }

            /// <summary>
            /// Reference to the user's Fellowship
            /// </summary>
            [Required]
            public string FellowshipName { get; set; }

        }
    }
}
