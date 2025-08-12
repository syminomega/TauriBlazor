using Microsoft.Extensions.DependencyInjection;
using TauriApi.Interfaces;
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
        services.AddSingleton<Tauri>();
        
        services.AddSingleton<TauriAppModule>();
        services.AddSingleton<TauriCoreModule>();
        services.AddSingleton<TauriEventModule>();
        services.AddSingleton<TauriImageModule>();
        services.AddSingleton<TauriPathModule>();
        services.AddSingleton<TauriWindowModule>();
        services.AddSingleton<TauriWebviewModule>();
        services.AddSingleton<TauriWebviewWindowModule>();
    }

    /// <summary>
    /// Adds a Tauri plugin to the service collection.
    /// </summary>
    public static void AddTauriPlugin<T>(this IServiceCollection services) where T : class, ITauriPlugin
    {
        services.AddSingleton<T>();
    }
}