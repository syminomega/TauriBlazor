using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

/// <summary>
/// Create new webview or get a handle to an existing one.
/// Webviews are identified by a label a unique identifier that can be used to reference it later.
/// It may only contain alphanumeric characters a-zA-Z plus the following special characters -, /, : and _.
/// </summary>
public class Webview : ITauriWebview
{
    internal Webview(IJSObjectReference webviewRef)
    {
        JsObjectRef = webviewRef;
    }

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }
}