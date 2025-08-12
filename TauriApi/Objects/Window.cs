using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;
using TauriApi.Utilities.JsonConverters;

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

/// <summary>
/// Allows you to retrieve information about a given monitor.
/// </summary>
public class Monitor
{
    /// <summary>
    /// Human-readable name of the monitor
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The monitor's resolution.
    /// </summary>
    public PhysicalSize Size { get; set; } = new(0, 0);

    /// <summary>
    /// The Top-left corner position of the monitor relative to the larger full screen area.
    /// </summary>
    public PhysicalPosition Position { get; set; } = new(0, 0);

    /// <summary>
    /// The monitor's work area.
    /// </summary>
    public MonitorWorkArea WorkArea { get; set; } = new();

    /// <summary>
    /// The scale factor that can be used to map physical pixels to logical pixels.
    /// </summary>
    public double ScaleFactor { get; set; }

    /// <summary>
    /// The monitor's work area struct.
    /// </summary>
    public class MonitorWorkArea
    {
        /// <summary>
        /// The position of the work area.
        /// </summary>
        public PhysicalPosition Position { get; set; } = new(0, 0);

        /// <summary>
        /// The size of the work area.
        /// </summary>
        public PhysicalSize Size { get; set; } = new(0, 0);
    }
}

/// <summary>
/// Options for creating a window.
/// </summary>
public record WindowOptions
{
    /// <summary>
    /// Show window in the center of the screen.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Center { get; init; }

    /// <summary>
    /// The initial vertical position. Only applies if <c>y</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? X { get; init; }

    /// <summary>
    /// The initial horizontal position. Only applies if <c>x</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Y { get; init; }

    /// <summary>
    /// The initial width.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Width { get; init; }

    /// <summary>
    /// The initial height.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Height { get; init; }

    /// <summary>
    /// The minimum width. Only applies if <c>minHeight</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinWidth { get; init; }

    /// <summary>
    /// The minimum height. Only applies if <c>minWidth</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinHeight { get; init; }

    /// <summary>
    /// The maximum width. Only applies if <c>maxHeight</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxWidth { get; init; }

    /// <summary>
    /// The maximum height. Only applies if <c>maxWidth</c> is also set.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxHeight { get; init; }

    /// <summary>
    /// Prevent the window from overflowing the working area (e.g. monitor size - taskbar size) on creation.
    /// Can either be set to <c>true</c> or to a PreventOverflowMargin object to set an additional margin.
    /// NOTE: The overflow check is only performed on window creation, resizes can still overflow.
    /// Platform-specific: iOS / Android unsupported.
    /// </summary>
    // TODO: preventOverflow

    /// <summary>
    /// Whether the window is resizable or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Resizable { get; init; }

    /// <summary>
    /// Window title.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }

    /// <summary>
    /// Whether the window is in fullscreen mode or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Fullscreen { get; init; }

    /// <summary>
    /// Whether the window will be initially focused or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Focus { get; init; }

    /// <summary>
    /// Whether the window is transparent or not.
    /// Note that on macOS this requires the macos-private-api feature flag, enabled under tauri.conf.json > app > macOSPrivateApi.
    /// WARNING: Using private APIs on macOS prevents your application from being accepted to the App Store.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Transparent { get; init; }

    /// <summary>
    /// Whether the window should be maximized upon creation or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Maximized { get; init; }

    /// <summary>
    /// Whether the window should be immediately visible upon creation or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Visible { get; init; }

    /// <summary>
    /// Whether the window should have borders and bars or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Decorations { get; init; }

    /// <summary>
    /// Whether the window should always be on top of other windows or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AlwaysOnTop { get; init; }

    /// <summary>
    /// Whether the window should always be below other windows.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AlwaysOnBottom { get; init; }

    /// <summary>
    /// Prevents the window contents from being captured by other apps.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContentProtected { get; init; }

    /// <summary>
    /// Whether the window icon should be added to the taskbar.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SkipTaskbar { get; init; }

    /// <summary>
    /// Whether the window has shadow.
    /// Platform-specific: Windows, Linux unsupported.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Shadow { get; init; }

    /// <summary>
    /// The initial window theme. Defaults to the system theme. Only implemented on Windows and macOS 10.14+.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Theme Theme { get; init; }

    /// <summary>
    /// The style of the macOS title bar.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TitleBarStyle? TitleBarStyle { get; init; }

    /// <summary>
    /// The position of the window controls on macOS. Requires titleBarStyle: 'overlay' and decorations: true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LogicalPosition? TrafficLightPosition { get; init; }

    /// <summary>
    /// If true, sets the window title to be hidden on macOS.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HiddenTitle { get; init; }

    /// <summary>
    /// Defines the window tabbing identifier on macOS. Windows with the same tabbing identifier will be grouped together.
    /// If the tabbing identifier is not set, automatic tabbing will be disabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TabbingIdentifier { get; init; }

    /// <summary>
    /// Whether the window's native maximize button is enabled or not. Defaults to true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Maximizable { get; init; }

    /// <summary>
    /// Whether the window's native minimize button is enabled or not. Defaults to true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Minimizable { get; init; }

    /// <summary>
    /// Whether the window's native close button is enabled or not. Defaults to true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Closable { get; init; }

    /// <summary>
    /// Sets a parent to the window to be created. Can be either a Window or a label of the window.
    /// Platform-specific: Windows, Linux, macOS.
    /// </summary>
    // TODO: parent

    /// <summary>
    /// Whether the window should be visible on all workspaces or virtual desktops.
    /// Platform-specific: Windows / iOS / Android unsupported.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? VisibleOnAllWorkspaces { get; init; }

    /// <summary>
    /// Window effects. Requires the window to be transparent. Platform-specific: Windows, Linux unsupported.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WindowEffects? WindowEffects { get; init; }

    /// <summary>
    /// Set the window background color. Platform-specific: Android / iOS unsupported. Windows: alpha channel is ignored.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Color? BackgroundColor { get; init; }

    /// <summary>
    /// Change the default background throttling behaviour. Platform-specific: Linux / Windows / Android unsupported. iOS/macOS supported since v17/v14.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BackgroundThrottlingPolicy? BackgroundThrottling { get; init; }

    /// <summary>
    /// Whether we should disable JavaScript code execution on the webview or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? JavascriptDisabled { get; init; }

    /// <summary>
    /// On macOS and iOS there is a link preview on long pressing links, this is enabled by default.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowLinkPreview { get; init; }

    /// <summary>
    /// Allows disabling the input accessory view on iOS. The accessory view is the view that appears above the keyboard when a text input element is focused.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableInputAccessoryView { get; init; }
}

/// <summary>
/// The window effects configuration object.
/// </summary>
public class WindowEffects
{
    /// <summary>
    /// List of Window effects to apply to the Window.
    /// Conflicting effects will apply the first one and ignore the rest.
    /// </summary>
    public List<Effect> Effects { get; init; } = new();

    /// <summary>
    /// Window effect state (macOS Only).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EffectState? State { get; set; }

    /// <summary>
    /// Window effect corner radius (macOS Only).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Radius { get; set; }

    /// <summary>
    /// Window effect color. Affects <see cref="Effect.Blur"/> and <see cref="Effect.Acrylic"/> only
    /// on Windows 10 v1903+. Doesn't have any effect on Windows 7 or Windows 11.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Color? Color { get; set; }
}

#endregion

#pragma warning disable CS1591

#region Enums

[JsonConverter(typeof(CustomEnumValueConverter<BackgroundThrottlingPolicy>))]
public enum BackgroundThrottlingPolicy
{
    [CustomEnumValue("disabled")] Disabled,
    [CustomEnumValue("throttle")] Throttle,
    [CustomEnumValue("suspend")] Suspend
}

/// <summary>
/// Platform-specific window effects
/// </summary>
[JsonConverter(typeof(CustomEnumValueConverter<Effect>))]
public enum Effect
{
    /// <summary>
    /// A default material appropriate for the view's effectiveAppearance. macOS 10.14-
    /// </summary>
    [Obsolete("Deprecated since macOS 10.14. You should instead choose an appropriate semantic material.")]
    [CustomEnumValue("appearanceBased")] AppearanceBased,
    
    [Obsolete("Deprecated since macOS 10.14. Use a semantic material instead.")]
    [CustomEnumValue("light")] Light,
    
    [Obsolete("Deprecated since macOS 10.14. Use a semantic material instead.")]
    [CustomEnumValue("dark")] Dark,
    
    [Obsolete("Deprecated since macOS 10.14. Use a semantic material instead.")]
    [CustomEnumValue("mediumLight")] MediumLight,
    
    [Obsolete("Deprecated since macOS 10.14. Use a semantic material instead.")]
    [CustomEnumValue("ultraDark")] UltraDark,
    
    /// <summary>
    /// macOS 10.10+
    /// </summary>
    [CustomEnumValue("titlebar")] Titlebar,
    
    /// <summary>
    /// macOS 10.10+
    /// </summary>
    [CustomEnumValue("selection")] Selection,
    
    /// <summary>
    /// macOS 10.11+
    /// </summary>
    [CustomEnumValue("menu")] Menu,
    
    /// <summary>
    /// macOS 10.11+
    /// </summary>
    [CustomEnumValue("popover")] Popover,
    
    /// <summary>
    /// macOS 10.11+
    /// </summary>
    [CustomEnumValue("sidebar")] Sidebar,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("headerView")] HeaderView,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("sheet")] Sheet,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("windowBackground")] WindowBackground,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("hudWindow")] HudWindow,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("fullScreenUI")] FullScreenUi,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("tooltip")] Tooltip,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("contentBackground")] ContentBackground,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("underWindowBackground")] UnderWindowBackground,
    
    /// <summary>
    /// macOS 10.14+
    /// </summary>
    [CustomEnumValue("underPageBackground")] UnderPageBackground,
    
    /// <summary>
    /// Windows 11 Only
    /// </summary>
    [CustomEnumValue("mica")] Mica,
    
    /// <summary>
    /// Windows 7/10/11(22H1) Only
    /// </summary>
    /// <remarks>This effect has bad performance when resizing/dragging the window on Windows 11 build 22621.</remarks>
    [CustomEnumValue("blur")] Blur,
    
    /// <summary>
    /// Windows 10/11
    /// </summary>
    /// <remarks>This effect has bad performance when resizing/dragging the window on Windows 10 v1903+ and Windows 11 build 22000.</remarks>
    [CustomEnumValue("acrylic")] Acrylic,
    
    /// <summary>
    /// Tabbed effect that matches the system dark preference Windows 11 Only
    /// </summary>
    [CustomEnumValue("tabbed")] Tabbed,
    
    /// <summary>
    /// Tabbed effect with dark mode but only if dark mode is enabled on the system Windows 11 Only
    /// </summary>
    [CustomEnumValue("tabbedDark")] TabbedDark,
    
    /// <summary>
    /// Tabbed effect with light mode Windows 11 Only
    /// </summary>
    [CustomEnumValue("tabbedLight")] TabbedLight
}

/// <summary>
/// Window effect state macOS only
/// </summary>
/// <remarks>See https://developer.apple.com/documentation/appkit/nsvisualeffectview/state</remarks>
[JsonConverter(typeof(CustomEnumValueConverter<EffectState>))]
public enum EffectState
{
    /// <summary>
    /// Make window effect state follow the window's active state macOS only
    /// </summary>
    [CustomEnumValue("followsWindowActiveState")] FollowsWindowActiveState,
    
    /// <summary>
    /// Make window effect state always active macOS only
    /// </summary>
    [CustomEnumValue("active")] Active,
    
    /// <summary>
    /// Make window effect state always inactive macOS only
    /// </summary>
    [CustomEnumValue("inactive")] Inactive
}
#endregion

#region Type Aliases

[JsonConverter(typeof(CustomEnumValueConverter<Theme>))]
public enum Theme
{
    [CustomEnumValue("light")] Light,
    [CustomEnumValue("dark")] Dark,
}

[JsonConverter(typeof(CustomEnumValueConverter<TitleBarStyle>))]
public enum TitleBarStyle
{
    [CustomEnumValue("visible")] Visible,
    [CustomEnumValue("transparent")] Transparent,
    [CustomEnumValue("overlay")] Overlay,
}

#endregion

#pragma warning restore CS1591