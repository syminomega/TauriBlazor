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
public abstract class EventTarget
{
    /// <summary>
    /// Event target kind.
    /// </summary>
    public string Kind { get; }

    private EventTarget(string kind)
    {
        Kind = kind;
    }

    /// <inheritdoc />
    public sealed class Any : EventTarget
    {
        /// <inheritdoc />
        public Any() : base("Any")
        {
        }
    }

    /// <inheritdoc />
    public sealed class AnyLabel : EventTarget
    {
        /// <summary>
        /// Label for the event target.
        /// </summary>
        public string Label { get; }

        /// <inheritdoc />
        public AnyLabel(string label) : base("AnyLabel")
        {
            Label = label;
        }

        /// <summary>
        /// Implicit conversion from string to AnyLabel.
        /// </summary>
        /// <param name="label">target label</param>
        /// <returns></returns>
        public static implicit operator AnyLabel(string label)
        {
            return new AnyLabel(label);
        }
    }

    /// <inheritdoc />
    public sealed class App : EventTarget
    {
        /// <inheritdoc />
        public App() : base("App")
        {
        }
    }

    /// <inheritdoc />
    public sealed class Window : EventTarget
    {
        /// <summary>
        /// Label for the event target.
        /// </summary>
        public string Label { get; }

        /// <inheritdoc />
        public Window(string label) : base("Window")
        {
            Label = label;
        }
    }

    /// <inheritdoc />
    public sealed class Webview : EventTarget
    {
        /// <summary>
        /// Label for the event target.
        /// </summary>
        public string Label { get; }

        /// <inheritdoc />
        public Webview(string label) : base("Webview")
        {
            Label = label;
        }
    }

    /// <inheritdoc />
    public sealed class WebviewWindow : EventTarget
    {
        /// <summary>
        /// Label for the event target.
        /// </summary>
        public string Label { get; }

        /// <inheritdoc />
        public WebviewWindow(string label) : base("WebviewWindow")
        {
            Label = label;
        }
    }
}

/// <summary>
/// Unlisten event delegate.
/// </summary>
public delegate ValueTask UnlistenFn();

#endregion