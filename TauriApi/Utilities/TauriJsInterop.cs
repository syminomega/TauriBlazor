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

    #endregion

    #region Window

    public async Task<IJSObjectReference> ConstructWindow(string label, WindowOptions? options)
    {
        var module = await _moduleTask.Value;
        var appWindow = await module.InvokeAsync<IJSObjectReference>("constructWindow", label, options);
        return appWindow;
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