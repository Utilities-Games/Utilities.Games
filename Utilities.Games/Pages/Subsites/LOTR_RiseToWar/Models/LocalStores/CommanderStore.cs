using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models.LocalStores
{
    public class CommanderStore : LocalStore<UserServerCommander>
    {
        public override string SubsiteKey => LOTR_RiseToWar_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "userCommanders";

        public CommanderStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(UserServerCommander item) => item.Id;

        public override ValueTask SaveAsync(UserServerCommander item)
        {
            item.Id = $"{item.ServerNumber}:{item.CommanderName}";
            item.LastUpdated = DateTime.UtcNow;
            return base.SaveAsync(item);
        }
    }
    public class UserServerCommander
    {
        public string Id { get; set; }

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
