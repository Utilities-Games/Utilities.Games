using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.Halo.Models.LocalStores
{
    public class RankStore : LocalStore<CurrentRank>
    {
        public override string SubsiteKey => Halo_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "ranks";

        public RankStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(CurrentRank item) => item.Installment;

        public override ValueTask SaveAsync(CurrentRank item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return base.SaveAsync(item);
        }
    }

    public class CurrentRank
    {
        /// <summary>
        /// Name of the installment the rank applies to.
        /// </summary>
        [Required]
        public string Installment { get; set; }

        /// <summary>
        /// Name of the rank achieved on the installment.
        /// </summary>
        public string Rank { get; set; }

        public DateTime LastUpdated { get; set; }
    }

}
