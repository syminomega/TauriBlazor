using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

/// <summary>
/// The event system allows you to emit events to the backend and listen to events from it.
/// </summary>
public class TauriEventModule
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.event";
    private readonly TauriJsInterop _tauriJsInterop;

    /// <summary>
    /// Inject TauriEvent.
    /// </summary>
    public TauriEventModule(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    /// <summary>
    /// Emits an event to all targets.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public async Task Emit(string eventName, object? payload = null)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.emit", eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given target label.
    /// </summary>
    /// <param name="targetLabel">Label of the target Window/Webview/WebviewWindow</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public async Task EmitTo(string targetLabel, string eventName, object? payload = null)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.emitTo", targetLabel, eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="target">Raw <see cref="EventTarget"/> object.</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public async Task EmitTo<T>(T target, string eventName, object? payload = null)
        where T : EventTarget
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.emitTo", target, eventName, payload);
    }

    #region Listen

    /// <summary>
    /// Listen to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync,
        EventOptions? options = null)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync, options);
    }

    /// <summary>
    /// Listen to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen(TauriEventName eventName, Func<Task> callbackAsync,
        EventOptions? options = null)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync, options);
    }

    /// <summary>
    /// Listen to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen<TR>(string eventName, Func<TR, Task> callbackAsync,
        EventOptions? options = null)
    {
        var eventHandler = new TauriEventHandler<TR>(callbackAsync, once: false);
        var jsHandler = await _tauriJsInterop.ListenEvent(eventName,
            DotNetObjectReference.Create<ITauriEventHandler>(eventHandler), options);
        eventHandler.HandlerRef = jsHandler;
        return eventHandler.Unlisten;
    }

    /// <summary>
    /// Listen to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen(string eventName, Func<Task> callbackAsync, EventOptions? options = null)
    {
        var eventHandler = new TauriEventHandler(callbackAsync, once: false);
        var jsHandler = await _tauriJsInterop.ListenEvent(eventName,
            DotNetObjectReference.Create<ITauriEventHandler>(eventHandler), options);
        eventHandler.HandlerRef = jsHandler;
        return eventHandler.Unlisten;
    }

    #endregion

    #region Once

    /// <summary>
    /// Listen once to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync,
        EventOptions? options = null)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync, options);
    }

    /// <summary>
    /// Listen once to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once(TauriEventName eventName, Func<Task> callbackAsync, EventOptions? options = null)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync, options);
    }

    /// <summary>
    /// Listen once to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once<TR>(string eventName, Func<TR, Task> callbackAsync, EventOptions? options = null)
    {
        var eventHandler = new TauriEventHandler<TR>(callbackAsync, once: true);
        var jsHandler = await _tauriJsInterop.OnceEvent(eventName,
            DotNetObjectReference.Create<ITauriEventHandler>(eventHandler), options);
        eventHandler.HandlerRef = jsHandler;
        return eventHandler.Unlisten;
    }

    /// <summary>
    /// Listen once to an emitted event to any <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <param name="options">Event listening options.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once(string eventName, Func<Task> callbackAsync, EventOptions? options = null)
    {
        var eventHandler = new TauriEventHandler(callbackAsync, once: true);
        var jsHandler = await _tauriJsInterop.OnceEvent(eventName,
            DotNetObjectReference.Create<ITauriEventHandler>(eventHandler), options);
        eventHandler.HandlerRef = jsHandler;
        return eventHandler.Unlisten;
    }

    #endregion
}