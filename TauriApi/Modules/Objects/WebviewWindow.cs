using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Modules;
using TauriApi.Utilities;

namespace TauriApi;

/// <inheritdoc />
internal class TauriWebviewWindow : ITauriWebviewWindow
{
    private readonly TauriJsInterop _tauriJsInterop;
    private readonly TauriEventModule _tauriEvent;

    internal TauriWebviewWindow(IJSObjectReference webviewWindowRef,
        TauriJsInterop tauriJsInterop, TauriEventModule tauriEvent)
    {
        _tauriJsInterop = tauriJsInterop;
        JsObjectRef = webviewWindowRef;
        _tauriEvent = tauriEvent;
    }


    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }

    /// <summary>
    /// The webview label. It is a unique identifier for the webview, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label => _tauriJsInterop.GetJsProperty<string>(JsObjectRef, "label");


    public async Task<UnlistenFn> Listen<TR>(string eventName, Func<TR, Task> callbackAsync)
    {
        var label = await Label;
        var eventOption = new EventOptions(EventTarget.WebviewWindow(label));
        return await _tauriEvent.Listen(eventName, callbackAsync, eventOption);
    }

    public async Task<UnlistenFn> Listen(string eventName, Func<Task> callbackAsync)
    {
        var label = await Label;
        var eventOption = new EventOptions(EventTarget.WebviewWindow(label));
        return await _tauriEvent.Listen(eventName, callbackAsync, eventOption);
    }

    public async Task<UnlistenFn> Once<TR>(string eventName, Func<TR, Task> callbackAsync)
    {
        var label = await Label;
        var eventOption = new EventOptions(EventTarget.WebviewWindow(label));
        return await _tauriEvent.Once(eventName, callbackAsync, eventOption);
    }

    public async Task<UnlistenFn> Once(string eventName, Func<Task> callbackAsync)
    {
        var label = await Label;
        var eventOption = new EventOptions(EventTarget.WebviewWindow(label));
        return await _tauriEvent.Once(eventName, callbackAsync, eventOption);
    }
}