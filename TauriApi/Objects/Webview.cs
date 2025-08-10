using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi;

/// <inheritdoc />
public class Webview : ITauriWebview
{
    internal Webview(IJSObjectReference webviewRef, TauriJsInterop tauriJsInterop)
    {
        _tauriJsInterop = tauriJsInterop;
        JsObjectRef = webviewRef;
    }

    private readonly TauriJsInterop _tauriJsInterop;

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }

    /// <inheritdoc />
    public ValueTask<string> Label => _tauriJsInterop.GetJsProperty<string>(JsObjectRef, "label");
}

/// <summary>
/// Webview Methods
/// </summary>
public static class WebviewExtensions
{
    /// <summary>
    /// Emits an event to all targets.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task Emit(this ITauriWebview webview, string eventName, object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emit", eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given target label.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="targetLabel">Label of the target Window/Webview/WebviewWindow</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo(this ITauriWebview webview, string targetLabel, string eventName,
        object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", targetLabel, eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="target">Raw <see cref="EventTarget"/> object.</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo<T>(this ITauriWebview webview, T target, string eventName, object? payload = null)
        where T : EventTarget
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", target, eventName, payload);
    }
    
    /// <summary>
    /// Set webview zoom level.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="scaleFactor"></param>
    public static async Task SetZoom(this ITauriWebview webview, double scaleFactor)
    {
        await webview.JsObjectRef.InvokeVoidAsync("setZoom", scaleFactor);
    }
}

#region Interfaces

/// <summary>
/// Options for creating a webview.
/// </summary>
public record WebviewOptions
{
    /// <summary>
    /// Remote URL or local file path to open.
    /// </summary>
    /// <remarks>
    /// <para>URL such as <c>https://github.com/tauri-apps</c> is opened directly on a Tauri webview.</para>
    /// <para>data: URL such as <c>data:text/html,&lt;html&gt;...</c> is only supported with the <c>webview-data-url</c> Cargo feature for the <c>tauri</c> dependency.</para>
    /// <para>local file path or route such as <c>/path/to/page.html</c> or <c>/users</c> is appended to the application URL (the devServer URL on development, or <c>tauri://localhost/</c> and <c>https://tauri.localhost/</c> on production).</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }

    /// <summary>
    /// The initial vertical position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? X { get; init; }

    /// <summary>
    /// The initial horizontal position.
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
    /// Whether the webview is transparent or not.
    /// </summary>
    /// <remarks>
    /// <para>Note that on <c>macOS</c> this requires the <c>macos-private-api</c> feature flag, enabled under <c>tauri.conf.json &gt; app &gt; macOSPrivateApi</c>.</para>
    /// <para><strong>WARNING:</strong> Using private APIs on <c>macOS</c> prevents your application from being accepted to the <c>App Store</c>.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Transparent { get; init; }

    /// <summary>
    /// Whether the webview should have focus or not.
    /// </summary>
    /// <remarks>
    /// Since version 2.1.0
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Focus { get; init; }

    /// <summary>
    /// Whether the drag and drop is enabled or not on the webview. By default, it is enabled.
    /// Disabling it is required to use HTML5 drag and drop on the frontend on Windows.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DragDropEnabled { get; init; }

    /// <summary>
    /// Whether clicking an inactive webview also clicks through to the webview on macOS.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AcceptFirstMouse { get; init; }

    /// <summary>
    /// The user agent for the webview.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserAgent { get; init; }

    /// <summary>
    /// Whether the webview should be launched in incognito mode.
    /// </summary>
    /// <remarks>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>Android:</strong> Unsupported.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Incognito { get; init; }

    /// <summary>
    /// The proxy URL for the WebView for all network requests.
    /// </summary>
    /// <remarks>
    /// <para>Must be either a <c>http://</c> or a <c>socks5://</c> URL.</para>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>macOS:</strong> Requires the <c>macos-proxy</c> feature flag and only compiles for macOS 14+.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyUrl { get; init; }

    /// <summary>
    /// Whether page zooming by hotkeys is enabled.
    /// </summary>
    /// <remarks>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>Windows:</strong> Controls WebView2's <c>IsZoomControlEnabled</c> setting.</para>
    /// <para><strong>MacOS / Linux:</strong> Injects a polyfill that zooms in and out with <c>ctrl/command</c> + <c>-/=</c>, 20% in each step, ranging from 20% to 1000%. Requires <c>webview:allow-set-webview-zoom</c> permission.</para>
    /// <para><strong>Android / iOS:</strong> Unsupported.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ZoomHotkeysEnabled { get; init; }

    /// <summary>
    /// Sets whether the custom protocols should use <c>https://&lt;scheme&gt;.localhost</c> instead of the default <c>http://&lt;scheme&gt;.localhost</c> on Windows and Android. Defaults to <c>false</c>.
    /// </summary>
    /// <remarks>
    /// <para><strong>Note:</strong></para>
    /// <para>Using a <c>https</c> scheme will NOT allow mixed content when trying to fetch <c>http</c> endpoints and therefore will not match the behavior of the <c>&lt;scheme&gt;://localhost</c> protocols used on macOS and Linux.</para>
    /// <para><strong>Warning:</strong></para>
    /// <para>Changing this value between releases will change the IndexedDB, cookies and localstorage location and your app will not be able to access them.</para>
    /// <para>Since version 2.1.0</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseHttpsScheme { get; init; }

    /// <summary>
    /// Whether web inspector, which is usually called browser devtools, is enabled or not. Enabled by default.
    /// </summary>
    /// <remarks>
    /// <para>This API works in <strong>debug</strong> builds, but requires <c>devtools</c> feature flag to enable it in <strong>release</strong> builds.</para>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>macOS:</strong> This will call private functions on <strong>macOS</strong>.</para>
    /// <para><strong>Android:</strong> Open <c>chrome://inspect/#devices</c> in Chrome to get the devtools window. Wry's <c>WebView</c> devtools API isn't supported on Android.</para>
    /// <para><strong>iOS:</strong> Open Safari &gt; Develop &gt; [Your Device Name] &gt; [Your WebView] to get the devtools window.</para>
    /// <para>Since version 2.1.0</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Devtools { get; init; }

    // /// <summary>
    // /// Set the window and webview background color.
    // /// </summary>
    // /// <remarks>
    // /// <para><strong>Platform-specific:</strong></para>
    // /// <para><strong>macOS / iOS:</strong> Not implemented.</para>
    // /// <para><strong>Windows:</strong></para>
    // /// <para>- On Windows 7, alpha channel is ignored.</para>
    // /// <para>- On Windows 8 and newer, if alpha channel is not <c>0</c>, it will be ignored.</para>
    // /// <para>Since version 2.1.0</para>
    // /// </remarks>
    // public Color? BackgroundColor { get; init; }
}

#endregion
