using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TauriApi;
using TauriApi.Plugins;
using TauriBlazorDemo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add TauriApi services
builder.Services.AddTauriApi();
builder.Services.AddTauriPlugin<TauriDialog>();

await builder.Build().RunAsync();
