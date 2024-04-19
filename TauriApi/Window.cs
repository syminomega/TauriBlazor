using System.Text.Json;
using Microsoft.JSInterop;

namespace TauriApi;

public class Window
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.window";
    private readonly TauriJsInterop _tauriJsInterop;

    public Window(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;

        AppWindow = new Lazy<Task<WebviewWindow>>(async () =>
        {
            var appWindow = await _tauriJsInterop.GetAppWindow();
            var windowLabel = await _tauriJsInterop.GetAppWindowLabel(appWindow);
            return new WebviewWindow(appWindow, _jsRuntime, _tauriJsInterop, windowLabel);
        });
    }

    public Lazy<Task<WebviewWindow>> AppWindow { get; set; }
}

public class WebviewWindow : IAsyncDisposable
{
    private readonly IJSObjectReference _appWindowRef;
    private readonly IJSRuntime _jsRuntime;
    private readonly TauriJsInterop _tauriJsInterop;

    public string Label { get; init; }

    internal WebviewWindow(
        IJSObjectReference appWindowRef,
        IJSRuntime jsRuntime,
        TauriJsInterop tauriJsInterop,
        string label)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
        _appWindowRef = appWindowRef;
        Label = label;
    }

    private static Dictionary<string, Delegate> EventHandlers { get; } = new();

    [JSInvokable("WindowEventCallback")]
    public static Task WindowEventCallback(string windowLabel, string eventName, object? data)
    {
        if (EventHandlers.TryGetValue($"{windowLabel}_{eventName}", out var handler))
        {
            //判断data是不是JsonElement类型
            if (data is JsonElement jsonElement)
            {
                //将JsonElement转换为对应的类型
                data = JsonSerializer.Deserialize(jsonElement.GetRawText(),
                    handler.Method.GetParameters()[0].ParameterType);
            }

            //判断data是否能转换成代理参数的类型
            handler.DynamicInvoke(data);
        }
        else
        {
            throw new NullReferenceException($"No handler for event: {windowLabel}_{eventName}.");
        }

        return Task.CompletedTask;
    }

    public async Task Listen<T>(string eventName, Action<T> handler)
    {
        //将label加入到事件名中，避免事件名冲突
        if (EventHandlers.TryAdd($"{Label}_{eventName}", handler))
        {
            await _tauriJsInterop.WindowListen(_appWindowRef, eventName);
        }
    }

    public async Task Unlisten(string eventName)
    {
        EventHandlers.Remove($"{Label}_{eventName}");
        await _tauriJsInterop.WindowUnlisten(_appWindowRef, eventName);
    }

    public async ValueTask DisposeAsync()
    {
        await _appWindowRef.DisposeAsync();
    }
}