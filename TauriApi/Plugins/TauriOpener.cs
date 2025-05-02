using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi.Plugins;

/// <summary>
/// Open files and URLs using their default application.
/// </summary>
public class TauriOpener : ITauriPlugin
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Inject TauriOpener.
    /// </summary>
    public TauriOpener(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private const string Prefix = "__TAURI__.opener";

    /// <summary>
    /// Opens a path with the system’s default app, or the one specified with openWith.
    /// </summary>
    /// <param name="path">The path to open.</param>
    /// <param name="openWith">The app to open the path with. If not specified,
    /// defaults to the system default application for the specified path type.</param>
    public async Task OpenPath(string path, string? openWith = null)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.openPath", path, openWith);
    }

    /// <summary>
    /// Opens a url with the system’s default app, or the one specified with openWith.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    /// <param name="openWith">The app to open the URL with. If not specified,
    /// defaults to the system default application for the specified url type.</param>
    public async Task OpenUrl(string url, string? openWith = null)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.openUrl", url, openWith);
    }

    /// <summary>
    /// Reveal a path with the system’s default explorer.
    /// <li>Android / iOS: Unsupported.</li>
    /// </summary>
    /// <param name="path">The path to reveal.</param>
    public async Task RevealItemInDir(string path)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.revealItemInDir", path);
    }
}