using Microsoft.JSInterop;

namespace TauriApi.Modules;

/// <summary>
/// TauriApp base class
/// </summary>
public class TauriApp
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.app";

    public TauriApp(IJSRuntime jsRuntime)
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

    /// <summary>
    /// Set app’s theme
    /// </summary>
    /// <param name="theme"></param>
    public async Task SetTheme(Theme theme)
    {
        var themeStr = theme switch
        {
            Theme.Light => "light",
            Theme.Dark => "dark",
            _ => null
        };
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.setTheme", themeStr);
    }

    /// <summary>
    /// Hides the application on macOS
    /// </summary>
    public async Task Hide()
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.hide");
    }

    /// <summary>
    /// Shows the application on macOS. This function does not automatically focus any specific app window.
    /// </summary>
    public async Task Show()
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.show");
    }
}