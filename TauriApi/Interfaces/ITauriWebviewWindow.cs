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
    
    
    #region Listen

    /// <summary>
    /// Listen to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new async Task<UnlistenFn> Listen<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new async Task<UnlistenFn> Listen(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new Task<UnlistenFn> Listen<TR>(string eventName, Func<TR, Task> callbackAsync);


    /// <summary>
    /// Listen to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new Task<UnlistenFn> Listen(string eventName, Func<Task> callbackAsync);

    #endregion

    #region Once

    /// <summary>
    /// Listen once to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new async Task<UnlistenFn> Once<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new async Task<UnlistenFn> Once(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new Task<UnlistenFn> Once<TR>(string eventName, Func<TR, Task> callbackAsync);

    /// <summary>
    /// Listen once to an emitted event on this webview window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public new Task<UnlistenFn> Once(string eventName, Func<Task> callbackAsync);

    #endregion
}