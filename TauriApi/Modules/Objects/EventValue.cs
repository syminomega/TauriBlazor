using System.Reflection;
using System.Text.Json.Serialization;
using TauriApi.Utilities;

namespace TauriApi;

/// <summary>
/// Tauri build-in events.
/// </summary>
public enum TauriEventName
{
#pragma warning disable CS1591
    [TauriEventName("tauri://drag-drop")] DragDrop,
    [TauriEventName("tauri://drag-enter")] DragEnter,
    [TauriEventName("tauri://drag-leave")] DragLeave,
    [TauriEventName("tauri://drag-over")] DragOver,

    [TauriEventName("tauri://webview-created")]
    WebviewCreated,
    [TauriEventName("tauri://blur")] WindowBlur,

    [TauriEventName("tauri://close-requested")]
    WindowCloseRequested,

    [TauriEventName("tauri://window-created")]
    WindowCreated,
    [TauriEventName("tauri://destroyed")] WindowDestroyed,
    [TauriEventName("tauri://focus")] WindowFocus,
    [TauriEventName("tauri://move")] WindowMove,
    [TauriEventName("tauri://resize")] WindowResize,

    [TauriEventName("tauri://scale-change")]
    WindowScaleFactorChange,

    [TauriEventName("tauri://theme-changed")]
    WindowThemeChanged,
#pragma warning restore CS1591
}

/// <summary>
/// Extension methods for TauriEvent enum.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get the Tauri event name from the enum value.
    /// </summary>
    /// <param name="tauriEventName"></param>
    /// <returns></returns>
    public static string GetTauriEventName(this TauriEventName tauriEventName)
    {
        // Get the type of the enum
        var type = tauriEventName.GetType();
        // Get the member info for the enum value
        var memberInfo = type.GetMember(tauriEventName.ToString());

        // Get TauriEventNameAttribute
        var attribute = memberInfo[0].GetCustomAttribute<TauriEventNameAttribute>();
        return attribute!.EventName;
    }
}

#region Interfaces

/// <summary>
/// Listen event options.
/// </summary>
public class EventOptions
{
    /// <summary>
    /// The event target to listen to, see <see cref="EventTarget"/>.
    /// If a string is provided, EventTarget.AnyLabel is used.
    /// </summary>
    public EventTarget? Target { get; }

    /// <summary>
    /// Create an event options object with event target.
    /// </summary>
    /// <param name="target"></param>
    public EventOptions(EventTarget target)
    {
        Target = target;
    }
}

#endregion

#region Type Alias

/// <summary>
/// Type alias for the event target.
/// </summary>
public class EventTarget
{
    /// <summary>
    /// Event target kind.
    /// </summary>
    public string Kind { get; }

    /// <summary>
    /// Label for the event target, if applicable.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; }

    private EventTarget(string kind, string? label)
    {
        Kind = kind;
        Label = label;
    }
    
    /// <summary>
    /// App event target, which is the main application.
    /// </summary>
    public static EventTarget App()
    {
        return new EventTarget("App", null);
    }
    /// <summary>
    /// Any event target.
    /// </summary>
    public static EventTarget Any()
    {
        return new EventTarget("Any", null);
    }

    /// <summary>
    /// Any event target with a specific label.
    /// </summary>
    public static EventTarget AnyLabel(string label)
    {
        return new EventTarget("AnyLabel", label);
    }
    
    /// <summary>
    /// Window with a specific label.
    /// </summary>
    public static EventTarget Window(string label)
    {
        return new EventTarget("Window", label);
    }
    
    /// <summary>
    /// Webview with a specific label.
    /// </summary>
    public static EventTarget Webview(string label)
    {
        return new EventTarget("Webview", label);
    }
    
    /// <summary>
    /// Webview Window with a specific label.
    /// </summary>
    public static EventTarget WebviewWindow(string label)
    {
        return new EventTarget("WebviewWindow", label);
    }
    
    /// <summary>
    /// Any event target with a specific label.
    /// </summary>
    public static implicit operator EventTarget(string label)
    {
        return new EventTarget("AnyLabel", label);
    }
}

/// <summary>
/// Unlisten event delegate.
/// </summary>
public delegate ValueTask UnlistenFn();

#endregion