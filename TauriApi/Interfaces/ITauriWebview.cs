using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

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
    
    #region Listen

    /// <summary>
    /// Listen to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Listen<TR>(string eventName, Func<TR, Task> callbackAsync);


    /// <summary>
    /// Listen to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Listen(string eventName, Func<Task> callbackAsync);

    #endregion

    #region Once

    /// <summary>
    /// Listen once to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Once<TR>(string eventName, Func<TR, Task> callbackAsync);

    /// <summary>
    /// Listen once to an emitted event on this webview.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Once(string eventName, Func<Task> callbackAsync);

    #endregion
}

/// <summary>
/// Webview Methods
/// </summary>
public static class WebviewExtensions
{
    /// <summary>
    /// Emits an event to all targets.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task Emit(this ITauriWebview webview, string eventName, object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emit", eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given target label.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="targetLabel">Label of the target Window/Webview/WebviewWindow</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo(this ITauriWebview webview, string targetLabel, string eventName,
        object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", targetLabel, eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="target">Raw <see cref="EventTarget"/> object.</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo<T>(this ITauriWebview webview, T target, string eventName, object? payload = null)
        where T : EventTarget
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", target, eventName, payload);
    }

    /// <summary>
    /// Set webview zoom level.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="scaleFactor"></param>
    public static async Task SetZoom(this ITauriWebview webview, double scaleFactor)
    {
        await webview.JsObjectRef.InvokeVoidAsync("setZoom", scaleFactor);
    }
}