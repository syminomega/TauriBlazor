using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi;

/// <inheritdoc />
public class WebviewWindow : ITauriWebviewWindow
{
    internal WebviewWindow(IJSObjectReference webviewWindowRef, TauriJsInterop tauriJsInterop)
    {
        _tauriJsInterop = tauriJsInterop;
        JsObjectRef = webviewWindowRef;
    }
    
    private readonly TauriJsInterop _tauriJsInterop;

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }
    
    /// <summary>
    /// The webview label. It is a unique identifier for the webview, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label => _tauriJsInterop.GetJsProperty<string>(JsObjectRef, "label");
}