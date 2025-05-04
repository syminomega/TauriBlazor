using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

/// <summary>
/// The WebviewWindow instance to communicate with the window and webview.
/// </summary>
public class WebviewWindow : ITauriWindow, ITauriWebview
{
    internal WebviewWindow(IJSObjectReference webviewWindowRef)
    {
        JsObjectRef = webviewWindowRef;
    }

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }
}