namespace TauriApi.Interfaces;

/// <summary>
/// The WebviewWindow instance to communicate with the window and webview.
/// </summary>
public interface ITauriWebviewWindow : ITauriWebview, ITauriWindow
{
    /// <summary>
    /// The webview label. It is a unique identifier for the webview, can be used to reference it later.
    /// </summary>
    public new ValueTask<string> Label { get; }

    ValueTask<string> ITauriWebview.Label => Label;
    ValueTask<string> ITauriWindow.Label => Label;
}