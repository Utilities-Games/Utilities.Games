using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models.LocalStores
{
    public static class LOTR_RiseToWar_LocalStore {
        public const string SUBSITE_KEY = "LOTR_RiseToWar";
    }
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
    public class CommanderStore : LocalStore<UserServerCommander>
    {
        public override string SubsiteKey => LOTR_RiseToWar_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "userCommanders";

        public CommanderStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(UserServerCommander item) => item.Id;

        public override ValueTask SaveAsync(UserServerCommander item)
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

        public DateTime LastUpdated { get; set; }
    }

    public class UserServerCommander {
        public string Id { get; set; }// => $"{ServerNumber}:{CommanderName}";

        [Required]
        public int ServerNumber { get; set; }

        [Required]
        public string CommanderName { get; set; }

        public int CurrentLevel { get; set; }

        public DateTime LastUpdated { get; set; }

        // TODO: Maintain Skill Levels

        // TODO: Maintain Equipment + Equipment Levels
    }
}
