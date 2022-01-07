using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.Halo.Models.LocalStores
{
    public class SkullStore : LocalStore<CollectedSkull>
    {
        public override string SubsiteKey => Halo_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "skulls";

        public SkullStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(CollectedSkull item) => item.Id;
    }
    /// <summary>
    /// Reference to a server that the user has joined.
    /// </summary>
    public class CollectedSkull {
        /// <summary>
        /// Unique id for the instance of a collected skull.
        /// </summary>
        public string Id => $"{Installment}-{Skull}";

        /// <summary>
        /// Unique number for the server the user has joined.
        /// </summary>
        [Required]
        public string Skull { get; set; }

        /// <summary>
        /// Name of the installment the skull was collected on.
        /// </summary>
        [Required]
        public string Installment { get; set; }
    }

}
