using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

/// <summary>
/// Create new window or get a handle to an existing one.
/// Windows are identified by a label a unique identifier that can be used to reference it later.
/// It may only contain alphanumeric characters a-zA-Z plus the following special characters -, /, : and _.
/// </summary>
public interface ITauriWindow : ITauriObject
{
    /// <summary>
    /// The window label. It is a unique identifier for the window, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label { get; }

    // TODO: listeners

    #region Listen

    /// <summary>
    /// Listen to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Listen(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Listen(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Listen<TR>(string eventName, Func<TR, Task> callbackAsync);


    /// <summary>
    /// Listen to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Listen(string eventName, Func<Task> callbackAsync);

    #endregion

    #region Once

    /// <summary>
    /// Listen once to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once<TR>(TauriEventName eventName, Func<TR, Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Tauri build-in event.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public async Task<UnlistenFn> Once(TauriEventName eventName, Func<Task> callbackAsync)
    {
        var eventNameString = eventName.GetTauriEventName();
        return await Once(eventNameString, callbackAsync);
    }

    /// <summary>
    /// Listen once to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <typeparam name="TR">Callback value</typeparam>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Once<TR>(string eventName, Func<TR, Task> callbackAsync);

    /// <summary>
    /// Listen once to an emitted event on this window.
    /// </summary>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="callbackAsync">Event handler callback.</param>
    /// <returns>A function to unlisten to the event. Note that removing the listener is required if your listener goes out of scope e.g. the component is disposed.</returns>
    public Task<UnlistenFn> Once(string eventName, Func<Task> callbackAsync);

    #endregion
}

/// <summary>
/// Window Methods
/// </summary>
public static class WindowExtensions
{
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

    /// <summary>
    /// The scale factor that can be used to map physical pixels to logical pixels.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's monitor scale factor.</returns>
    public static async Task<double> ScaleFactor(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<double>("scaleFactor");
    }

    /// <summary>
    /// The position of the top-left hand corner of the window's client area relative to the top-left hand corner of the desktop.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's inner position.</returns>
    public static async Task<PhysicalPosition> InnerPosition(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<PhysicalPosition>("innerPosition");
    }

    /// <summary>
    /// The position of the top-left hand corner of the window relative to the top-left hand corner of the desktop.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's outer position.</returns>
    public static async Task<PhysicalPosition> OuterPosition(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<PhysicalPosition>("outerPosition");
    }

    /// <summary>
    /// The physical size of the window's client area.
    /// The client area is the content of the window, excluding the title bar and borders.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's inner size.</returns>
    public static async Task<PhysicalSize> InnerSize(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<PhysicalSize>("innerSize");
    }

    /// <summary>
    /// The physical size of the entire window.
    /// These dimensions include the title bar and borders. If you don't want that (and you usually don't), use inner_size instead.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's outer size.</returns>
    public static async Task<PhysicalSize> OuterSize(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<PhysicalSize>("outerSize");
    }

    /// <summary>
    /// Gets the window's current fullscreen state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is in fullscreen mode or not.</returns>
    public static async Task<bool> IsFullscreen(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isFullscreen");
    }

    /// <summary>
    /// Gets the window's current minimized state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is minimized or not.</returns>
    public static async Task<bool> IsMinimized(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isMinimized");
    }

    /// <summary>
    /// Gets the window's current maximized state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is maximized or not.</returns>
    public static async Task<bool> IsMaximized(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isMaximized");
    }

    /// <summary>
    /// Gets the window's current focus state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is focused or not.</returns>
    public static async Task<bool> IsFocused(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isFocused");
    }

    /// <summary>
    /// Gets the window's current decorated state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is decorated or not.</returns>
    public static async Task<bool> IsDecorated(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isDecorated");
    }

    /// <summary>
    /// Gets the window's current resizable state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is resizable or not.</returns>
    public static async Task<bool> IsResizable(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isResizable");
    }

    /// <summary>
    /// Gets the window's native maximize button state.
    /// Platform-specific: Linux / iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window's native maximize button is enabled or not.</returns>
    public static async Task<bool> IsMaximizable(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isMaximizable");
    }

    /// <summary>
    /// Gets the window's native minimize button state.
    /// Platform-specific: Linux / iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window's native minimize button is enabled or not.</returns>
    public static async Task<bool> IsMinimizable(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isMinimizable");
    }

    /// <summary>
    /// Gets the window's native close button state.
    /// Platform-specific: iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window's native close button is enabled or not.</returns>
    public static async Task<bool> IsClosable(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isClosable");
    }

    /// <summary>
    /// Gets the window's current visible state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is visible or not.</returns>
    public static async Task<bool> IsVisible(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isVisible");
    }

    /// <summary>
    /// Gets the window's current title.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window's title.</returns>
    public static async Task<string> Title(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<string>("title");
    }

    /// <summary>
    /// Gets the window's current theme.
    /// Platform-specific: macOS: Theme was introduced on macOS 10.14. Returns `light` on macOS 10.13 and below.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>The window theme.</returns>
    public static async Task<Theme?> Theme(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<Theme?>("theme");
    }

    /// <summary>
    /// Whether the window is configured to be always on top of other windows or not.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is always on top or not.</returns>
    public static async Task<bool> IsAlwaysOnTop(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isAlwaysOnTop");
    }

    /// <summary>
    /// Centers the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Center(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("center");
    }

    /// <summary>
    /// Requests user attention to the window, this has no effect if the application
    /// is already focused. How requesting for user attention manifests is platform dependent,
    /// see `UserAttentionType` for details.
    /// Providing `null` will unset the request for user attention. Unsetting the request for
    /// user attention might not be done automatically by the WM when the window receives input.
    /// Platform-specific: macOS: `null` has no effect. Linux: Urgency levels have the same effect.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="requestType">The type of user attention request.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task RequestUserAttention(this ITauriWindow window, UserAttentionType? requestType)
    {
        await window.JsObjectRef.InvokeVoidAsync("requestUserAttention", requestType);
    }

    /// <summary>
    /// Updates the window resizable flag.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="resizable">Whether the window should be resizable.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetResizable(this ITauriWindow window, bool resizable)
    {
        await window.JsObjectRef.InvokeVoidAsync("setResizable", resizable);
    }

    /// <summary>
    /// Enable or disable the window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="enabled">Whether the window should be enabled.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetEnabled(this ITauriWindow window, bool enabled)
    {
        await window.JsObjectRef.InvokeVoidAsync("setEnabled", enabled);
    }

    /// <summary>
    /// Whether the window is enabled or disabled.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>Whether the window is enabled or not.</returns>
    public static async Task<bool> IsEnabled(this ITauriWindow window)
    {
        return await window.JsObjectRef.InvokeAsync<bool>("isEnabled");
    }

    /// <summary>
    /// Sets whether the window's native maximize button is enabled or not.
    /// If resizable is set to false, this setting is ignored.
    /// Platform-specific: macOS: Disables the "zoom" button in the window titlebar, which is also used to enter fullscreen mode. Linux / iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="maximizable">Whether the window's maximize button should be enabled.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetMaximizable(this ITauriWindow window, bool maximizable)
    {
        await window.JsObjectRef.InvokeVoidAsync("setMaximizable", maximizable);
    }

    /// <summary>
    /// Sets whether the window's native minimize button is enabled or not.
    /// Platform-specific: Linux / iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="minimizable">Whether the window's minimize button should be enabled.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetMinimizable(this ITauriWindow window, bool minimizable)
    {
        await window.JsObjectRef.InvokeVoidAsync("setMinimizable", minimizable);
    }

    /// <summary>
    /// Sets whether the window's native close button is enabled or not.
    /// Platform-specific: Linux: GTK+ will do its best to convince the window manager not to show a close button. Depending on the system, this function may not have any effect when called on a window that is already visible. iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="closable">Whether the window's close button should be enabled.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetClosable(this ITauriWindow window, bool closable)
    {
        await window.JsObjectRef.InvokeVoidAsync("setClosable", closable);
    }

    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="title">The new title</param>
    public static async Task SetTitle(this ITauriWindow window, string title)
    {
        await window.JsObjectRef.InvokeVoidAsync("setTitle", title);
    }

    /// <summary>
    /// Maximizes the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Maximize(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("maximize");
    }

    /// <summary>
    /// Unmaximizes the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Unmaximize(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("unmaximize");
    }

    /// <summary>
    /// Toggles the window maximized state.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task ToggleMaximize(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("toggleMaximize");
    }

    /// <summary>
    /// Minimizes the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Minimize(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("minimize");
    }

    /// <summary>
    /// Unminimizes the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Unminimize(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("unminimize");
    }

    /// <summary>
    /// Sets the window visibility to true.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Show(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("show");
    }

    /// <summary>
    /// Sets the window visibility to false.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Hide(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("hide");
    }

    /// <summary>
    /// Closes the window.
    /// Note this emits a closeRequested event so you can intercept it. To force window close, use <see cref="Destroy"/>.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task Close(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("close");
    }

    /// <summary>
    /// Whether the window should have borders and bars.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="decorations">Whether the window should have borders and bars.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetDecorations(this ITauriWindow window, bool decorations)
    {
        await window.JsObjectRef.InvokeVoidAsync("setDecorations", decorations);
    }

    /// <summary>
    /// Whether or not the window should have shadow.
    /// Platform-specific: Windows: `false` has no effect on decorated window, shadows are always ON. `true` will make undecorated window have a 1px white border, and on Windows 11, it will have a rounded corners. Linux: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="enable">Whether the window should have shadow.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetShadow(this ITauriWindow window, bool enable)
    {
        await window.JsObjectRef.InvokeVoidAsync("setShadow", enable);
    }

    /// <summary>
    /// Set window effects.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="effects">The window effects to apply.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetEffects(this ITauriWindow window, Effects effects)
    {
        await window.JsObjectRef.InvokeVoidAsync("setEffects", effects);
    }

    /// <summary>
    /// Clear any applied effects if possible.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task ClearEffects(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("clearEffects");
    }

    /// <summary>
    /// Whether the window should always be on top of other windows.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="alwaysOnTop">Whether the window should always be on top of other windows or not.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetAlwaysOnTop(this ITauriWindow window, bool alwaysOnTop)
    {
        await window.JsObjectRef.InvokeVoidAsync("setAlwaysOnTop", alwaysOnTop);
    }

    /// <summary>
    /// Whether the window should always be below other windows.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="alwaysOnBottom">Whether the window should always be below other windows or not.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetAlwaysOnBottom(this ITauriWindow window, bool alwaysOnBottom)
    {
        await window.JsObjectRef.InvokeVoidAsync("setAlwaysOnBottom", alwaysOnBottom);
    }

    /// <summary>
    /// Prevents the window contents from being captured by other apps.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="contentProtected">Whether to protect the window content.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetContentProtected(this ITauriWindow window, bool contentProtected)
    {
        await window.JsObjectRef.InvokeVoidAsync("setContentProtected", contentProtected);
    }

    /// <summary>
    /// Resizes the window with a new inner size.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="size">The logical or physical inner size.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetSize(this ITauriWindow window, object size)
    {
        // TODO: Support LogicalSize | PhysicalSize | Size union type
        await window.JsObjectRef.InvokeVoidAsync("setSize", size);
    }

    /// <summary>
    /// Sets the window minimum inner size. If the `size` argument is not provided, the constraint is unset.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="size">The logical or physical inner size, or `null` to unset the constraint.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetMinSize(this ITauriWindow window, object? size)
    {
        // TODO: Support LogicalSize | PhysicalSize | Size | null union type
        await window.JsObjectRef.InvokeVoidAsync("setMinSize", size);
    }

    /// <summary>
    /// Sets the window maximum inner size. If the `size` argument is undefined, the constraint is unset.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="size">The logical or physical inner size, or `null` to unset the constraint.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetMaxSize(this ITauriWindow window, object? size)
    {
        // TODO: Support LogicalSize | PhysicalSize | Size | null union type
        await window.JsObjectRef.InvokeVoidAsync("setMaxSize", size);
    }

    /// <summary>
    /// Sets the window inner size constraints.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="constraints">The window size constraints, or `null` to unset the constraint.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetSizeConstraints(this ITauriWindow window, WindowSizeConstraints? constraints)
    {
        await window.JsObjectRef.InvokeVoidAsync("setSizeConstraints", constraints);
    }

    /// <summary>
    /// Sets the window outer position.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="position">The new position, in logical or physical pixels.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetPosition(this ITauriWindow window, object position)
    {
        // TODO: Support LogicalPosition | PhysicalPosition | Position union type
        await window.JsObjectRef.InvokeVoidAsync("setPosition", position);
    }

    /// <summary>
    /// Sets the window fullscreen state.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="fullscreen">Whether the window should go to fullscreen or not.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetFullscreen(this ITauriWindow window, bool fullscreen)
    {
        await window.JsObjectRef.InvokeVoidAsync("setFullscreen", fullscreen);
    }

    /// <summary>
    /// On macOS, Toggles a fullscreen mode that doesn't require a new macOS space. Returns a boolean indicating whether the transition was successful (this won't work if the window was already in the native fullscreen).
    /// This is how fullscreen used to work on macOS in versions before Lion. And allows the user to have a fullscreen window without using another space or taking control over the entire monitor.
    /// On other platforms, this is the same as <see cref="SetFullscreen"/>.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="fullscreen">Whether the window should go to simple fullscreen or not.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetSimpleFullscreen(this ITauriWindow window, bool fullscreen)
    {
        await window.JsObjectRef.InvokeVoidAsync("setSimpleFullscreen", fullscreen);
    }

    /// <summary>
    /// Bring the window to front and focus.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetFocus(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("setFocus");
    }

    /// <summary>
    /// Sets the window icon.
    /// Note that you may need the `image-ico` or `image-png` Cargo features to use this API.
    /// To enable it, change your Cargo.toml file:
    /// [dependencies]
    /// tauri = { version = "...", features = ["...", "image-png"] }
    /// </summary>
    /// <param name="window"></param>
    /// <param name="icon">Icon bytes or path to the icon file.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetIcon(this ITauriWindow window, object icon)
    {
        // TODO: Support string | Image | Uint8Array | ArrayBuffer | number[] union type
        await window.JsObjectRef.InvokeVoidAsync("setIcon", icon);
    }

    /// <summary>
    /// Whether the window icon should be hidden from the taskbar or not.
    /// Platform-specific: macOS: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="skip">true to hide window icon, false to show it.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetSkipTaskbar(this ITauriWindow window, bool skip)
    {
        await window.JsObjectRef.InvokeVoidAsync("setSkipTaskbar", skip);
    }

    /// <summary>
    /// Grabs the cursor, preventing it from leaving the window.
    /// There's no guarantee that the cursor will be hidden. You should hide it by yourself if you want so.
    /// Platform-specific: Linux: Unsupported. macOS: This locks the cursor in a fixed location, which looks visually awkward.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="grab">`true` to grab the cursor icon, `false` to release it.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetCursorGrab(this ITauriWindow window, bool grab)
    {
        await window.JsObjectRef.InvokeVoidAsync("setCursorGrab", grab);
    }

    /// <summary>
    /// Modifies the cursor's visibility.
    /// Platform-specific: Windows: The cursor is only hidden within the confines of the window. macOS: The cursor is hidden as long as the window has input focus, even if the cursor is outside of the window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="visible">If `false`, this will hide the cursor. If `true`, this will show the cursor.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetCursorVisible(this ITauriWindow window, bool visible)
    {
        await window.JsObjectRef.InvokeVoidAsync("setCursorVisible", visible);
    }

    /// <summary>
    /// Modifies the cursor icon of the window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="icon">The new cursor icon.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetCursorIcon(this ITauriWindow window, CursorIcon icon)
    {
        await window.JsObjectRef.InvokeVoidAsync("setCursorIcon", icon);
    }

    /// <summary>
    /// Sets the window background color.
    /// Platform-specific: Windows: alpha channel is ignored. iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="color">The background color.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetBackgroundColor(this ITauriWindow window, TauriColor color)
    {
        await window.JsObjectRef.InvokeVoidAsync("setBackgroundColor", color);
    }

    /// <summary>
    /// Changes the position of the cursor in window coordinates.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="position">The new cursor position.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetCursorPosition(this ITauriWindow window, object position)
    {
        // TODO: Support LogicalPosition | PhysicalPosition | Position union type
        await window.JsObjectRef.InvokeVoidAsync("setCursorPosition", position);
    }

    /// <summary>
    /// Changes the cursor events behavior.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="ignore">`true` to ignore the cursor events; `false` to process them as usual.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetIgnoreCursorEvents(this ITauriWindow window, bool ignore)
    {
        await window.JsObjectRef.InvokeVoidAsync("setIgnoreCursorEvents", ignore);
    }

    /// <summary>
    /// Starts dragging the window.
    /// </summary>
    /// <param name="window"></param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task StartDragging(this ITauriWindow window)
    {
        await window.JsObjectRef.InvokeVoidAsync("startDragging");
    }

    /// <summary>
    /// Starts resize-dragging the window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="direction">The resize direction.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task StartResizeDragging(this ITauriWindow window, ResizeDirection direction)
    {
        await window.JsObjectRef.InvokeVoidAsync("startResizeDragging", direction);
    }

    /// <summary>
    /// Sets the badge count. It is app wide and not specific to this window.
    /// Platform-specific: Windows: Unsupported. Use <see cref="SetOverlayIcon"/> instead.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="count">The badge count. Use `null` to remove the badge.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetBadgeCount(this ITauriWindow window, int? count = null)
    {
        await window.JsObjectRef.InvokeVoidAsync("setBadgeCount", count);
    }

    /// <summary>
    /// Sets the badge label. macOS only.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="label">The badge label. Use `null` to remove the badge.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetBadgeLabel(this ITauriWindow window, string? label = null)
    {
        await window.JsObjectRef.InvokeVoidAsync("setBadgeLabel", label);
    }

    /// <summary>
    /// Sets the overlay icon. Windows only.
    /// The overlay icon can be set for every window.
    /// Note that you may need the `image-ico` or `image-png` Cargo features to use this API.
    /// To enable it, change your Cargo.toml file:
    /// [dependencies]
    /// tauri = { version = "...", features = ["...", "image-png"] }
    /// </summary>
    /// <param name="window"></param>
    /// <param name="icon">Icon bytes or path to the icon file. Use `null` to remove the overlay icon.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    private static async Task SetOverlayIcon(this ITauriWindow window, object? icon = null)
    {
        // TODO: Support string | Image | Uint8Array | ArrayBuffer | number[] union type
        await window.JsObjectRef.InvokeVoidAsync("setOverlayIcon", icon);
    }

    /// <summary>
    /// Sets the taskbar progress state.
    /// Platform-specific: Linux / macOS: Progress bar is app-wide and not specific to this window. Linux: Only supported desktop environments with `libunity` (e.g. GNOME).
    /// </summary>
    /// <param name="window"></param>
    /// <param name="state">The progress bar state.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetProgressBar(this ITauriWindow window, ProgressBarState state)
    {
        await window.JsObjectRef.InvokeVoidAsync("setProgressBar", state);
    }

    /// <summary>
    /// Sets whether the window should be visible on all workspaces or virtual desktops.
    /// Platform-specific: Windows / iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="visible">Whether the window should be visible on all workspaces.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetVisibleOnAllWorkspaces(this ITauriWindow window, bool visible)
    {
        await window.JsObjectRef.InvokeVoidAsync("setVisibleOnAllWorkspaces", visible);
    }

    /// <summary>
    /// Sets the title bar style. macOS only.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="style">The title bar style.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetTitleBarStyle(this ITauriWindow window, TitleBarStyle style)
    {
        await window.JsObjectRef.InvokeVoidAsync("setTitleBarStyle", style);
    }

    /// <summary>
    /// Set window theme, pass in `null` to follow system theme.
    /// Platform-specific: Linux / macOS: Theme is app-wide and not specific to this window. iOS / Android: Unsupported.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="theme">The theme to set, or `null` to follow system theme.</param>
    /// <returns>A promise indicating the success or failure of the operation.</returns>
    public static async Task SetTheme(this ITauriWindow window, Theme? theme = null)
    {
        await window.JsObjectRef.InvokeVoidAsync("setTheme", theme);
    }
}