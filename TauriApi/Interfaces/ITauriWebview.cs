namespace TauriApi.Interfaces;

/// <summary>
/// Create new webview or get a handle to an existing one.
/// Webviews are identified by a label a unique identifier that can be used to reference it later.
/// It may only contain alphanumeric characters a-zA-Z plus the following special characters -, /, : and _.
/// </summary>
public interface ITauriWebview : ITauriObject
{
    /// <summary>
    /// The webview label. It is a unique identifier for the webview, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label { get; }
    
    // TODO: listeners
    // window
}