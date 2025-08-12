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
        TauriAppModule tauriApp,
        TauriCoreModule tauriCore,
        TauriEventModule tauriEvent,
        TauriImageModule tauriImage,
        TauriPathModule tauriPath,
        TauriWindowModule tauriWindow,
        TauriWebviewModule tauriWebview,
        TauriWebviewWindowModule tauriWebviewWindow)
    {
        App = tauriApp;
        Core = tauriCore;
        Event = tauriEvent;
        Image = tauriImage;
        Path = tauriPath;
        Window = tauriWindow;
        Webview = tauriWebview;
        WebviewWindow = tauriWebviewWindow;
    }

    /// <summary>
    /// @tauri-apps/api/app
    /// </summary>
    public TauriAppModule App { get; }

    /// <summary>
    /// @tauri-apps/api/core
    /// </summary>
    public TauriCoreModule Core { get; }

    /// <summary>
    /// @tauri-apps/api/event
    /// </summary>
    public TauriEventModule Event { get; }
    
    /// <summary>
    /// @tauri-apps/api/image
    /// </summary>
    public TauriImageModule Image { get; }
    
    /// <summary>
    /// @tauri-apps/api/path
    /// </summary>
    public TauriPathModule Path { get; }

    /// <summary>
    /// @tauri-apps/api/window
    /// </summary>
    public TauriWindowModule Window { get; }

    /// <summary>
    /// @tauri-apps/api/webview
    /// </summary>
    public TauriWebviewModule Webview { get; set; }

    /// <summary>
    /// @tauri-apps/api/webviewWindow
    /// </summary>
    public TauriWebviewWindowModule WebviewWindow { get; set; }
}