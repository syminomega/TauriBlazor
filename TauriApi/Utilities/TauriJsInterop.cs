using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

#pragma warning disable CS1591

namespace TauriApi.Utilities;

public class TauriJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly ILogger<TauriJsInterop> _logger;

    public TauriJsInterop(IJSRuntime jsRuntime, ILogger<TauriJsInterop> logger)
    {
        _moduleTask = new(jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/SyminStudio.TauriApi/js/tauri-api.js").AsTask);
        _logger = logger;
    }

    public async ValueTask<T> GetJsProperty<T>(IJSObjectReference obj, string propPath)
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<T>("getPropertyInObject", obj, propPath);
    }

    #region Event

    public async Task<IJSObjectReference> ListenEvent(
        string eventName, DotNetObjectReference<ITauriEventHandler> eventHandler, EventOptions? options)
    {
        var module = await _moduleTask.Value;
        var eventRef = await module.InvokeAsync<IJSObjectReference>("listenEvent", eventName, eventHandler, options);
        return eventRef;
    }

    public async Task<IJSObjectReference> OnceEvent(
        string eventName, DotNetObjectReference<ITauriEventHandler> eventHandler, EventOptions? options)
    {
        var module = await _moduleTask.Value;
        var eventRef = await module.InvokeAsync<IJSObjectReference>("onceEvent", eventName, eventHandler, options);
        return eventRef;
    }

    #endregion

    #region Window

    public async Task<IJSObjectReference> ConstructWindow(string label, WindowOptions? options)
    {
        var module = await _moduleTask.Value;
        var appWindow = await module.InvokeAsync<IJSObjectReference>("constructWindow", label, options);
        return appWindow;
    }

    #endregion

    #region Webview

    public async Task<IJSObjectReference> ConstructWebview(string windowLabel, string label,
        WebviewStandaloneOptions options)
    {
        var module = await _moduleTask.Value;
        var appWebview = await module.InvokeAsync<IJSObjectReference>("constructWebview", windowLabel, label, options);
        return appWebview;
    }

    #endregion

    #region WebviewWindow

    public async Task<IJSObjectReference> ConstructWebviewWindow(string label, WindowOptions? windowOptions,
        WebviewOptions? webviewOptions)
    {
        var module = await _moduleTask.Value;
        var webviewWindow =
            await module.InvokeAsync<IJSObjectReference>("constructWebviewWindow", label, windowOptions,
                webviewOptions);
        return webviewWindow;
    }

    #endregion

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}