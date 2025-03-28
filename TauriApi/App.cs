using Microsoft.JSInterop;

namespace TauriApi;

/// <summary>
/// TauriApp base class
/// </summary>
public class App
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.app";

    public App(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Gets the application name
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetName()
    {
        return await _jsRuntime.InvokeAsync<string>($"{Prefix}.getName");
    }
    /// <summary>
    /// Gets the Tauri version
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetTauriVersion()
    {
        return await _jsRuntime.InvokeAsync<string>($"{Prefix}.getTauriVersion");
    }
    /// <summary>
    /// Gets the application version
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetVersion()
    {
        return await _jsRuntime.InvokeAsync<string>($"{Prefix}.getVersion");
    }
    
}