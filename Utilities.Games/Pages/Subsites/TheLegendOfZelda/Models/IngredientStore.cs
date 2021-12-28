using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.Games.Models;

namespace Utilities.Games.Pages.Subsites.TheLegendOfZelda.Models.LocalStores
{
    public static class TheLegendOfZelda_LocalStore {
        public const string SUBSITE_KEY = "TheLegendOfZelda";
    }
    public class IngredientStore : LocalStore<UserIngredient>
    {
        public override string SubsiteKey => TheLegendOfZelda_LocalStore.SUBSITE_KEY;
        public override string STORE_NAME => "userIngredients";

        public IngredientStore(HttpClient httpClient, IJSRuntime js) : base(httpClient, js) { }

        public override object GetKey(UserIngredient item) => item.Name;
    }
    public class UserIngredient {
        /// <summary>
        /// Name of the ingredient the player owns or has in inventory.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Quantity of this item that the player owns or has in inventory.
        /// </summary>
        public int Quantity { get; set; }
    }
}
