using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

/// <summary>
/// Provides APIs to create webviews, communicate with other webviews and manipulate the current webview.
/// </summary>
public class TauriWebviewModule
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.webview";
    private readonly TauriJsInterop _tauriJsInterop;
    private readonly TauriEventModule _tauriEvent;

    /// <summary>
    /// Inject TauriWebview.
    /// </summary>
    public TauriWebviewModule(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop, TauriEventModule tauriEvent)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
        _tauriEvent = tauriEvent;
    }

    /// <summary>
    /// Creates a new Webview.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="label">The unique webview label. Must be alphanumeric: a-zA-Z-/:_.</param>
    /// <param name="options"></param>
    /// <returns>The Webview instance to communicate with the webview.</returns>
    public async Task<ITauriWebview> CreateWebview(ITauriWindow window, string label, WebviewStandaloneOptions options)
    {
        var windowLabel = await window.Label;
        var webviewRef = await _tauriJsInterop.ConstructWebview(windowLabel, label, options);
        var webview = new TauriWebview(webviewRef, _tauriJsInterop, _tauriEvent);
        return webview;
    }
    
    
}