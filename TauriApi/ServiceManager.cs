using Microsoft.Extensions.DependencyInjection;
using TauriApi.Modules;
using TauriApi.Utilities;

namespace TauriApi;

public static class ServiceManager
{
    public static void AddTauriApi(this IServiceCollection services)
    {
        services.AddSingleton<TauriJsInterop>();
        services.AddSingleton<Tauri>();
        
        services.AddSingleton<TauriApp>();
        services.AddSingleton<TauriCore>();
        services.AddSingleton<TauriEvent>();
        services.AddSingleton<TauriWindow>();
    }
}