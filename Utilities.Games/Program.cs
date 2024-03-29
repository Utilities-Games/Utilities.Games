using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LOTR_RiseToWar = Utilities.Games.Pages.Subsites.LOTR_RiseToWar;
using TheLegendOfZelda = Utilities.Games.Pages.Subsites.TheLegendOfZelda;
using Halo = Utilities.Games.Pages.Subsites.Halo;

namespace Utilities.Games
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<Utilities.Games.Models.Notifications.NotificationTriggers>();

            builder.Services.AddScoped<LOTR_RiseToWar.Models.LocalStores.ServerStore>();
            builder.Services.AddScoped<LOTR_RiseToWar.Models.LocalStores.CommanderStore>();
            builder.Services.AddScoped<TheLegendOfZelda.Models.LocalStores.IngredientStore>();
            builder.Services.AddScoped<Halo.Models.LocalStores.SkullStore>();
            builder.Services.AddScoped<Halo.Models.LocalStores.RankStore>();

            var host = builder.Build();

            if (builder.HostEnvironment.IsDevelopment()) {
                var config = new WebAssemblyHostConfiguration();
            }

            await host.RunAsync();
        }
    }
}
