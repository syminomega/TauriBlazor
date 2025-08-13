using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi.Modules;

/// <summary>
/// Provides APIs to communicate with the window and webview.
/// </summary>
public class TauriWebviewWindowModule
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.webviewWindow";
    private readonly TauriJsInterop _tauriJsInterop;
    private readonly TauriEventModule _tauriEvent;

    /// <summary>
    /// Inject TauriWebviewWindow.
    /// </summary>
    public TauriWebviewWindowModule(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop, TauriEventModule tauriEvent)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
        _tauriEvent = tauriEvent;
    }

    /// <summary>
    /// Creates a new WebviewWindow.
    /// </summary>
    /// <param name="label">The unique window label. Must be alphanumeric: a-zA-Z-/:_.</param>
    /// <param name="windowOptions"></param>
    /// <param name="webviewOptions">width, height, x, y will be omitted.</param>
    /// <returns>The Window instance to communicate with the window.</returns>
    public async Task<ITauriWebviewWindow> CreateWebviewWindow(string label,
        WindowOptions? windowOptions,
        WebviewOptions? webviewOptions)
    {
        var webviewWindowRef = await _tauriJsInterop.ConstructWebviewWindow(label, windowOptions, webviewOptions);
        var webviewWindow = new TauriWebviewWindow(webviewWindowRef, _tauriJsInterop, _tauriEvent);
        return webviewWindow;
    }
}