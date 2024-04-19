using Microsoft.JSInterop;

namespace TauriApi;

public class TauriJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public TauriJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/SyminStudio.TauriApi/js/tauri-api.js").AsTask);
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

    #region Window

    public async Task<IJSObjectReference> GetAppWindow()
    {
        var module = await _moduleTask.Value;
        var appWindow = await module.InvokeAsync<IJSObjectReference>("getAppWindow");
        return appWindow;
    }

    public async Task<string> GetAppWindowLabel(IJSObjectReference appWindow)
    {
        var module = await _moduleTask.Value;
        var label = await module.InvokeAsync<string>("getWebviewWindowLabel", appWindow);
        return label;
    }

    public async Task WindowListen(IJSObjectReference appWindow, string eventName)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("addWindowListenBind", appWindow, eventName);
        //在window类中添加监听
    }

    public async Task WindowUnlisten(IJSObjectReference appWindow, string eventName)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("removeWindowListenBind", appWindow, eventName);
        //在window类中移除绑定
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