using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi;

/// <inheritdoc />
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

    /// <inheritdoc />
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


/// <summary>
/// Options for creating a window.
/// </summary>
public record WindowOptions
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AlwaysOnBottom { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AlwaysOnTop { get; init; }
    // backgroundColor
    // backgroundThrottling
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Center { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Closable { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContentProtected { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Decorations { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Focus { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Fullscreen { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Height { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HiddenTitle { get; init; }
}

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