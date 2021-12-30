using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models.LocalStores
{
    public class ServerStore : LocalStore<UserServer>
    {
        public override string SubsiteKey => LOTR_RiseToWar_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "userServers";

        public ServerStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(UserServer item) => item.ServerNumber;

        public override ValueTask SaveAsync(UserServer item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return base.SaveAsync(item);
        }
    }
    /// <summary>
    /// Reference to a server that the user has joined.
    /// </summary>
    public class UserServer {
        /// <summary>
        /// Unique number for the server the user has joined.
        /// </summary>
        [Required]
        public int ServerNumber { get; set; }

        /// <summary>
        /// Username of the user on the specified server.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Unique number for the user on the specified server.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name of the faction the user chose to side with.
        /// </summary>
        [Required]
        public string Faction { get; set; }

        /// <summary>
        /// Unique ID of the Fellowship/Warband the user joined.
        /// </summary>
        [MaxLength(4)]
        public string FellowshipId { get; set; }

        /// <summary>
        /// Name of the Fellowship/Warband the user joined.
        /// </summary>
        public string FellowshipName { get; set; }

        public DateTime LastUpdated { get; set; }
    }

}
