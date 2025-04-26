using TauriApi.Modules;

namespace TauriApi;

/// <summary>
/// Tauri API
/// </summary>
public class Tauri
{
    /// <summary>
    /// Inject Tauri api modules.
    /// </summary>
    public Tauri(
        TauriApp tauriApp,
        TauriCore tauriCore,
        TauriEvent tauriEvent,
        TauriWindow tauriWindow,
        TauriWebview tauriWebview,
        TauriWebviewWindow tauriWebviewWindow)
    {
        App = tauriApp;
        Core = tauriCore;
        Event = tauriEvent;
        Window = tauriWindow;
        Webview = tauriWebview;
        WebviewWindow = tauriWebviewWindow;
    }

    /// <summary>
    /// @tauri-apps/api/app
    /// </summary>
    public TauriApp App { get; }

    /// <summary>
    /// @tauri-apps/api/core
    /// </summary>
    public TauriCore Core { get; }

    /// <summary>
    /// @tauri-apps/api/event
    /// </summary>
    public TauriEvent Event { get; }

    /// <summary>
    /// @tauri-apps/api/window
    /// </summary>
    public TauriWindow Window { get; }

    /// <summary>
    /// @tauri-apps/api/webview
    /// </summary>
    public TauriWebview Webview { get; set; }

    /// <summary>
    /// @tauri-apps/api/webviewWindow
    /// </summary>
    public TauriWebviewWindow WebviewWindow { get; set; }
}