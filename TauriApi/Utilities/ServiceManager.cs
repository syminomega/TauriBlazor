using Microsoft.Extensions.DependencyInjection;

namespace TauriApi.Utilities;

public static class ServiceManager
{
    public static void AddTauriApi(this IServiceCollection services)
    {
        services.AddSingleton<Tauri>();
    }
}