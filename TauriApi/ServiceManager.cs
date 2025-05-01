using Microsoft.Extensions.DependencyInjection;
using TauriApi.Modules;
using TauriApi.Utilities;

namespace TauriApi;

/// <summary>
/// Service manager for TauriApi.
/// </summary>
public static class ServiceManager
{
    /// <summary>
    /// Adds the TauriApi services to the service collection.
    /// </summary>
    public static void AddTauriApi(this IServiceCollection services)
    {
        services.AddSingleton<TauriJsInterop>();
        services.AddSingleton<TauriEventManager>();
        services.AddSingleton<Tauri>();
        
        services.AddSingleton<TauriApp>();
        services.AddSingleton<TauriCore>();
        services.AddSingleton<TauriEvent>();
        services.AddSingleton<TauriImage>();
        services.AddSingleton<TauriWindow>();
        services.AddSingleton<TauriWebview>();
        services.AddSingleton<TauriWebviewWindow>();
    }
}