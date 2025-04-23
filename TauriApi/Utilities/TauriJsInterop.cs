using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

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

    #region Event

    public async Task Listen(string eventName)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("addListenBind", eventName);
        //在event类中添加监听
    }

    public async Task Unlisten(string eventName)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("removeListenBind", eventName);
        //在event类中移除绑定
    }

    #endregion

    public async Task<IJSObjectReference> ConstructWindow(string label, WindowOptions? options)
    {
        var module = await _moduleTask.Value;
        var appWindow = await module.InvokeAsync<IJSObjectReference>("constructWindow", label, options);
        return appWindow;
    }


    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}