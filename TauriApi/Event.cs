using Microsoft.JSInterop;

namespace TauriApi;

public class Event
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.event";
    private readonly TauriJsInterop _tauriJsInterop;

    public Event(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    private static Dictionary<string, Delegate> EventHandlers { get; } = new();

    [JSInvokable("TauriEventCallback")]
    public static Task TauriEventCallback(string eventName, object? data)
    {
        if (EventHandlers.TryGetValue(eventName, out var handler))
        {
            handler.DynamicInvoke(data);
        }
        else
        {
            throw new NullReferenceException($"No handler for event: {eventName}.");
        }

        return Task.CompletedTask;
    }

    public async Task Listen<T>(string eventName, Action<T> handler)
    {
        if (EventHandlers.TryAdd(eventName, handler))
        {
            await _tauriJsInterop.Listen(eventName);
        }
    }

    public async Task Unlisten(string eventName)
    {
        EventHandlers.Remove(eventName);
        await _tauriJsInterop.Unlisten(eventName);
    }
}