using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi;

/// <summary>
/// Create new window or get a handle to an existing one.
/// Windows are identified by a label a unique identifier that can be used to reference it later.
/// It may only contain alphanumeric characters a-zA-Z plus the following special characters -, /, : and _.
/// </summary>
public class Window : ITauriWindow
{
    internal Window(IJSObjectReference windowRef, TauriJsInterop tauriJsInterop)
    {
        _tauriJsInterop = tauriJsInterop;
        JsObjectRef = windowRef;
    }

    private readonly TauriJsInterop _tauriJsInterop;

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }

    /// <summary>
    /// The window label. It is a unique identifier for the window, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label => _tauriJsInterop.GetJsProperty<string>(JsObjectRef, "label");
}

/// <summary>
/// Window Methods
/// </summary>
public static class WindowExtensions
{
    /// <summary>
    /// Centers the window.
    /// </summary>
    public static async Task Center(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("center");
    }

    /// <summary>
    /// Clear any applied effects if possible.
    /// </summary>
    public static async Task ClearEffects(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("clearEffects");
    }

    /// <summary>
    /// Closes the window.
    /// Note this emits a closeRequested event so you can intercept it. To force window close, use <see cref="Destroy"/>.
    /// </summary>
    public static async Task Close(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("close");
    }

    /// <summary>
    /// Destroys the window. Behaves like <see cref="Close"/> but forces the window close instead of emitting a closeRequested event.
    /// </summary>
    public static async Task Destroy(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("destroy");
    }

    /// <summary>
    /// Emits an event to all targets.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task Emit(this ITauriWindow window, string eventName, object? payload = null)
    {
        await window.JsObjectRef.InvokeVoidAsync("emit", eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given target label.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="targetLabel">Label of the target Window/Webview/WebviewWindow</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo(this ITauriWindow window, string targetLabel, string eventName,
        object? payload = null)
    {
        await window.JsObjectRef.InvokeVoidAsync("emitTo", targetLabel, eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="target">Raw <see cref="EventTarget"/> object.</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo<T>(this ITauriWindow window, T target, string eventName, object? payload = null)
        where T : EventTarget
    {
        await window.JsObjectRef.InvokeVoidAsync("emitTo", target, eventName, payload);
    }
}

#region Interfaces

public class Monitor
{
}

#pragma warning disable CS1591
public record WindowOptions
{
    public bool? AlwaysOnBottom { get; init; }


    public bool? AlwaysOnTop { get; init; }

    // backgroundColor
    // backgroundThrottling
    public bool? Center { get; init; }
    public bool? Closable { get; init; }
    public bool? ContentProtected { get; init; }
    public bool? Decorations { get; init; }
    public bool? Focus { get; init; }
    public bool? Fullscreen { get; init; }
    public int? Height { get; init; }
    public bool? HiddenTitle { get; init; }
}

#pragma warning restore CS1591

#endregion

#region Type Aliases

#pragma warning disable CS1591
public enum Theme
{
    Light,
    Dark,
    System
}
#pragma warning restore CS1591

#endregion